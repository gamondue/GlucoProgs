# Identification module

This module implements identification routines for the MISO first-order model represented in `Documents/Identification/Diagram MISO first order sum of dynamics.md`.

## Files
- `Identification.cs` : static helper with methods to resample time series, fit ARX-first-order model with integer delays, greedy search for delays, compute SSE.
- `Identification2.cs` : higher-level routines that find isolated segments (single-input dynamics) and perform SISO identification per segment; aggregates results per input (CHO / Insulin) and provides convenience methods to run the two-channel identification pipeline.

## Identification2 (brief)
`Identification2` implements an identification workflow tailored to CGM and event data:

- Segment selection: for each meal or injection event the code verifies left-side conditions (basal glucose flatness for a configurable time window and no opposite events in a configurable pre-event interval) and right-side isolation (no opposite events in a configurable post-event interval).
- SISO estimation: on accepted segments a first-order ARX model `y[k]=a*y[k-1]+b*u[k-d]+c` is fitted using ridge regression while performing a grid search on integer delays.
- Aggregation: individual SISO results are aggregated into statistics (mean, std, median) for time constant (?), static gain (K), delay and offset; quality metrics (RMSE, R²) are also reported.
- Robustness: pre-processing includes light smoothing and robust slope checks (Theil–Sen + OLS) for the basal stability test; missing samples (NaN) are handled and rows with NaNs are skipped in regressions.

## Methods and mathematics
See the detailed explanation in `SharedMathematics/Mathematical methods used in Identification2.md`.
