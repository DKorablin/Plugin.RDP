# Plugin.RDP
[![Auto build](https://github.com/DKorablin/Plugin.RDP/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/Plugin.RDP/releases/latest)

A Remote Desktop client plugin for the SAL Flatbed host applications. It follows the classic Microsoft RDCMan workflow — a tree of saved connections on the left, sessions as tabbed documents — and adds configurable hot keys, automatic session resizing, `.rdp` file import/export and DPAPI password protection.

Built for **.NET Framework 4.8** and **.NET 8 (Windows)**. Uses the Microsoft RDP ActiveX control (MSTSC) installed with Windows.

## Features

### Connection tree
- Group connections into nested folders (nodes); reorder and move them with drag and drop.
- Quick search over the tree.
- Connected and disconnected clients are shown with different icons; each client can use its own icon.
- Several sessions can be open at the same time, each in its own document window.

### Connection settings
Each client has its own settings, edited in the panel next to the tree:

| Tab | Settings |
|---|---|
| General | Name, computer, user, password, domain |
| Connection | Port, connect to console (administrative session), authentication level, idle timeout, Hyper-V VM console (VM ID) |
| Display | Remote desktop size (same as client area, full screen, preset or custom size), color depth |
| Local Resources | Remote sound and sound quality, audio capture, keyboard hook mode, redirection of drives, printers, clipboard, ports, smart cards and Point of Service devices |
| Applications | Program to start on connection, working directory |
| Gateway | RD Gateway server, logon method, credentials, bypass for local addresses, share gateway credentials |
| Performance | Connection speed presets and individual visual effects (wallpaper, themes, animations, font smoothing, desktop composition, etc.) |

### Session window
- **Automatic resize.** When the desktop size is "Same as client area", the remote session follows the window size. The resize mode is set in the plugin settings and applies to new connections:
  - **Auto** (default) — Dynamic, then Reconnect, then SmartSizing
  - **Dynamic** — change the remote resolution without reconnecting (Windows 8.1 / Server 2012 R2 and newer)
  - **Reconnect** — reconnect with the new resolution
  - **SmartSizing** — scale the image without changing the remote resolution
  - **None** — do not resize
- **Configurable hot keys** for key combinations that Windows would otherwise handle locally (see [Hot keys](#hot-keys)).
- **Remote sessions list.** View the sessions on a server and disconnect or log them off. A client can also be logged off directly from its context menu.
- Optionally close the session window after disconnecting.

### .rdp files
- **Import** standard Remote Desktop Connection (`.rdp`) files created by `mstsc.exe`. Several files can be imported at once; the file name becomes the client name.
- **Export** a client to an `.rdp` file that opens in `mstsc.exe`. The saved password can be included, encrypted for the current Windows user in the same format `mstsc.exe` uses.
- Supported properties include address and port, credentials, authentication level, administrative session, display size and color depth, redirection, audio, keyboard hook, performance flags, start program, RD Gateway and Hyper-V VM console (`pcb`).

### Password protection
- Passwords in the settings file are encrypted with Windows DPAPI for the current user by default (**Encrypt password** setting). Turning the setting off rewrites the stored passwords as plain text; turning it on encrypts them again.
- Encrypted passwords can only be decrypted by the same Windows user on the same computer.
- **Auto fill password prompt** (off by default): if the server policy requires the password to be typed every time ("Always prompt for password upon connection"), the plugin can enter the stored password into the Windows Security dialog. It does this once per connection attempt, so a wrong password does not cause repeated attempts or an account lockout. Use it only if you accept the risk of bypassing that policy.

## Download
Get the latest release: https://github.com/DKorablin/Plugin.RDP/releases/latest

Each release contains a ZIP per target framework (`net48`, `net8.0-windows`) with the plugin and the [Flatbed.Dialog (Lite)](https://dkorablin.github.io/Flatbed-Dialog-Lite) host. The plugin is also published as the NuGet package `AlphaOmega.SAL.Plugin.RDP` on GitHub Packages.

## Installation
1. Download the latest release from [Releases](https://github.com/DKorablin/Plugin.RDP/releases).
2. Extract the ZIP file to a folder of your choice.
3. Run the included [Flatbed.Dialog (Lite)](https://dkorablin.github.io/Flatbed-Dialog-Lite) executable, or install the plugin into one of the supported host applications:
	- [Flatbed.Dialog](https://dkorablin.github.io/Flatbed-Dialog)
	- [Flatbed.MDI](https://dkorablin.github.io/Flatbed-MDI)
	- [Flatbed.MDI (WPF)](https://dkorablin.github.io/Flatbed-MDI-Avalon)

## Usage
1. Open the plugin from the host's main menu: **Tools → RDP Client**. The connection tree opens docked on the left.
2. Use the **Add** button (or the folder context menu) to add a node (folder) or a client.
3. Fill in the client settings and save them.
4. Double-click a client, press **Enter**, or choose **Connect** from its context menu. If the client is already connected, the same action brings its window to the front.

### Context menus
- **Client:** Connect, Focus, Disconnect, Log Off, List sessions, Export to .rdp file, Properties.
- **Folder:** Add Client, Add Node, Import .rdp file.

### Keyboard shortcuts in the tree

| Key | Action |
|---|---|
| Enter | Connect, or focus the session if it is already connected |
| Alt+Enter | Open client properties |
| F2 | Rename the selected node |
| Ctrl+N | Add a client |
| Delete / Backspace | Remove the selected node |

## Hot keys
The RDP control only lets you choose the key; the modifier is fixed (ALT or CTRL+ALT). Change the keys in the plugin settings under **ALT hot keys** and **CTRL hot keys**. Changes apply to new connections.

| Sent to the remote session | Default local key |
|---|---|
| ALT+TAB | ALT+PAGE UP |
| ALT+SHIFT+TAB | ALT+PAGE DOWN |
| ALT+ESC | ALT+INSERT |
| ALT+SPACE | ALT+DELETE |
| CTRL+ESC (Start menu) | ALT+HOME |
| CTRL+ALT+DEL | CTRL+ALT+END |
| Toggle full screen | CTRL+ALT+BREAK |

## Configuration

### Plugin settings
Open them from the host's plugin settings window.

| Category | Setting | Default | Description |
|---|---|---|---|
| Appearance | Close after disconnect | On | Close the session window after disconnecting |
| Appearance | Session resize mode | Auto | How the session fits the window (see [Session window](#session-window)) |
| Credentials | Auto fill password prompt | Off | Enter the stored password into the Windows Security dialog when the server requires it |
| Credentials | Encrypt password | On | Store passwords encrypted with DPAPI for the current Windows user |
| ALT hot keys / CTRL hot keys | — | See [Hot keys](#hot-keys) | Local keys that send special key combinations to the remote session |

### Storage
The connection tree and client settings are saved by the host as an XML blob named `RdpClient.xml`, in the host's plugin settings storage. Passwords in this file are DPAPI-encrypted unless **Encrypt password** is off. If you copy the file to another user account or computer, re-enter the passwords there.

## Building from source
Requirements: Windows, Visual Studio 2022 or the .NET 8 SDK (with the .NET Framework 4.8 targeting pack).

```powershell
git clone --recurse-submodules https://github.com/DKorablin/Plugin.RDP.git
cd Plugin.RDP
dotnet build Plugin.RDP.sln -c Release
```

- Shared code comes from git submodules (`Bll.Shared`, `WinForms.Shared`, `Images.Shared`). If you cloned without `--recurse-submodules`, run `git submodule update --init`.
- The `Windows.AxMSTSCLib` package (RDP ActiveX interop) is restored from GitHub Packages (`https://nuget.pkg.github.com/DKorablin/index.json`, see `NuGet.config`). GitHub Packages requires authentication, so add a NuGet source credential with a GitHub token that has the `read:packages` scope.
- Build output goes to `..\bin\Plugin.RDP\<framework>` relative to the solution (see `Directory.Build.props`).

### Project layout

| Path | Contents |
|---|---|
| `Plugin.RDP/PluginWindows.cs` | Plugin entry point: registers the Tools menu item and creates windows |
| `Plugin.RDP/PluginSettings.cs` | Plugin settings shown in the host's settings window |
| `Plugin.RDP/PanelRdpClient.cs` | Connection tree panel with the settings editor |
| `Plugin.RDP/DocumentRdpClient.cs` | Session window hosting the RDP control |
| `Plugin.RDP/RDP/` | RDP ActiveX wrapper (client versions 5–8), resize logic, remote session (WTS) API, credential dialog auto fill |
| `Plugin.RDP/Bll/` | Settings storage (`SettingsBll`, `SettingsDataSet`), `.rdp` file reader/writer, DPAPI password protection |
| `Plugin.RDP/UI/` | Client settings editor, remote sessions list, tree view and helper controls |

### CI
- `test.yml` builds the solution on pull requests.
- `release.yml` runs on pushes to `master`: it increments the version, builds and signs the assemblies, publishes the NuGet package, and creates a GitHub release with ZIP files for each framework.

## Acknowledgements
Inspired by Microsoft [Remote Desktop Connection Manager (RDCMan)](https://learn.microsoft.com/en-us/sysinternals/downloads/rdcman).