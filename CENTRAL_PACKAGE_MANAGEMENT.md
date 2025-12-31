# Central Package Management - Guida per GlucoProgs

## ?? Cos'è Directory.Packages.props

È un file che gestisce **centralmente** le versioni di tutti i pacchetti NuGet della solution.

## ? Vantaggi

1. **Versione unica**: Tutti i progetti usano la stessa versione di ogni pacchetto
2. **Aggiornamenti facili**: Cambi la versione in UN solo posto
3. **Niente conflitti**: Garantita consistenza tra progetti
4. **Più pulito**: I file `.csproj` non hanno più `Version="..."`

## ?? Come Iniziare ad Usarlo

### Passo 1: Il file è già creato
? `Directory.Packages.props` è già nella root della solution

### Passo 2: Aggiorna i file .csproj

Nei tuoi file di progetto (`.csproj`), **rimuovi l'attributo `Version`** dai `PackageReference`.

**PRIMA** (esempio da `GlucoMan.Maui.csproj`):
```xml
<ItemGroup>
  <PackageReference Include="CommunityToolkit.Maui" Version="12.2.0" />
  <PackageReference Include="Microsoft.Maui.Controls" Version="9.0.110" />
  <PackageReference Include="Microsoft.Data.Sqlite.Core" Version="9.0.9" />
</ItemGroup>
```

**DOPO** (versioni gestite centralmente):
```xml
<ItemGroup>
  <PackageReference Include="CommunityToolkit.Maui" />
  <PackageReference Include="Microsoft.Maui.Controls" />
  <PackageReference Include="Microsoft.Data.Sqlite.Core" />
</ItemGroup>
```

### Passo 3: Rebuild della solution

```bash
dotnet restore
dotnet build
```

## ?? Workflow Quotidiano

### Aggiungere un NUOVO pacchetto

1. **In `Directory.Packages.props`** aggiungi:
   ```xml
   <PackageVersion Include="Newtonsoft.Json" Version="13.0.3" />
   ```

2. **Nel tuo `.csproj`** aggiungi (senza `Version`):
   ```xml
   <PackageReference Include="Newtonsoft.Json" />
   ```

3. Rebuild:
   ```bash
   dotnet restore
   ```

### Aggiornare un pacchetto ESISTENTE

1. **In `Directory.Packages.props`** cambia solo la versione:
   ```xml
   <!-- Prima -->
   <PackageVersion Include="Microsoft.Maui.Controls" Version="9.0.110" />
   
   <!-- Dopo -->
   <PackageVersion Include="Microsoft.Maui.Controls" Version="9.0.120" />
   ```

2. Rebuild:
   ```bash
   dotnet restore
   dotnet build
   ```

3. ? **TUTTI** i progetti useranno automaticamente la versione 9.0.120!

## ?? Casi Speciali

### Pacchetto condizionale (solo Windows)

Nel `.csproj`:
```xml
<ItemGroup Condition="$(TargetFramework.Contains('windows'))">
  <PackageReference Include="System.Drawing.Common" />
</ItemGroup>
```

La versione è sempre in `Directory.Packages.props`:
```xml
<PackageVersion Include="System.Drawing.Common" Version="9.0.0" />
```

### Versione DIVERSA per UN progetto specifico

Se un progetto ha DAVVERO bisogno di una versione diversa:

```xml
<!-- Nel .csproj di quel progetto specifico -->
<PackageReference Include="Microsoft.Maui.Controls" VersionOverride="9.0.100" />
```

?? **Usa con cautela**: rompe la consistenza!

## ?? Verifica Versioni in Uso

### Vedere tutti i pacchetti di un progetto
```bash
dotnet list GlucoMan.Maui/GlucoMan.Maui.csproj package
```

### Vedere pacchetti outdated
```bash
dotnet list package --outdated
```

### Vedere tutte le versioni centralizzate
Basta aprire `Directory.Packages.props`!

## ??? Migrazione dei Progetti Esistenti

### Per GlucoMan.Maui
1. Apri `GlucoMan.Maui/GlucoMan.Maui.csproj`
2. Trova tutti i `<PackageReference ... Version="..." />`
3. Rimuovi l'attributo `Version="..."`
4. Salva e rebuild

### Per SharedGlucoMan (se ha pacchetti)
Stesso processo dei punti sopra.

### Per TestGlucoMan
Stesso processo dei punti sopra.

## ?? Esempio Completo

### Prima della migrazione

**GlucoMan.Maui.csproj:**
```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.110" />
```

**TestGlucoMan.csproj:**
```xml
<PackageReference Include="Microsoft.Maui.Controls" Version="9.0.100" /> <!-- VERSIONE DIVERSA! -->
<PackageReference Include="NUnit" Version="4.4.0" />
```

? **Problema**: Due versioni diverse dello stesso pacchetto!

### Dopo la migrazione

**Directory.Packages.props:**
```xml
<PackageVersion Include="Microsoft.Maui.Controls" Version="9.0.110" />
<PackageVersion Include="NUnit" Version="4.4.0" />
```

**GlucoMan.Maui.csproj:**
```xml
<PackageReference Include="Microsoft.Maui.Controls" />
```

**TestGlucoMan.csproj:**
```xml
<PackageReference Include="Microsoft.Maui.Controls" />
<PackageReference Include="NUnit" />
```

? **Risolto**: Entrambi usano versione 9.0.110!

## ?? Best Practices

1. ? **Aggiorna sempre** `Directory.Packages.props` quando aggiungi/aggiorni pacchetti
2. ? **Non usare** `Version` nei `.csproj` (lascia gestire al file centrale)
3. ? **Testa** dopo ogni aggiornamento di pacchetti
4. ?? **Evita** `VersionOverride` a meno che non sia VERAMENTE necessario
5. ?? **Documenta** nel `Directory.Packages.props` se usi versioni speciali

## ?? Troubleshooting

### "Package X not found"
1. Verifica che sia in `Directory.Packages.props`
2. Esegui `dotnet restore`

### "Version conflict"
1. Controlla se hai lasciato `Version="..."` in qualche `.csproj`
2. Rimuovi tutti gli attributi `Version` dai `PackageReference`

### "Downgrade detected"
Un progetto ha `VersionOverride` più vecchio del centrale. Rimuovilo o aggiornalo.

## ?? Riferimenti

- [Microsoft Docs - Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [NuGet Blog - Introducing CPM](https://devblogs.microsoft.com/nuget/introducing-central-package-management/)

## ?? Vantaggi per GlucoProgs

Con 4+ progetti (GlucoMan.Maui, SharedGlucoMan, SharedGeneral, TestGlucoMan):

- ? Aggiornare .NET MAUI da 9.0.110 a 9.0.120: **1 riga** invece di 4+
- ? Niente più "perché Test usa versione diversa da Main?"
- ? Più facile vedere TUTTE le dipendenze in un colpo d'occhio
- ? Meno merge conflicts sui file `.csproj`

---

**Nota**: Il file è **opzionale** ma **raccomandato** da Microsoft per soluzioni multi-progetto come GlucoProgs.
