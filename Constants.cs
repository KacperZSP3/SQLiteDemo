using System;
using System.Collections.Generic;
using System.Text;

// ============================================================================
// LEKCJA: SQLite — lokalna baza danych
// Plik: Constants.cs — konfiguracja bazy danych
// ============================================================================
//
// >>> Sekcja 3: Konfiguracja — klasa Constants
//
// Ta klasa przechowuje konfigurację bazy danych w jednym miejscu:
//   - DatabaseFilename: nazwa pliku bazy (rozszerzenie .db3)
//   - Flags: flagi otwarcia (ReadWrite, Create, SharedCache)
//   - DatabasePath: pełna ścieżka (AppDataDirectory + nazwa pliku)
//
// ============================================================================

namespace SQLiteDemo;

    public static class Constants
    {
        // >>> Sekcja 3.1 — Nazwa pliku bazy danych
        // Rozszerzenie .db3 to konwencja dla plików SQLite
        public const string DatabaseFilename = "LekcjaNotatki.db3";

        // >>> Sekcja 3.2 — Flagi otwarcia bazy danych
        // ReadWrite: odczyt i zapis
        // Create: stwórz plik jeśli nie istnieje
        // SharedCache: współdzielony cache dla lepszej wydajności
        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        // >>> Sekcja 3.1 — Pełna ścieżka do pliku bazy
        // Path.Combine łączy katalog AppDataDirectory z nazwą pliku
        // w sposób bezpieczny dla każdej platformy
        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }

