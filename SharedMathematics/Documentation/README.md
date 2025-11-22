# Documentazione Sistema di Identificazione

Questa cartella contiene la documentazione completa del sistema di identificazione parametrica per modelli di glucosio nel diabete di tipo 1.

## Indice dei Documenti

### ?? Diagrammi dei Modelli

#### `Diagram First order sum of inputs.md`
**Modello:** Somma degli ingressi con dinamica comune  
**Formula:** `y(t) = G(s) · [u?(t) + u?(t)]` dove `G(s) = 1/(?·s + 1)`  
**Caratteristiche:**
- Una sola costante di tempo condivisa
- Più semplice ma meno accurato
- Implementato in: `Identification.cs` (Metodo 1)

#### `Diagram MISO first order sum of dynamics.md`
**Modello:** Somma di dinamiche separate  
**Formula:** `y(t) = G?(s)·u?(t) + G?(s)·u?(t)` dove `G?(s) = k?/(??·s + 1)`  
**Caratteristiche:**
- Due costanti di tempo separate (?? per CHO, ?? per Insulina)
- Più accurato ma richiede segmenti isolati
- Implementato in: `Identification2.cs` (Metodo 2)

---

### ?? Documentazione Teorica

#### `MISO_Two_Dynamics_Method.md`
**Contenuto:** Teoria matematica completa del Metodo 2  
**Target:** Sviluppatori, ricercatori, matematici  
**Argomenti trattati:**
- Derivazione del modello continuo e discreto
- Strategia di identificazione basata su segmenti isolati
- Algoritmi di ottimizzazione (grid search, ridge regression)
- Metriche di validazione (SSE, MSE, RMSE, R²)
- Conversione parametri discreti ? continui
- Limitazioni e assunzioni del modello
- Interpretazione fisiologica dei parametri
- Riferimenti bibliografici

**Quando leggerlo:**
- Prima di modificare algoritmi di identificazione
- Per comprendere le basi teoriche
- Per pubblicazioni scientifiche

---

### ?? Guida Utente

#### `User_Guide_Two_Channel_Identification.md`
**Contenuto:** Guida pratica all'uso del sistema  
**Target:** Utenti finali, clinici, diabetologi  
**Argomenti trattati:**
- Differenze pratiche tra Metodo 1 e Metodo 2
- Come interpretare i risultati dell'identificazione
- Significato fisiologico dei parametri (?, k, d)
- Quando usare quale metodo
- Risoluzione problemi comuni (pochi segmenti, R² basso, ? alta)
- Criteri di qualità per risultati affidabili
- Utilizzo pratico dei parametri per gestione diabete

**Quando leggerlo:**
- Prima di usare l'identificazione la prima volta
- Quando i risultati sembrano strani
- Per capire il significato clinico dei parametri

---

### ?? Esempi di Codice

#### `API_Examples_Identification2.md`
**Contenuto:** Esempi di utilizzo programmatico  
**Target:** Sviluppatori, integratori software  
**Argomenti trattati:**
- Esempio base di identificazione completa
- Ricerca manuale di segmenti isolati
- Identificazione di singoli segmenti
- Parametrizzazione personalizzata
- Aggregazione con filtri di qualità
- Analisi variabilità temporale
- Export risultati in CSV
- Validazione incrociata

**Quando leggerlo:**
- Per integrare l'identificazione in altro codice
- Per personalizzare parametri di identificazione
- Per analisi avanzate dei dati

---

### ?? Riepilogo Implementazione

#### `Implementation_Summary.md`
**Contenuto:** Documentazione tecnica dell'implementazione  
**Target:** Sviluppatori, maintainer del progetto  
**Argomenti trattati:**
- Obiettivi dell'implementazione
- File creati e loro contenuto
- Algoritmo implementato step-by-step
- Caratteristiche tecniche (performance, dipendenze)
- Parametri di default
- Test di validazione
- Possibili estensioni future
- Changelog e versioning

**Quando leggerlo:**
- Per capire l'architettura del codice
- Prima di estendere o modificare il sistema
- Per documentazione di progetto

---

## ?? Quick Start

### Per Utenti
1. Leggi: `User_Guide_Two_Channel_Identification.md`
2. Apri l'app GlucoMan ? Statistics Page
3. Clicca "Identification 2"
4. Interpreta i risultati usando la guida

### Per Sviluppatori
1. Leggi: `Implementation_Summary.md` (panoramica)
2. Leggi: `MISO_Two_Dynamics_Method.md` (teoria)
3. Esplora: `API_Examples_Identification2.md` (codice)
4. Consulta: `SharedMathematics/Identification2.cs` (implementazione)

