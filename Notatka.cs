using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

// ============================================================================
// LEKCJA: SQLite — lokalna baza danych
// Plik: Notatka.cs — model danych
// ============================================================================
//
// >>> Sekcja 4: Model danych — klasa z atrybutami
//
// Każda publiczna właściwość z get/set automatycznie staje się
// kolumną w tabeli SQLite. Atrybuty [PrimaryKey] i [AutoIncrement]
// definiują klucz główny z automatycznym numerowaniem.
//
// ============================================================================


namespace SQLiteDemo;
/// <summary>
/// Model danych reprezentujący jedną notatkę w bazie SQLite.
/// >>> Sekcja 4.1: [PrimaryKey, AutoIncrement]
/// >>> Sekcja 4.2: Właściwości jako kolumny tabeli
/// </summary>
public class Notatka
{
    // >>> Sekcja 4.1 — Klucz główny z automatycznym numerowaniem
    // Nowy obiekt ma Id = 0. Po zapisie do bazy — SQLite nada Id (1, 2, 3...)
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // >>> Sekcja 4.2 — Właściwości automatycznie mapowane na kolumny
    // string → TEXT, DateTime → TEXT (ISO 8601)
    public string Tytul { get; set; }

    public string Tresc { get; set; }

    public DateTime DataUtworzenia { get; set; }
}
