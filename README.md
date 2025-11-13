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
To install the RDP Plugin, follow these steps:
1. Download the latest release from the [Releases](https://github.com/DKorablin/Plugin.RDP/releases)
2. Extract the downloaded ZIP file to a desired location.
3. Use the provided [Flatbed.Dialog (Lite)](https://dkorablin.github.io/Flatbed-Dialog-Lite) executable or download one of the supported host applications:
	- [Flatbed.Dialog](https://dkorablin.github.io/Flatbed-Dialog)
	- [Flatbed.MDI](https://dkorablin.github.io/Flatbed-MDI)
	- [Flatbed.MDI (WPF)](https://dkorablin.github.io/Flatbed-MDI-Avalon)

## Usage
- Open RDP Plugin from the host application's Plugins menu. Default location: Tools -> RDP Client
- Create nodes and clients inside the tree.
- Edit a client to specify server, credentials and options.
- Connect to a client by double-clicking or using the context menu.
- Use configured shortcut keys inside the remote session.

## Configuration
Settings are stored as an embedded XML blob. They are loaded on startup and saved after changes (see `SettingsBll`).

## Acknowledgements
Inspired by Microsoft RDCMan. This project adds custom features and hot key flexibility.