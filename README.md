# AppAudit

Aplikacja demonstracyjna do audytu zainstalowanych programów. Projekt składa się z kilku modułów, które współpracują ze sobą w architekturze warstwowej.

## Struktura projektu
- **AppAudit.Api** – minimalne API odpowiedzialne za udostępnianie danych i generowanie dokumentacji Swagger.
- **AppAudit.Application** – warstwa aplikacyjna z obsługą logiki biznesowej i mediatorów.
- **AppAudit.Contracts** – współdzielone kontrakty i modele danych wykorzystywane przez pozostałe moduły.
- **AppAudit.Web** – aplikacja webowa (Blazor Server) komunikująca się z API.
- **AppAudit.Collector** – narzędzie konsolowe zbierające informacje o programach z systemu.
- **AppAudit.Tests** – zestaw testów jednostkowych (NUnit/Moq).

## Wykorzystane narzędzia i biblioteki
Projekt bazuje na platformie **.NET 9.0** i wykorzystuje kilka popularnych pakietów:
- **Entity Framework Core** z bazą **SQLite** do dostępu do danych.
- **MediatR** do implementacji wzorca Mediator.
- **Swashbuckle** i **Microsoft.AspNetCore.OpenApi** do generowania dokumentacji API.
- **CsvHelper** do eksportu danych w formacie CSV.
- **Blazor Server** do budowy warstwy webowej.

## Uruchomienie
1. Zainstaluj [.NET SDK](https://dotnet.microsoft.com/).
2. Przywróć i zbuduj rozwiązanie:
   ```bash
   dotnet build
   ```
3. Uruchom poszczególne moduły w osobnych terminalach:
   - API: `dotnet run --project AppAudit.Api` (domyślnie na `http://localhost:5000`).
   - Interfejs webowy: `dotnet run --project AppAudit.Web` (domyślnie na `http://localhost:5002`).
   - Kolektor: `dotnet run --project AppAudit.Collector -- --api http://localhost:5000 --minutes 5`.
4. W systemie Windows można skorzystać ze skryptu `run_all.bat`, który automatycznie kompiluje rozwiązanie i uruchamia wszystkie trzy moduły.

