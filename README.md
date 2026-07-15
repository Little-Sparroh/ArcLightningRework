# Arc Lightning Rework

A BepInEx mod for Mycopunk that reworks Scout Laser Rifle Arc Lightning behavior and turbocharge support.

## Features

- **Lightning Arc Rework**: Spawns lightning forks on laser hits based on arc chance, once per fire burst.
- **Turbocharged Arc**: Turbocharged Lightning Arc upgrades chain to more enemies with higher damage (10 max hits, 15 damage).
- **Turbocharge Support**: Marks Lightning Arc upgrades as turbochargeable and updates descriptions/stat text accordingly.

## Getting Started

### Dependencies

* Mycopunk (base game)
* [BepInEx](https://github.com/BepInEx/BepInEx) - Version 5.4.2403 or compatible
* .NET Framework 4.8
* [HarmonyLib](https://github.com/pardeike/Harmony) (included via NuGet)

### Building/Compiling

```bash
dotnet build --configuration Release
```

### Installing

**Via Thunderstore (Recommended)**:
1. Download and install via Thunderstore Mod Manager

**Manual Installation**:
1. Place the built `ArcLightningRework.dll` in your `<Mycopunk Directory>/BepInEx/plugins/` folder

## Configuration

Access mod settings at `<Mycopunk Directory>/BepInEx/config/sparroh.arclightningrework.cfg`:

| Setting | Default | Description |
|---------|---------|-------------|
| Enable Arc Lightning Rework | `true` | Enables lightning arc chaining and turbocharged upgrade support. |

## Authors

- Sparroh

## License

This project is licensed under the MIT License - see the LICENSE file for details
