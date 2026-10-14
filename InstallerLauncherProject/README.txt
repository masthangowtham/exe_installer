# Installer Launcher (Windows x64)

This is a small Windows GUI launcher that lets you browse for an `.exe` or `.msi` and launch it.

## Build the standalone EXE on Windows

1. Install the .NET 8 SDK for Windows x64 from:
   https://dotnet.microsoft.com/download/dotnet/8.0
2. Extract this project folder.
3. Open PowerShell in the folder containing `InstallerLauncher.csproj`.
4. Run:

```powershell
dotnet publish -c Release
```

The standalone executable will be created at:

`bin\Release\net8.0-windows\win-x64\publish\InstallerLauncher.exe`

Copy that EXE wherever you need it for testing.

## Important security note

This launcher does not bypass Windows UAC or company IT controls, and it does not grant itself administrator rights. It starts the selected installer using normal Windows mechanisms. Whether the installer runs elevated depends on the installer manifest, Windows policy, and your organization's approved self-service portal configuration.

Only run installers approved by your IT team.
