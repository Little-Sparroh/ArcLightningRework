# Arc Lightning Rework

Enhances Scout Laser Rifle Arc Lightning with chaining, turbocharge support, and updated upgrade text.

## Features

- **Lightning Arc Rework**: On laser hits, rolls arc chance and spawns a lightning fork at most once per fire burst.
- **Normal Arc**: Chains with range 10 and 1 max hit.
- **Turbocharged Arc**: When a Lightning Arc upgrade is turbocharged, chains with range 10, 10 max hits, and 15 damage.
- **Turbocharge Support**: Marks Lightning Arc upgrades as turbochargeable and updates descriptions, turbocharged info,
  and stat text accordingly.

## Getting Started

### Dependencies

- [Mycopunk](https://store.steampowered.com/app/2523040/Mycopunk/) (base game)
- [BepInExPack_Mycopunk](https://thunderstore.io/c/mycopunk/p/BepInEx/BepInExPack_Mycopunk/) 5.4.2403 or compatible

### Building

Requires .NET SDK with `netstandard2.1` support.

```bash
dotnet build --configuration Release
```

Output DLL: `bin/Release/netstandard2.1/ArcLightningRework.dll`

### Installing

**Via Thunderstore (recommended)**

1. Install with a Thunderstore-compatible mod manager.

**Manual installation**

1. Install BepInEx for Mycopunk.
2. Copy `ArcLightningRework.dll` to `<Mycopunk Directory>/BepInEx/plugins/`.

## Configuration

Config file: `<Mycopunk Directory>/BepInEx/config/sparroh.arclightningrework.cfg`

| Setting       | Section | Default | Description                                                                                         |
|---------------|---------|---------|-----------------------------------------------------------------------------------------------------|
| Enable Rework | General | `true`  | Enhances lightning arc behavior for Scout Laser Rifle, allowing chaining and turbocharged upgrades. |

## Authors

- Sparroh

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
