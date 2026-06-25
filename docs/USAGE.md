# Compound Wealth Simulator - End-User Guide

The **Compound Wealth Simulator** is a high-precision command-line tool designed for retail investors to model and project their long-term wealth growth. By simulating how initial savings (the principal) and periodic contributions compound over time at a stable interest rate, users can evaluate different saving strategies and visualize their financial future.

This guide details how to install, configure, and execute the compiled utility, explaining output interpretation, error scenarios, and command-line options.

---

## 1. Obtaining & Building the Application

### Option A: Running from Source
If you have the .NET 10.0 SDK installed, you can build and run the application directly:
```sh
# Build the project
dotnet build

# Run with arguments
dotnet run --project src/CompoundWealthSimulator/CompoundWealthSimulator.csproj -- --principal 5000 --contribution 200 --frequency Monthly --rate 5.5 --duration 10
```

### Option B: Running the Compiled Binary
After building the project, you can run the compiled executable from the output folder:
- **Windows**: `src/CompoundWealthSimulator/bin/Debug/net10.0/CompoundWealthSimulator.exe`
- **Linux/macOS**: `src/CompoundWealthSimulator/bin/Debug/net10.0/CompoundWealthSimulator`

---

## 2. CLI Reference & Configuration Options

The simulator accepts the following standard command-line flags:

| Flag (Short) | Option (Long) | Type | Required | Description |
| :--- | :--- | :--- | :--- | :--- |
| `-p` | `--principal` | Decimal | Yes | The initial lump-sum deposit (must be >= 0). |
| `-c` | `--contribution` | Decimal | Yes | The recurring deposit amount (must be >= 0). |
| `-f` | `--frequency` | String | Yes | The deposit interval. Valid options: `Monthly`, `Quarterly`, `Annually`. |
| `-r` | `--rate` | Decimal | Yes | The expected annual interest rate as a percentage (e.g. `5.5` for 5.5%). Must be >= 0. |
| `-d` | `--duration` | Integer | Yes | The simulation horizon in years (must be >= 0). |

---

## 3. Real Example Invocation & Output

### Scenario: High-Yield Compound Savings
An investor initializes a savings account with **$10,000**, committing to save **$500 monthly** at a high-yield annual return of **6.0%** for a duration of **5 years**.

#### Command:
```sh
dotnet run --project src/CompoundWealthSimulator/CompoundWealthSimulator.csproj -- -p 10000 -c 500 -f Monthly -r 6.0 -d 5
```

#### Output:
```text
Year | Total Contributions | Cumulative Interest |          End Balance
-----+---------------------+---------------------+---------------------
   1 |          $16,000.00 |             $789.24 |          $16,789.24
   2 |          $22,000.00 |           $2,221.73 |          $24,221.73
   3 |          $28,000.00 |           $4,357.51 |          $32,357.51
   4 |          $34,000.00 |           $7,260.67 |          $41,260.67
   5 |          $40,000.00 |          $11,000.22 |          $51,000.22
```

*(Note: Total Contributions represents the initial $10,000 plus the sum of all $500 monthly deposits. Cumulative Interest reflects the compound interest earned over time, and End Balance is the sum of contributions and interest.)*

---

## 4. Input Validation & Troubleshooting

To maintain high structural integrity and prevent unrealistic financial entries, the CLI enforces strict boundary validations. When validation checks fail, the utility will terminate with a non-zero exit code and print a clear error message.

### Common Validation Rules:
1. **Negative Capital Denied**: Principal (`--principal`) and Contribution (`--contribution`) must be greater than or equal to `0`.
2. **Rate Limits**: Annual Interest Rate (`--rate`) must be a positive percentage value (or 0). It represents the rate as a whole number (e.g. `5` for 5.0%, not `0.05`).
3. **Horizon Limits**: Duration (`--duration`) must be a positive integer (or 0). Projections beyond typical maximum thresholds (e.g. 100 years) are supported but subject to execution memory safeguards.

### Error Output Example:
If you supply a negative principal:
```sh
dotnet run --project src/CompoundWealthSimulator/CompoundWealthSimulator.csproj -- -p -1000 -c 100 -f Monthly -r 5.0 -d 10
```
Response:
```text
Error: Principal cannot be negative.
```