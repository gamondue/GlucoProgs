Implementation Summary - Identification

This document summarizes the implementation choices in `SharedMathematics/Identification2.cs`.

- Segment selection:
  - Left-side checks: basal stability using OLS and Theil-Sen slopes on `basalCheckHours` window; no opposite events in `beforeIsolationHours`.
  - Right-side checks: no opposite events for `isolationHours` after the event.
  - Minimum glucose samples computed from `minDataHours` and `TsSeconds`.

- SISO estimation:
  - Each segment is resampled to uniform grid; input impulse placed at event sample.
  - Grid search over integer delays; for each delay solve ridge linear least squares for (a,b,c).
  - Choose delay minimizing SSE.

- Aggregation:
  - Individual SISO results aggregated with mean/std/median per channel.

- Robustness:
  - 3-point moving average smoothing and robust Theil–Sen slope for basal check.

See `SharedMathematics/Mathematical methods used in Identification2.md` for mathematical background and examples.
