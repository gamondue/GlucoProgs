<#
.SYNOPSIS
    Converte i file .csproj per usare Central Package Management

.DESCRIPTION
    Questo script rimuove automaticamente gli attributi Version dai PackageReference
    nei file .csproj, permettendo di usare Directory.Packages.props per la gestione
    centralizzata delle versioni.

.PARAMETER DryRun
    Se specificato, mostra cosa verrebbe fatto senza modificare i file

.EXAMPLE
    .\Convert-ToCentralPackageManagement.ps1
    Converte tutti i file .csproj nella directory corrente e sottodirectory

.EXAMPLE
    .\Convert-ToCentralPackageManagement.ps1 -DryRun
    Mostra cosa verrebbe fatto senza modificare i file
#>

param(
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host "  Central Package Management - Conversion Tool" -ForegroundColor Cyan
Write-Host "=====================================================" -ForegroundColor Cyan
Write-Host ""

# Trova tutti i file .csproj
$projectFiles = Get-ChildItem -Path . -Filter "*.csproj" -Recurse

if ($projectFiles.Count -eq 0) {
    Write-Host "Nessun file .csproj trovato!" -ForegroundColor Red
    exit 1
}

Write-Host "Trovati $($projectFiles.Count) file(i) .csproj:" -ForegroundColor Green
foreach ($file in $projectFiles) {
    Write-Host "  - $($file.FullName)" -ForegroundColor Gray
}
Write-Host ""

if ($DryRun) {
    Write-Host "MODALITÀ DRY-RUN: Nessun file verrà modificato" -ForegroundColor Yellow
    Write-Host ""
}

$totalChanges = 0

foreach ($projectFile in $projectFiles) {
    Write-Host "Elaborazione: $($projectFile.Name)" -ForegroundColor Cyan
    
    # Leggi il contenuto
    $content = Get-Content $projectFile.FullName -Raw
    $originalContent = $content
    
    # Pattern regex per trovare PackageReference con Version
    # Cattura: <PackageReference Include="NomePackage" Version="X.Y.Z" />
    # Oppure: <PackageReference Include="NomePackage" Version="X.Y.Z">
    $pattern = '(<PackageReference\s+Include="[^"]+"\s+)Version="[^"]+"(\s*/?)'
    
    # Conta quante modifiche faremo
    $matches = [regex]::Matches($content, $pattern)
    $changeCount = $matches.Count
    
    if ($changeCount -eq 0) {
        Write-Host "  ? Nessuna modifica necessaria (già convertito o nessun PackageReference)" -ForegroundColor Gray
        Write-Host ""
        continue
    }
    
    Write-Host "  Trovati $changeCount PackageReference con Version:" -ForegroundColor Yellow
    
    # Mostra cosa stiamo per cambiare
    foreach ($match in $matches) {
        $fullMatch = $match.Value
        $packageName = if ($fullMatch -match 'Include="([^"]+)"') { $matches[0].Groups[1].Value } else { "Unknown" }
        $version = if ($fullMatch -match 'Version="([^"]+)"') { $matches[0].Groups[1].Value } else { "Unknown" }
        
        Write-Host "    - $packageName (Version=$version)" -ForegroundColor Gray
    }
    
    if (-not $DryRun) {
        # Esegui la sostituzione: rimuovi l'attributo Version
        $newContent = [regex]::Replace($content, $pattern, '$1$2')
        
        # Backup del file originale
        $backupPath = "$($projectFile.FullName).backup"
        Copy-Item -Path $projectFile.FullName -Destination $backupPath -Force
        
        # Salva il nuovo contenuto
        Set-Content -Path $projectFile.FullName -Value $newContent -NoNewline
        
        Write-Host "  ? Modificato (backup: $($projectFile.Name).backup)" -ForegroundColor Green
        $totalChanges += $changeCount
    } else {
        Write-Host "  [DRY-RUN] Verrebbero rimossi $changeCount attributi Version" -ForegroundColor Yellow
        $totalChanges += $changeCount
    }
    
    Write-Host ""
}

Write-Host "=====================================================" -ForegroundColor Cyan
if ($DryRun) {
    Write-Host "DRY-RUN COMPLETATO" -ForegroundColor Yellow
    Write-Host "Verrebbero modificati $totalChanges PackageReference in totale" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Per applicare le modifiche esegui:" -ForegroundColor Cyan
    Write-Host "  .\Convert-ToCentralPackageManagement.ps1" -ForegroundColor White
} else {
    Write-Host "CONVERSIONE COMPLETATA!" -ForegroundColor Green
    Write-Host "Modificati $totalChanges PackageReference in totale" -ForegroundColor Green
    Write-Host ""
    Write-Host "PROSSIMI PASSI:" -ForegroundColor Cyan
    Write-Host "1. Verifica che Directory.Packages.props contenga tutti i pacchetti" -ForegroundColor White
    Write-Host "2. Esegui: dotnet restore" -ForegroundColor White
    Write-Host "3. Esegui: dotnet build" -ForegroundColor White
    Write-Host "4. Se tutto funziona, elimina i file .backup" -ForegroundColor White
    Write-Host ""
    Write-Host "Se qualcosa va storto:" -ForegroundColor Yellow
    Write-Host "  - I backup sono in: *.csproj.backup" -ForegroundColor Gray
    Write-Host "  - Ripristina con: Get-ChildItem *.backup -Recurse | ForEach-Object { Copy-Item `$_.FullName (`$_.FullName -replace '.backup','') -Force }" -ForegroundColor Gray
}
Write-Host "=====================================================" -ForegroundColor Cyan
