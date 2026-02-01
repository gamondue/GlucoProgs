<#
Scan all .xaml files for {StaticResource KEY} usages and add any missing keys as Color entries
into "GlucoMan.Maui/Resources/Styles/Styles.xaml". The script is idempotent and will not add
keys that already exist.

Usage:
  From repository root (where this script is located), run in PowerShell:
    pwsh ./scripts/add-fallback-resources.ps1

Note: Review the added colors after running; default color used is #CCCCCC.
#>

Set-StrictMode -Version Latest

$repoRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$stylesFile = Join-Path $repoRoot "GlucoMan.Maui\Resources\Styles\Styles.xaml"
$colorsFile = Join-Path $repoRoot "GlucoMan.Maui\Resources\Styles\Colors.xaml"

if (-not (Test-Path $stylesFile)) {
    Write-Error "Styles.xaml not found at $stylesFile"
    exit 1
}

# collect declared resource keys from Styles.xaml and Colors.xaml (x:Key and Color keys)
$declared = @{}
$declFiles = @($stylesFile, $colorsFile) | Where-Object { Test-Path $_ }
foreach ($f in $declFiles) {
    $content = Get-Content $f -Raw
    $matches = [regex]::Matches($content, 'x:Key\s*=\s*"([^"]+)"')
    foreach ($m in $matches) { $declared[$m.Groups[1].Value] = $true }
    # also capture Color x:Key="..." (already covered) but keep for safety
    $matches2 = [regex]::Matches($content, '<Color\s+x:Key="([^"]+)"')
    foreach ($m in $matches2) { $declared[$m.Groups[1].Value] = $true }
}

# scan all xaml files for StaticResource usages
$referenced = @{}
Get-ChildItem -Recurse -Include *.xaml | ForEach-Object {
    try {
        $t = Get-Content $_.FullName -Raw
    } catch { $t = "" }
    if ($t.Length -eq 0) { return }
    $rx = [regex] '\{StaticResource\s+([^\}\s]+)\}'
    $ms = $rx.Matches($t)
    foreach ($m in $ms) { $referenced[$m.Groups[1].Value] = $true }
}

# compute missing keys
$missing = @()
foreach ($k in $referenced.Keys) {
    if (-not $declared.ContainsKey($k)) { $missing += $k }
}

if ($missing.Count -eq 0) {
    Write-Output "No missing StaticResource keys detected."
    exit 0
}

Write-Output "Missing keys to add to Styles.xaml:`n" + ($missing -join "`n")

# Prepare default Color entries to insert
$insertions = ""
foreach ($k in $missing) {
    # choose a default color or brush; default is light gray for unknown keys
    $color = '#CCCCCC'
    # build string with concatenation to avoid quoting/interpolation issues
    $insertions += '    <Color x:Key="' + $k + '">' + $color + '</Color>' + "`n"
}

# Insert before the closing ResourceDictionary tag
$stylesText = Get-Content $stylesFile -Raw
$closingTag = '</ResourceDictionary>'
if ($stylesText -notlike "*${closingTag}*") {
    Write-Error "Unexpected Styles.xaml format (no closing ResourceDictionary tag)."
    exit 1
}

# Ensure we don't add duplicates if script run twice
foreach ($k in $missing) {
    # use escaped key when matching
    $escapedKey = [regex]::Escape($k)
    if ($stylesText -match ('x:Key="' + $escapedKey + '"')) {
        Write-Output "Key $k already present in Styles.xaml, skipping."
        # remove corresponding insertion if already present
        $pattern = '\\s*<Color x:Key="' + $escapedKey + '">.*?</Color>\r?\n'
        $insertions = [regex]::Replace($insertions, $pattern, '', [System.Text.RegularExpressions.RegexOptions]::Singleline)
    }
}

if ($insertions.Trim().Length -eq 0) {
    Write-Output "No new insertions required after duplicate check."
    exit 0
}

$newStyles = $stylesText -replace [regex]::Escape($closingTag), "$insertions$closingTag"

# Backup original
Copy-Item $stylesFile "$stylesFile.bak" -Force

Set-Content -Path $stylesFile -Value $newStyles -Encoding UTF8

Write-Output "Inserted missing keys into Styles.xaml and backed up original to Styles.xaml.bak"
exit 0
