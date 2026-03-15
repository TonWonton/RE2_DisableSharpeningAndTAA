# RE2_DisableSharpeningAndTAA

## Description
Disable sharpening and TAA in Resident Evil 2 (2019) / Resident Evil 2 Remake.
Why both sharpening and TAA at the same time? Because for some reason sharpening is tied to the TAA settings in some of the RE Engine games. This also means that the mod doesn't work if you are using FSR super resolution.

## Dependencies
- REFrameworkNETPluginConfig https://github.com/TonWonton/REFrameworkNETPluginConfig

## Prerequisites
- REFramework and the REFramework C# API (csharp-api) https://github.com/praydog/REFramework-nightly/releases
- .NET 10.0 Desktop Runtime https://dotnet.microsoft.com/en-us/download/dotnet/10.0

## Installation
1. Install prerequisites
  - REFramework: Install BOTH the `RE2.zip` AND `csharp-api.zip`
  - .NET 10.0: Install `.NET 10.0 Desktop Runtime x64`
2. Download the mod and extract to game folder
  - RE2_DisableSharpeningAndTAA.dll should be in \GAME_FOLDER\reframework\plugins\managed\RE2_DisableSharpeningAndTAA.dll
3. The first startup after installing the REFramework `csharp-api` might take a while. It is done when the game isn't frozen anymore and it says "setting up script watcher"
4. Open the REFramework UI -> `REFramework.NET script generated UI` -> RE2_DisableSharpeningAndTAA. If the mod appears there it is installed correctly

## Features
- Disable sharpening and TAA
- Can enable FXAA. End result is sharpening disabled + FXAA only

## Note
- Does not work if you are using FSR super resolution

## Changelog
### v1.0.0
- Initial release
