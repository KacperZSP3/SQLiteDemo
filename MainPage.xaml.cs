// ============================================================================
// LEKCJA: SQLite — lokalna baza danych
// Plik: MainPage.xaml.cs — logika strony z operacjami CRUD
// ============================================================================
//
// SPIS ODNIESIEŃ DO LEKCJI:
//   - OnZapiszClicked()     → Sekcja 6.5: Wzorzec SaveItemAsync (insert/update)
//   - OnNotatkaSelected()   → Sekcja 6.2, 6.3, 6.4: Read, Update, Delete
//   - OdswiezListeAsync()   → Sekcja 6.2: Read — PobierzWszystkieAsync
//   - OnAppearing()         → Sekcja 7: Użycie w aplikacji
//
// ============================================================================

namespace SQLiteDemo
{
    public partial class MainPage : ContentPage
    {
        // >>> Sekcja 7 — Tworzenie instancji klasy bazodanowej
        // NotatkaDatabase zarządza połączeniem z SQLite
        NotatkaDatabase baza = new NotatkaDatabase();

        // Pole przechowujące notatkę w trybie edycji (null = tryb dodawania)
        Notatka edytowanaNotatka = null;

        public MainPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Odświeża listę notatek przy każdym pojawieniu się strony.
        /// >>> Sekcja 7: Użycie w aplikacji
        ///
        /// OnAppearing jest wywoływane gdy strona staje się widoczna.
        /// Idealne miejsce do odświeżenia danych z bazy.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await OdswiezListeAsync();
        }


        /// <summary>
        /// Pobiera notatki z bazy i przypisuje do CollectionView.
        /// >>> Sekcja 6.2: Read — Table<T>().ToListAsync()
        /// </summary>
        async Task OdswiezListeAsync()
        {
            // >>> Sekcja 6.2 — Pobranie wszystkich rekordów
            var notatki = await baza.PobierzWszystkieAsync();

            // Przypisanie listy do CollectionView
            NotatkiLista.ItemsSource = notatki;

            // Aktualizacja licznika
            LicznikLabel.Text = $"Notatek: {notatki.Count}";
        }


        /// <summary>
        /// Obsługa przycisku "Dodaj notatkę" / "Zapisz zmiany".
        /// >>> Sekcja 6.5: Wzorzec SaveItemAsync
        ///
        /// Jeśli edytowanaNotatka != null → aktualizujemy (Update, Sekcja 6.3)
        /// Jeśli edytowanaNotatka == null → dodajemy nową (Insert, Sekcja 6.1)
        /// </summary>
        async void OnZapiszClicked(object sender, EventArgs e)
        {
            // Walidacja — tytuł nie może być pusty
            if (string.IsNullOrWhiteSpace(TytulEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj tytuł notatki.", "OK");
                return;
            }

            if (edytowanaNotatka != null)
            {
                // --- TRYB EDYCJI ---
                // >>> Sekcja 6.3 — Update: zmiana istniejącej notatki
                edytowanaNotatka.Tytul = TytulEntry.Text.Trim();
                edytowanaNotatka.Tresc = TrescEditor.Text?.Trim() ?? "";

                // >>> Sekcja 6.5 — ZapiszNotatkeAsync wykryje Id != 0 → UpdateAsync
                await baza.ZapiszNotatkeAsync(edytowanaNotatka);

                // Powrót do trybu dodawania
                edytowanaNotatka = null;
                ZapiszButton.Text = "Dodaj notatkę";
            }
            else
            {
                // --- TRYB DODAWANIA ---
                // >>> Sekcja 6.1 — Tworzenie nowego obiektu Notatka
                var notatka = new Notatka
                {
                    Tytul = TytulEntry.Text.Trim(),
                    Tresc = TrescEditor.Text?.Trim() ?? "",
                    DataUtworzenia = DateTime.Now
                    // Id = 0 (domyślnie) → InsertAsync nada Id
                };

                // >>> Sekcja 6.5 — ZapiszNotatkeAsync wykryje Id == 0 → InsertAsync
                await baza.ZapiszNotatkeAsync(notatka);
            }

            // Wyczyść formularz i odśwież listę
            TytulEntry.Text = "";
            TrescEditor.Text = "";
            await OdswiezListeAsync();
        }


        /// <summary>
        /// Reakcja na kliknięcie notatki w liście — menu: Edytuj / Usuń.
        /// >>> Sekcja 6.2: Read (pobieranie wybranej notatki)
        /// >>> Sekcja 6.3: Update (edycja)
        /// >>> Sekcja 6.4: Delete (usuwanie)
        ///
        /// Używa DisplayActionSheet do wyświetlenia opcji.
        /// Przy usuwaniu — DisplayAlert z potwierdzeniem.
        /// </summary>
        async void OnNotatkaSelected(object sender, SelectionChangedEventArgs e)
        {
            // Sprawdź czy wybrano notatkę
            if (e.CurrentSelection.FirstOrDefault() is not Notatka notatka)
                return;

            // Odznacz element (żeby można było kliknąć ponownie)
            NotatkiLista.SelectedItem = null;

            // Pokaż menu opcji
            string akcja = await DisplayActionSheet(
                notatka.Tytul, "Anuluj", null, "Edytuj", "Usuń");

            if (akcja == "Edytuj")
            {
                // --- EDYCJA ---
                // Wypełnij formularz danymi notatki
                edytowanaNotatka = notatka;
                TytulEntry.Text = notatka.Tytul;
                TrescEditor.Text = notatka.Tresc;

                // Zmień tekst przycisku na "Zapisz zmiany"
                ZapiszButton.Text = "Zapisz zmiany";
            }
            else if (akcja == "Usuń")
            {
                // --- USUWANIE ---
                // Potwierdzenie przed usunięciem
                bool potwierdzone = await DisplayAlert(
                    "Potwierdzenie",
                    $"Czy na pewno chcesz usunąć notatkę \"{notatka.Tytul}\"?",
                    "Usuń", "Anuluj");

                if (potwierdzone)
                {
                    // >>> Sekcja 6.4 — DeleteAsync usuwa rekord po Id
                    await baza.UsunNotatkeAsync(notatka);
                    await OdswiezListeAsync();
                }
            }
        }
    }
}
