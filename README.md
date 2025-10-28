# Plugin.RDP
[![Auto build](https://github.com/DKorablin/Plugin.RDP/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/Plugin.RDP/releases/latest)

Remote Desktop plugin that extends classic RDCMan style workflow with modern shortcuts, grouping and persistence. Supports legacy .NET Framework 3.5 and .NET 8 builds.

## Features
- Tree based grouping of RDP connections (folders and clients)
- Editable client settings persisted in plugin storage (XML blob)
- Customizable hot keys (ALT / CTRL+ALT combinations) for remote session control:
  - ALT+TAB / ALT+SHIFT+TAB, ALT+ESC, ALT+SPACE, CTRL+ESC
  - CTRL+ALT+DEL, Full screen toggle, Previous / Next session
- Multi-monitor connection option
- Auto-close window after disconnect (optional)
- Per-connection credentials (user / password / domain)
- Event model for connection state changes and client updates

## Download
Get the latest packaged release: https://github.com/DKorablin/Plugin.RDP/releases/latest

## Installation
1. Download the release archive (.zip or .nupkg).
2. Place the plugin assembly into the host application plugin directory (SAL / host supporting Plugin.RDP).
3. Restart the host application; Plugin.RDP should appear in the plugin list.

## Usage
- Create folders and clients inside the tree.
- Edit a client to specify server, credentials and options.
- Use configured shortcut keys inside the remote session.
- Enable multi-monitor if you want the session to span all displays.

## Configuration
Settings are stored as an embedded XML blob. They are loaded on startup and saved after changes (see SettingsBll).

## Building
Requires .NET SDK supporting net8.0-windows and .NET Framework 3.5 targeting packs.

Typical steps:
- Restore NuGet packages
- Build Plugin.RDP.csproj (Debug or Release)

## Acknowledgements
Inspired by Microsoft RDCMan. This project adds custom features and hot key flexibility.