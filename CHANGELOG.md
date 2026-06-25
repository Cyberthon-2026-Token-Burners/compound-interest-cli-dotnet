# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Implemented high-precision compound interest simulation calculations in `Simulator.cs` under `src/CompoundWealthSimulator`.
- Introduced strong-typed contracts: `Frequency` enum, `SimulationParameters` record, and `ProjectionRow` struct.
- Established strict input parameter validations (Principal, Contribution, Frequency, AnnualInterestRate, DurationYears) with sequential ArgumentException precedence checks.
- Created `ISimulator` interface guaranteeing high-precision iterative compounding computations (without float-based precision drift).

### Changed
- Scaffolding of the core solution structure in `CompoundWealthSimulator.sln`.
- Created executable CLI application project skeleton targeting `net10.0`.
- Integrated `CommandLineParser` package dependencies (v2.9.1) within production target.
- Scaffolded testing target project using `xunit` (v2.9.2) alongside .NET test utility assemblies.
- Implemented standard baseline configuration environment within `.gitignore`.
- Established foundational Program Entry Point with standard initialized output.