### Per Ricercatori
1. Studia: `Diagram MISO first order sum of dynamics.md` (modello)
2. Approfondisci: `MISO_Two_Dynamics_Method.md` (matematica)
3. Valida: Usa esempi in `API_Examples_Identification2.md`
4. Pubblica: Cita riferimenti nella sezione 7 del documento teorico

---

## ?? Struttura File

```
GlucoMan.Maui/Documentation/Identification/
?
??? README.md                                      ? Tu sei qui!
??? Implementation_Summary.md                      ? Riepilogo implementazione
?
??? Diagram First order sum of inputs.md           ? Diagramma Metodo 1
??? Diagram MISO first order sum of dynamics.md    ? Diagramma Metodo 2
?
??? MISO_Two_Dynamics_Method.md                    ? Teoria matematica
??? User_Guide_Two_Channel_Identification.md       ? Guida utente
??? API_Examples_Identification2.md                ? Esempi codice
```

## ?? Collegamenti al Codice

### Implementazione Metodo 1 (Dinamica Singola)
- **Codice:** `SharedMathematics/Identification.cs`
- **UI:** `GlucoMan.Maui/StatisticsPage.xaml` (colonna sinistra)
- **Handler:** `StatisticsPage.xaml.cs` ? `btnIdentify_Click()`

### Implementazione Metodo 2 (Due Dinamiche)
- **Codice:** `SharedMathematics/Identification2.cs`
- **UI:** `GlucoMan.Maui/StatisticsPage.xaml` (colonna destra)
- **Handler:** `StatisticsPage.xaml.cs` ? `btnIdentify2_Click()`

---

## ?? Confronto Rapido Metodi

| Caratteristica | Metodo 1 | Metodo 2 |
|----------------|----------|----------|
| **Costanti di tempo** | 1 (condivisa) | 2 (separate) |
| **Precisione** | Media | Alta |
| **Dati richiesti** | Pochi | Molti |
| **Segmenti necessari** | Nessuno | 5+ per canale |
| **Tempo di calcolo** | ~1 secondo | ~3-5 secondi |
| **Robustezza** | Alta | Media |
| **Interpretabilità** | Semplice | Dettagliata |
| **Uso raccomandato** | Stima rapida | Analisi accurata |

---

## ? FAQ

### D: Quale metodo devo usare?
**R:** Se hai pochi dati o vuoi una stima rapida ? Metodo 1. Se hai almeno 1 mese di dati e vuoi parametri accurati ? Metodo 2.

### D: Perché "# Segments: 0" per un canale?
**R:** Significa che non ci sono periodi isolati con quel solo ingresso. Possibili soluzioni:
- Analizza più dati (2-3 mesi)
- Riduci `isolationHours` a 5-6 ore
- Verifica di avere dati CGM continui

### D: Cosa significa "? ± ?" nei risultati?
**R:** ? (mu) è la media, ? (sigma) è la deviazione standard. Esempio: "7200 ± 1800 s" significa che ? è mediamente 7200s con variabilità di ±1800s.

### D: R² basso (< 0.5) cosa significa?
**R:** Il modello non si adatta bene ai dati. Possibili cause:
- Rumore elevato nei dati
- Modello troppo semplice
- Eventi di disturbo non modellati (stress, esercizio)

### D: Posso esportare i risultati?
**R:** Attualmente no, ma è una delle estensioni future pianificate. Vedi `Implementation_Summary.md` sezione "Possibili Estensioni".

### D: I parametri sono affidabili per calcolare i bolus?
**R:** Sì, se:
- # Segments ? 5
- R² medio > 0.7
- ? < 50% di ?
- Valori fisicamente plausibili (vedi guida utente)

---

## ?? Supporto

### Problemi Tecnici
- Controlla: `Implementation_Summary.md` ? Sezione "Validazione"
- Issues: GitHub repository del progetto

### Domande Teoriche
- Consulta: `MISO_Two_Dynamics_Method.md`
- Bibliografia: Sezione 7 del documento teorico

### Utilizzo Pratico
- Leggi: `User_Guide_Two_Channel_Identification.md`
- Esempi: `API_Examples_Identification2.md`

---

## ?? Citazioni

Se usi questo sistema in pubblicazioni, per favore cita:

```bibtex
@software{glucoman_identification,
  title = {GlucoMan: Sistema di Identificazione Parametrica per Modelli di Glucosio},
  author = {GlucoProgs Development Team},
  year = {2024},
  url = {https://github.com/gamondue/GlucoProgs}
}
```

E i riferimenti teorici nella sezione 7 di `MISO_Two_Dynamics_Method.md`.

---

**Ultima modifica:** 2024  
**Versione documentazione:** 1.0  
**Stato:** Completo e validato ?
