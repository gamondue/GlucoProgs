# Identificazione modello “MISO first order (MD)”

Questa pagina riassume una procedura pratica e ripetibile per identificare un modello MISO (multi-input single-output) del primo ordine con ritardi. Contiene anche uno scheletro C# riutilizzabile per integrare l’identificazione nel progetto.

---

## 1) Definizione del modello (discreto)
Modello ARX-first-order con ritardi interi:
y[k] = a * y[k-1] + sum_i b_i * u_i[k - d_i] + c + e[k]

- a = exp(-Ts / T) → da `a` si ricava la costante di tempo `T = -Ts / ln(a)`.
- Ritardi Td_i quantizzati in campioni: `d_i = round(Td_i / Ts)`.
- b_i correlano ai guadagni K_i (per step unitario: b_i ≈ K_i * (1 - a)).

---

## 2) Strategia di identificazione
- Preprocessing:
  - allineare/interpolare i segnali su intervallo di campionamento uniforme `Ts`;
  - rimuovere offset/trend (detrend);
  - applicare filtro passa-basso leggero se necessario.
- Stima ritardi:
  - ricerca a griglia su intervallo plausibile (0..Nmax campioni);
  - per sistemi con molti ingressi usare ricerca greedy (stima ritardi uno per volta).
- Per una combinazione fissa di `d_i` costruire la matrice regressore Φ e risolvere LS (minimi quadrati) per [a, b_1..b_m, c].
- Per rifinitura eseguire ottimizzazione non-lineare (Levenberg–Marquardt) su parametri reali e ritardi continui (interpolare ingressi).
- Validazione: analisi dei residui, R², AIC/BIC, cross‑validation, bootstrap per intervalli di confidenza.

---

## 3) Suggerimenti pratici
- Inizializzazione:
  - K_i iniziali da risposta a step (Δy/Δu);
  - T iniziale dal tempo a 63% della risposta.
- Ritardo iniziale: tempo di prima risposta significativa.
- Regolarizzazione: usare ridge (λ) per regressori correlati.
- Stima online: RLS con forgetting factor per adattamento.
- Campionamento: Ts ≪ T (es. Ts ≤ T/10).

---

## 4) Output e validazione nella UI
Mostrare per ciascun ingresso:
- K_i, T, Td_i, SSE, R², covarianze (o intervalli bootstrap).
Visualizzare grafici dati vs simulazione e residui.

---

## 5) Librerie consigliate
- Aggiungere tramite __Manage NuGet Packages__: `MathNet.Numerics` (algebra lineare, QR, solutori) e opzionalmente pacchetti di ottimizzazione.

---

First-order sum of inputs — Identification notes

Model assumption:
- The glucose response is modelled as the sum of two independent first-order dynamics, one for CHO and one for insulin:

  y(t) = G_CHO(s) * u_CHO(t) + G_Insulin(s) * u_Insulin(t)

  where each transfer function is G(s) = k / (? s + 1).

Identification approach:
- Each isolated segment assumes only one input is active; therefore the identification of each dynamic can be performed as SISO.
- For segments containing multiple same-type events their combined input is used (sum of impulse magnitudes) and the segment is identified as a single event.

Limitations:
- Linear time-invariant first-order assumption may not capture complex physiology or meal absorption variability.
- Requires careful selection of isolation windows and basal stability thresholds to avoid contaminated segments.
