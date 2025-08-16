@echo off
setlocal ENABLEDELAYEDEXPANSION

rem === USTAW KODOWANIE KONSOLI NA UTF-8 (polskie znaki) ===
chcp 65001 >NUL

rem === USTAWIENIA ROZWIĄZANIA, PROJEKTÓW I PORTÓW ===
set "SOLUTION=AppAudit.sln"
set "API_PROJ=AppAudit.Api\AppAudit.Api.csproj"
set "WEB_PROJ=AppAudit.Web\AppAudit.Web.csproj"
set "COLLECTOR_PROJ=AppAudit.Collector\AppAudit.Collector.csproj"

set "API_URL=http://localhost:5000"
set "WEB_URL=http://localhost:5002"

rem (opcjonalnie interfejs CLI po polsku)
set "DOTNET_CLI_UI_LANGUAGE=pl"

rem === SPRAWDZENIE dotnet ===
where dotnet >NUL 2>&1
if errorlevel 1 (
  echo [BŁĄD] Nie znaleziono polecenia "dotnet". Zainstaluj .NET SDK i dodaj do PATH.
  exit /b 1
)

rem === CLEAN + RESTORE + REBUILD (świeży build) ===
echo [CLEAN] Czyszczę rozwiazanie: %SOLUTION%
dotnet clean "%SOLUTION%" -c Debug
if errorlevel 1 (
  echo [BŁĄD] Clean nieudany.
  exit /b 1
)

echo [RESTORE] Przywracam pakiety...
dotnet restore "%SOLUTION%"
if errorlevel 1 (
  echo [BŁĄD] Restore nieudany.
  exit /b 1
)

echo [BUILD] Buduję od zera (Rebuild): %SOLUTION%
dotnet build "%SOLUTION%" -c Debug -t:Rebuild --no-incremental
if errorlevel 1 (
  echo [BŁĄD] Build nieudany.
  exit /b 1
)

rem === START API (własne okno, UTF-8 w nowym cmd) ===
echo [RUN] Uruchamiam API na %API_URL%
start "AppAudit.API" cmd /k ^
  chcp 65001^>NUL ^& set "ASPNETCORE_URLS=%API_URL%" ^& dotnet run --project "%API_PROJ%" --no-build -c Debug

rem === CZEKAJ, AŻ API WSTANIE ===
echo [WAIT] Czekam 5 s na start API...
timeout /t 5 /nobreak >NUL

rem === START WEB (własne okno, UTF-8) ===
echo [RUN] Uruchamiam WEB na %WEB_URL%
start "AppAudit.WEB" cmd /k ^
  chcp 65001^>NUL ^& set "ASPNETCORE_URLS=%WEB_URL%" ^& dotnet run --project "%WEB_PROJ%" --no-build -c Debug

rem === CZEKAJ CHWILĘ PRZED COLLECTOREM ===
echo [WAIT] Czekam 3 s przed startem Collectora...
timeout /t 3 /nobreak >NUL

rem === START COLLECTOR (własne okno, UTF-8) ===
echo [RUN] Uruchamiam Collector (API=%API_URL%)
start "AppAudit.COLLECTOR" cmd /k ^
  chcp 65001^>NUL ^& dotnet run --project "%COLLECTOR_PROJ%" --no-build -c Debug -- --api "%API_URL%" --minutes 5

echo [OK] Wszystko odpalone w osobnych oknach.
rem === ZATRZYMAJ GŁÓWNE OKNO ===
echo.
echo Nacisnij dowolny klawisz, aby zamknąć to okno...
pause >nul
endlocal
