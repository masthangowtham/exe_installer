# Installer Launcher — cloud build

This project uses GitHub Actions to compile a standalone Windows x64 EXE in the cloud. You do not need to install .NET on your office laptop.

## Build steps

1. Sign in to GitHub and create a **private** repository, for example `installer-launcher`.
2. Upload the contents of this folder to the repository root. Keep the `.github/workflows/build.yml` path intact.
3. Open the repository's **Actions** tab and enable Actions if asked.
4. Select **Build Windows EXE** and click **Run workflow** (or push/commit the files to `main`).
5. Open the completed workflow run. Under **Artifacts**, download `InstallerLauncher-windows-x64`.
6. Extract the downloaded artifact; it contains `InstallerLauncher.exe`.

## What the app does

It provides a file picker for `.exe` and `.msi` installers and starts the selected installer through standard Windows process-launch mechanisms.

## Security limitation

This launcher does not bypass UAC, company controls, or endpoint management. It cannot grant itself administrator rights. Elevation depends on the installer and your organization's approved self-service portal policy. Use only software approved by your IT team.
