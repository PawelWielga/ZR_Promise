@echo off
setlocal ENABLEDELAYEDEXPANSION

rem === USTAWIENIA PORTÓW I ŚCIEŻEK ===
set SOLUTION=AppAudit.sln
set API_PROJ=AppAudit.Api\AppAudit.Api.csproj
set WEB_PROJ=AppAudit.Web\AppAudit.Web.csproj
set COLLECTOR_PROJ=AppAudit.Collector\AppAudit.Collector.csproj

set API_URL=http://localhost:5000
set WEB_URL=http://localhost:5002

rem === SPRAWDZENIE dotnet ===
where dotnet >NUL 2>&1
if errorlevel 1 (
  echo [ERR] Nie znaleziono polecenia "dotnet". Zainstaluj .NET SDK i dodaj do PATH.
  exit /b 1
)

rem === BUDOWANIE ROZWIĄZANIA ===
echo [BUILD] Buduję rozwiazanie: %SOLUTION%
dotnet build "%SOLUTION%" -c Debug
if errorlevel 1 (
  echo [ERR] Build nieudany.
  exit /b 1
)

rem === START API (ustawiamy port) ===
echo [RUN] Uruchamiam API na %API_URL%
start "AppAudit.API" cmd /k ^
  "set ASPNETCORE_URLS=%API_URL% && dotnet run --project "%API_PROJ%" --no-build -c Debug"

rem === CZEKAJ, AZ API WSTANIE ===
echo [WAIT] Czekam 5s na start API...
timeout /t 5 /nobreak >NUL

rem === START WEB (ustawiamy port) ===
echo [RUN] Uruchamiam WEB na %WEB_URL%
start "AppAudit.WEB" cmd /k ^
  "set ASPNETCORE_URLS=%WEB_URL% && dotnet run --project "%WEB_PROJ%" --no-build -c Debug"

rem === CZEKAJ CHWILĘ PRZED COLLECTOREM ===
echo [WAIT] Czekam 3s przed startem Collectora...
timeout /t 3 /nobreak >NUL

rem === START COLLECTOR (wskazujemy endpoint API i interwał) ===
echo [RUN] Uruchamiam Collector (API=%API_URL%)
start "AppAudit.COLLECTOR" cmd /k ^
  "dotnet run --project "%COLLECTOR_PROJ%" --no-build -c Debug -- --api %API_URL% --minutes 5"

echo [OK] Wszystko odpalone w osobnych oknach.
endlocal
