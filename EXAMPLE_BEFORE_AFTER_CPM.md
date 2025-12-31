# Esempio Prima/Dopo - Central Package Management

## ?? Struttura dei File

```
GlucoProgs/
??? Directory.Packages.props          ? NUOVO! Gestione centralizzata
??? GlucoMan.Maui/
?   ??? GlucoMan.Maui.csproj          ? Modificato (rimossi Version)
??? SharedGlucoMan/
?   ??? SharedGlucoMan.projitems
??? TestGlucoMan/
?   ??? TestGlucoMan.csproj           ? Modificato (rimossi Version)
??? ...
```

---

## ?? PRIMA (senza Central Package Management)

### GlucoMan.Maui.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net9.0-windows;net9.0-android</TargetFrameworks>
    <!-- ... -->
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="CommunityToolkit.Maui" Version="12.2.0" />
    <PackageReference Include="MathNet.Numerics" Version="5.0.0" />
    <PackageReference Include="Microsoft.Data.Sqlite.Core" Version="9.0.9" />
    <PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.9" />
    <PackageReference Include="Microsoft.Maui.Controls" Version="9.0.110" />
    <PackageReference Include="NUnit" Version="4.4.0" />
    <PackageReference Include="SkiaSharp.Views.Maui.Controls" Version="3.116.1" />
    <PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.11" />
  </ItemGroup>

  <!-- Windows-specific -->
  <ItemGroup Condition="$(TargetFramework.Contains('windows'))">
    <PackageReference Include="System.Drawing.Common" Version="9.0.0" />
  </ItemGroup>
</Project>
```

### TestGlucoMan.csproj (ipotetico)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- ATTENZIONE: Versione DIVERSA da GlucoMan.Maui! -->
    <PackageReference Include="Microsoft.Maui.Controls" Version="9.0.100" />
    <PackageReference Include="NUnit" Version="4.4.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="5.1.1" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
  </ItemGroup>
</Project>
```

? **Problemi**:
- `Microsoft.Maui.Controls` ha versioni **diverse** (9.0.110 vs 9.0.100)
- Per aggiornare MAUI devi modificare **2+ file**
- Rischio di dimenticare qualche progetto
- Conflitti di dipendenze potenziali

---

## ?? DOPO (con Central Package Management)

### Directory.Packages.props (NUOVO!)
```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>

  <ItemGroup>
    <!-- .NET MAUI -->
    <PackageVersion Include="Microsoft.Maui.Controls" Version="9.0.110" />
    <PackageVersion Include="Microsoft.Extensions.Logging.Debug" Version="9.0.9" />
    
    <!-- Community -->
    <PackageVersion Include="CommunityToolkit.Maui" Version="12.2.0" />
    
    <!-- Database -->
    <PackageVersion Include="Microsoft.Data.Sqlite.Core" Version="9.0.9" />
    <PackageVersion Include="SQLitePCLRaw.bundle_green" Version="2.1.11" />
    
    <!-- Graphics -->
    <PackageVersion Include="SkiaSharp.Views.Maui.Controls" Version="3.116.1" />
    <PackageVersion Include="System.Drawing.Common" Version="9.0.0" />
    
    <!-- Mathematics -->
    <PackageVersion Include="MathNet.Numerics" Version="5.0.0" />
    
    <!-- Testing -->
    <PackageVersion Include="NUnit" Version="4.4.0" />
    <PackageVersion Include="NUnit3TestAdapter" Version="5.1.1" />
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
  </ItemGroup>
</Project>
```

### GlucoMan.Maui.csproj (modificato)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net9.0-windows;net9.0-android</TargetFrameworks>
    <!-- ... -->
  </PropertyGroup>

  <ItemGroup>
    <!-- Niente più Version! Le versioni sono in Directory.Packages.props -->
    <PackageReference Include="CommunityToolkit.Maui" />
    <PackageReference Include="MathNet.Numerics" />
    <PackageReference Include="Microsoft.Data.Sqlite.Core" />
    <PackageReference Include="Microsoft.Extensions.Logging.Debug" />
    <PackageReference Include="Microsoft.Maui.Controls" />
    <PackageReference Include="NUnit" />
    <PackageReference Include="SkiaSharp.Views.Maui.Controls" />
    <PackageReference Include="SQLitePCLRaw.bundle_green" />
  </ItemGroup>

  <!-- Windows-specific (ancora condizionale, ma senza Version) -->
  <ItemGroup Condition="$(TargetFramework.Contains('windows'))">
    <PackageReference Include="System.Drawing.Common" />
  </ItemGroup>
