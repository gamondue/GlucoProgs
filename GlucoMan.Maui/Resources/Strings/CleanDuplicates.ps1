# ========================================
# PowerShell Script to Remove Duplicates from .resx Files
# ========================================

Write-Host "=== Cleaning AppStrings.resx and AppStrings.it.resx ===" -ForegroundColor Cyan
Write-Host ""

$resxPath = "C:\Develop\Git\GlucoProgs\GlucoMan.Maui\Resources\Strings\AppStrings.resx"
$resxItPath = "C:\Develop\Git\GlucoProgs\GlucoMan.Maui\Resources\Strings\AppStrings.it.resx"

# Function to remove duplicate section
function Remove-DuplicateSection {
    param (
        [string]$FilePath,
        [string]$StartMarker
    )
    
    Write-Host "Processing: $FilePath" -ForegroundColor Yellow
    
    if (-not (Test-Path $FilePath)) {
        Write-Host "  ERROR: File not found!" -ForegroundColor Red
        return $false
    }
    
    # Read file content
    $content = Get-Content -Path $FilePath -Raw
    
    # Find the start of duplicate section
    $startIndex = $content.IndexOf($StartMarker)
    
    if ($startIndex -eq -1) {
        Write-Host "  No duplicate section found. File is clean!" -ForegroundColor Green
        return $true
    }
    
    # Find </root> tag
    $endIndex = $content.LastIndexOf("</root>")
    
    if ($endIndex -eq -1) {
        Write-Host "  ERROR: </root> tag not found!" -ForegroundColor Red
        return $false
    }
    
    # Extract clean content (everything before the duplicate section + </root>)
    $cleanContent = $content.Substring(0, $startIndex) + "</root>"
    
    # Create backup
    $backupPath = $FilePath + ".backup"
    Copy-Item -Path $FilePath -Destination $backupPath -Force
    Write-Host "  Backup created: $backupPath" -ForegroundColor Gray
    
    # Write cleaned content
    Set-Content -Path $FilePath -Value $cleanContent -NoNewline -Encoding UTF8
    
    # Count removed lines
    $originalLines = ($content -split "`n").Count
    $newLines = ($cleanContent -split "`n").Count
    $removedLines = $originalLines - $newLines
    
    Write-Host "  SUCCESS: Removed $removedLines duplicate lines" -ForegroundColor Green
    return $true
}

# Clean AppStrings.resx
Write-Host ""
Write-Host "1. Cleaning AppStrings.resx (English)" -ForegroundColor Cyan
$marker1 = "  <!-- ============================================ -->"
$success1 = Remove-DuplicateSection -FilePath $resxPath -StartMarker $marker1

# Clean AppStrings.it.resx
Write-Host ""
Write-Host "2. Cleaning AppStrings.it.resx (Italian)" -ForegroundColor Cyan
$marker2 = "  <!-- FoodToHitTargetCarbsPage (IT) -->"
$success2 = Remove-DuplicateSection -FilePath $resxItPath -StartMarker $marker2

# Summary
Write-Host ""
Write-Host "=== SUMMARY ===" -ForegroundColor Cyan
if ($success1 -and $success2) {
    Write-Host "? Both files cleaned successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Yellow
    Write-Host "  1. Open Visual Studio" -ForegroundColor White
    Write-Host "  2. Build ? Rebuild Solution" -ForegroundColor White
    Write-Host "  3. Check for remaining XHR0012 errors" -ForegroundColor White
    Write-Host ""
    Write-Host "Backup files created (you can delete them after verification):" -ForegroundColor Gray
    Write-Host "  - $resxPath.backup"
    Write-Host "  - $resxItPath.backup"
} else {
    Write-Host "? Errors occurred during cleaning. Check messages above." -ForegroundColor Red
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
