# RE2_DisableSharpeningAndTAA

## Description
Disable sharpening and TAA in Resident Evil 2 (2019) / Resident Evil 2 Remake.

## Dependencies
- REFrameworkNETPluginConfig https://github.com/TonWonton/REFrameworkNETPluginConfig

## Installation
### Lua
1. Install REFramework
  - NexusMods: https://www.nexusmods.com/residentevil22019/mods/1097
  - GitHub: https://github.com/praydog/REFramework-nightly/releases
2. Download the lua script and extract to game folder
  - `RE2_DisableSharpeningAndTAA.lua` should be in `\GAME_FOLDER\reframework\autorun\RE2_DisableSharpeningAndTAA.lua`

### C#
1. Install prerequisites
  - REFramework + REFramework csharp-api (download and extract both `RE2.zip` AND `csharp-api.zip` to the game folder): https://github.com/praydog/REFramework-nightly/releases
    - Only extract `dinput8.dll` from the `RE2.zip`
  - .NET 10.0 Desktop Runtime x64: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
2. Download the plugin and extract to game folder
  - `RE2_DisableSharpeningAndTAA.dll` should be in `\GAME_FOLDER\reframework\plugins\managed\RE2_DisableSharpeningAndTAA.dll`

- If the `csharp-api` is installed correctly a CMD window will pop up when launching the game
- The first startup after installing the `csharp-api` might take a while. Wait until it is complete. When the game isn't frozen anymore and it says "setting up script watcher" it is done
- The mod settings are under `REFramework.NET script generated UI` instead of the normal `Script generated UI`

## Features
- Disable sharpening and TAA

## Changelog
### v1.0.0
- Initial release

### v1.0.0-1
- Add lua version
