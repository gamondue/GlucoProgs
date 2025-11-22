# Methods used for MISO first-order identification

This document describes the mathematical methods used in `Identification.cs` and suggested improvements.

## Model
We consider a MISO (multiple input, single output) model where the output y(t) is the sum of the responses of multiple first-order systems to their respective inputs plus a bias term and noise:

In discrete-time ARX form (uniform sampling Ts):

    y[k] = a * y[k-1] + \sum_{i=1}^m b_i * u_i[k - d_i] + c + e[k]

- `a` is the autoregressive coefficient: a = exp(-Ts / T) where T is the continuous-time time constant.
- `b_i` scales the input `u_i` contribution; for a unit step, steady-state gain `K_i` relates to `b_i` and `a`.
- `d_i` are integer delays in samples (Td_i = d_i * Ts).
- `c` is an offset/bias.
- `e[k]` is residual (noise).

## Overview of approach

1. Align signals on a uniform timebase (common start and end), resampling inputs and outputs by linear interpolation.
2. Handle missing output samples by skipping rows in the regression where required regressors or outputs are NaN.
3. For candidate integer delays `d_i` estimate coefficients using regularized least squares (Ridge) to mitigate collinearity.
4. Use a greedy search over delays: iterate inputs and search best delay for each while holding others fixed, repeat until convergence.
5. Evaluate fit using SSE; refine parameters with continuous optimization if needed.

## Details

### Resampling
- Inputs and outputs are resampled to a grid `t_k = t0 + k * Ts`.
- Linear interpolation is used between known timestamps. Outside the known range, `NaN` is used.

### Linear regression
Given delays `d_i`, build matrix M with rows:

    [ y[k-1], u1[k-d1], u2[k-d2], ..., 1 ]

and rhs y[k]. Solve (M^T M + ? I) x = M^T y where x = [a, b1, b2, ..., c].

### Greedy delay search
- For each input i, test delays in [0..maxDelaySamples], compute LS fit and SSE, pick best.
- Repeat for each input until no improvement or iteration limit.

## Limitations and improvements

### Limitations
- Integer delays only (can be refined to fractional delays with interpolation).
- Greedy search may miss global optimum when delays are strongly coupled.
- Assumes time-invariant linear dynamics.

### Improvements
- Use non-linear optimization (Levenberg-Marquardt) on (a, b_i, d_i continuous) initialized with LS solution.
- Use instrumental variables or errors-in-variables for noisy inputs.
- Estimate parameter confidence via bootstrap or analytical covariance from residual variance and (M^T M)^{-1}.
- Use regularization path (cross-validated ?).
- Support missing data with EM-like approaches or state-space models + Kalman filtering.
- Allow distributed delays / higher order dynamics.

## Using MathNet.Numerics
- MathNet is used for matrix operations and solving normal equations.
- For optimization consider MathNet's optimization package or external libraries.

