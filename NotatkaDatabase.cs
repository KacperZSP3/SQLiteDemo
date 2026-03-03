using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
// ============================================================================
// LEKCJA: SQLite — lokalna baza danych
// Plik: NotatkaDatabase.cs — klasa dostępu do bazy danych
// ============================================================================
//
// SPIS ODNIESIEŃ DO LEKCJI 80:
//   - Init()                  → Sekcja 5.2: Lazy initialization
//   - ZapiszNotatkeAsync()    → Sekcja 6.5: Wzorzec SaveItemAsync
//   - PobierzWszystkieAsync() → Sekcja 6.2: Read — Table<T>().ToListAsync()
//   - PobierzPoIdAsync()      → Sekcja 6.2: Read — Where + FirstOrDefault
//   - UsunNotatkeAsync()      → Sekcja 6.4: Delete — DeleteAsync
//
// ============================================================================

namespace SQLiteDemo;

/// <summary>
/// Klasa dostępu do bazy danych SQLite dla modelu Notatka.
/// >>> Sekcja 5: Klasa dostępu do bazy danych
///
/// Wzorzec: każda metoda publiczna wywołuje Init() na początku,
/// co zapewnia lazy initialization — baza jest tworzona dopiero
/// przy pierwszym użyciu.
/// </summary>
public class NotatkaDatabase
{
    // >>> Sekcja 5.1 — SQLiteAsyncConnection: połączenie asynchroniczne
    // Operacje async nie blokują UI
    SQLiteAsyncConnection database;


    /// <summary>
    /// Lazy initialization — tworzy bazę tylko raz.
    /// >>> Sekcja 5.2: Wzorzec Init
    ///
    /// Jeśli database != null → baza już istnieje, nic nie rób.
    /// Jeśli database == null → stwórz połączenie i tabelę.
    /// </summary>
    async Task Init()
    {
        if (database is not null)
            return;

        // >>> Sekcja 5.1 — Tworzenie połączenia z flagami z Constants
        database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);

        // >>> Sekcja 5.3 — CreateTableAsync tworzy tabelę na podstawie klasy
        // Jeśli tabela już istnieje — nie nadpisuje jej
        await database.CreateTableAsync<Notatka>();
    }


    // ========================================================================
    // OPERACJE CRUD
    // ========================================================================

    /// <summary>
    /// Zapisuje notatkę — insert (nowa) lub update (istniejąca).
    /// >>> Sekcja 6.5: Wzorzec SaveItemAsync
    ///
    /// Id == 0 → nowy rekord → InsertAsync (Sekcja 6.1)
    /// Id != 0 → istniejący rekord → UpdateAsync (Sekcja 6.3)
    /// </summary>
    public async Task<int> ZapiszNotatkeAsync(Notatka notatka)
    {
        await Init();

        if (notatka.Id != 0)
        {
            // >>> Sekcja 6.3 — Update: SQLite szuka rekordu po Id i nadpisuje
            return await database.UpdateAsync(notatka);
        }
        else
        {
            // >>> Sekcja 6.1 — Insert: dodaje nowy rekord, nadaje Id
            return await database.InsertAsync(notatka);
        }
    }


    /// <summary>
    /// Pobiera wszystkie notatki z bazy.
    /// >>> Sekcja 6.2: Read — Table<T>().ToListAsync()
    /// </summary>
    public async Task<List<Notatka>> PobierzWszystkieAsync()
    {
        await Init();
        return await database.Table<Notatka>().ToListAsync();
    }


    /// <summary>
    /// Pobiera jedną notatkę po Id.
    /// >>> Sekcja 6.2: Read — Where + FirstOrDefaultAsync
    ///
    /// Zwraca Notatka lub null jeśli nie znaleziono.
    /// </summary>
    public async Task<Notatka> PobierzPoIdAsync(int id)
    {
        await Init();
        return await database.Table<Notatka>()
            .Where(n => n.Id == id)
            .FirstOrDefaultAsync();
    }


    /// <summary>
    /// Usuwa notatkę z bazy.
    /// >>> Sekcja 6.4: Delete — DeleteAsync
    ///
    /// Rekord identyfikowany po Id (klucz główny).
    /// Zwraca liczbę usuniętych wierszy.
    /// </summary>
    public async Task<int> UsunNotatkeAsync(Notatka notatka)
    {
        await Init();
        return await database.DeleteAsync(notatka);
    }
}

