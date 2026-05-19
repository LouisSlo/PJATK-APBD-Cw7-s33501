# APBD - Ćwiczenie 7: REST API 

## Opis projektu
Celem projektu było stworzenie aplikacji REST Web API służącej do zarządzania zestawami komputerowymi oraz ich komponentami. [cite_start]Aplikacja została zrealizowana z wykorzystaniem platformy ASP.NET Core oraz Entity Framework Core, ściśle trzymając się podejścia Code First[cite: 3, 5, 8].

Zgodnie z dobrymi praktykami architektonicznymi, aplikacja nie wystawia bezpośrednio encji bazodanowych na zewnątrz. [cite_start]Do komunikacji z klientem (dane wejściowe i wyjściowe) wykorzystano osobne klasy DTO (Data Transfer Object)

## Wykorzystane technologie
* C# / .NET 10.0 (ASP.NET Core Web API)
* Entity Framework Core (SQL Server)
* Docker (do hostowania środowiska bazy danych)
* Swagger / Swashbuckle (do dokumentacji i testowania endpointów)

## Zaimplementowane endpointy
Aplikacja udostępnia następujące operacje:
* GET /api/pcs - pobieranie listy wszystkich komputerów
* GET /api/pcs/{id}/components - pobieranie szczegółowych informacji o wybranym komputerze wraz z przypisaną do niego listą komponentów, typów oraz producentów [cite: 92-94]. Wymagało to zaawansowanego złączenia tabel i mapowania danych.
* POST /api/pcs - dodawanie nowego komputera do bazy 
* PUT /api/pcs/{id} - aktualizacja danych istniejącego komputera 
* DELETE /api/pcs/{id} - usuwanie komputera o wskazanym identyfikatorze 

## Baza danych i Migracje
Struktura bazy danych została utworzona bez pisania bezpośredniego kodu SQL, wykorzystując mechanizm migracji Entity Framework Core. 

* Modele w C# (Encje) zostały ręcznie zdefiniowane na podstawie dostarczonego diagramu.
* W klasie konfiguracyjnej `AppDbContext` zastosowano Fluent API, aby precyzyjnie określić ograniczenia bazy danych: maksymalne długości znaków, odpowiednie typy (np. `date` vs `datetime2`), właściwości nawigacyjne oraz klucze złożone dla tabeli łączącej `PCComponents` .
* Dzięki wygenerowaniu migracji (`InitialCreate`), polecenie `database update` automatycznie utworzyło odpowiednie tabele w SQL Serverze.
* Wykorzystano również mechanizm `HasData`, co pozwoliło na automatyczne wypełnienie wygenerowanej bazy danych początkowymi rekordami (seedowanie danych) już podczas uruchamiania migracji.
