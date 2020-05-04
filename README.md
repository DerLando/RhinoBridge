# RhinoBridge
Quixel Bridge PlugIn for Export to Rhinoceros3d

## Liability, Warranty, Suggested use cases

This software is in a **very** early stage of development. Please don't use it for any critical production work, as crashes and bugs are almost guaranteed to happen, as there is not enough feedback yet. Until this officially changes, think of *RhinoBridge* as a toy to play around with.
This plugIn is **not** officially supported, not developed by Quixel. It is a personal project and might lack the professionalism you are searching for in software.

## Motivation

This plugIn aims to allow for simple transfer of assets and materials from Quixel Bridge, to Rhinoceros3d.

## Versions

Currently, only Rhino 7 WIP Version 7.0.20119.13305 and Quixel Bridge Version 2020.2.1 are officially supported. Newer versions are expected to *just work*.

## Transfer

To Import from Quixel Bridge, run the `_RhinoBridgeStartServer` command inside of Rhino. After that the plugIn is listening for all *Custom Socket Export*s from Quixel Bridge.

![Import Material into Rhino](https://github.com/DerLando/RhinoBridge/blob/master/Documentation/Images/Rhino_bridge_import.png)

![Export from quixel bridge](https://github.com/DerLando/RhinoBridge/blob/master/Documentation/Images/Quixel_socket_export.png)

To transfer 3d **Assets**, a back-buffer is loaded and Rhino will inform you to run the `_RhinoBridgeImport3dAsset` command.

![Import 3d asset into Rhino](https://github.com/DerLando/RhinoBridge/blob/master/Documentation/Images/Rhino_bridge_import_asset.png)

## Installation

You can get the latest version of *RhinoBridge* either via the package manager inside of Rhino, using the `_PackageManager` command, or download the latest installable *.rhp* file from https://github.com/DerLando/RhinoBridge/releases/latest

## Issues

Feel free to raise any issues using the Issue tracker on this repository