</Project>
```

### TestGlucoMan.csproj (modificato)
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <!-- Niente più Version! Tutti i progetti usano la stessa versione -->
    <PackageReference Include="Microsoft.Maui.Controls" />
    <PackageReference Include="NUnit" />
    <PackageReference Include="NUnit3TestAdapter" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" />
  </ItemGroup>
</Project>
```

? **Vantaggi**:
- `Microsoft.Maui.Controls` usa **sempre** versione 9.0.110 (consistenza!)
- Per aggiornare MAUI: **1 solo file** da modificare (Directory.Packages.props)
- File `.csproj` più puliti e leggibili
- Zero conflitti di versioni

---

## ?? Confronto: Aggiornare Microsoft.Maui.Controls da 9.0.110 a 9.0.120

### ?? PRIMA (senza CPM)
1. Apri `GlucoMan.Maui/GlucoMan.Maui.csproj`
2. Trova `<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.110" />`
3. Cambia in `Version="9.0.120"`
4. Apri `TestGlucoMan/TestGlucoMan.csproj`
5. Trova `<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.100" />`
6. Cambia in `Version="9.0.120"`
7. Apri altri progetti che usano MAUI...
8. Ripeti per ognuno...
9. `dotnet restore`
10. Spera di non aver dimenticato nessun progetto ??

**Tempo stimato**: 5-10 minuti  
**Rischio errore**: ALTO

### ?? DOPO (con CPM)
1. Apri `Directory.Packages.props`
2. Cambia `<PackageVersion Include="Microsoft.Maui.Controls" Version="9.0.110" />` in `Version="9.0.120"`
3. `dotnet restore`
4. ? Fatto! TUTTI i progetti ora usano 9.0.120

**Tempo stimato**: 30 secondi  
**Rischio errore**: ZERO

---

## ?? Esempio Reale: Cosa Succede Durante Build

### PRIMA
```
GlucoMan.Maui.csproj: Microsoft.Maui.Controls 9.0.110 ?
TestGlucoMan.csproj:   Microsoft.Maui.Controls 9.0.100 ??

WARNING NU1608: Detected package downgrade: Microsoft.Maui.Controls from 9.0.110 to 9.0.100
```

### DOPO
```
Directory.Packages.props: Microsoft.Maui.Controls 9.0.110
  ?? GlucoMan.Maui.csproj ?
  ?? TestGlucoMan.csproj ?

Tutte le versioni consistenti!
```

---

## ?? Statistiche per GlucoProgs

### File .csproj Prima
- **Righe totali**: ~500 righe tra tutti i progetti
- **PackageReference con Version**: ~40
- **Pacchetti duplicati**: 8-10 (stessa lib in più progetti)

### File .csproj Dopo
- **Righe totali**: ~450 righe (50 in meno!)
- **PackageReference con Version**: 0 (tutti in Directory.Packages.props)
- **Pacchetti duplicati**: 0 (centralizzati)

### Directory.Packages.props
- **Righe totali**: ~50
- **Pacchetti unici gestiti**: 12-15
- **Fonte di verità unica**: 1 file

**Risparmio netto**: ~50 righe eliminate + gestione semplificata

---

## ? Checklist di Conversione

- [ ] Creare `Directory.Packages.props` nella root
- [ ] Elencare tutti i pacchetti con versioni
- [ ] Rimuovere `Version="..."` da `GlucoMan.Maui.csproj`
- [ ] Rimuovere `Version="..."` da `TestGlucoMan.csproj`
- [ ] Rimuovere `Version="..."` da altri progetti
- [ ] Eseguire `dotnet restore`
- [ ] Eseguire `dotnet build`
- [ ] Verificare che non ci siano errori
- [ ] Committare le modifiche in Git

**Tempo totale stimato**: 15-20 minuti per tutta la solution

---

## ?? Risultato Finale

**Prima**: Gestione manuale, rischio di inconsistenze, aggiornamenti laboriosi

**Dopo**: Gestione centralizzata, zero inconsistenze, aggiornamenti in 30 secondi

**ROI**: Ogni aggiornamento futuro di pacchetti ti farà risparmiare 5-10 minuti! ??
