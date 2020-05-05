# RhinoBridge
Quixel Bridge PlugIn for Export to Rhinoceros3d

## Liability, Warranty, Suggested use cases

This software is in a **very** early stage of development. Please don't use it for any critical production work, as crashes and bugs are almost guaranteed to happen, as there is not enough feedback yet. Until this officially changes, think of *RhinoBridge* as a toy to play around with.
This plugIn is **not** officially supported, nor developed by Quixel. It is a personal project and might lack the professionalism you are searching for in software.
All the licensing agreements for using Quixel Megascans assets still apply, as *RhinoBridge* is just an importing utility.

## Motivation

This plugIn aims to allow for simple transfer of assets and materials from Quixel Bridge, to Rhinoceros3d.

## Versions

Currently, only Rhino 7 WIP Version 7.0.20119.13305 and Quixel Bridge Version 2020.2.1 are officially supported. Newer versions are expected to *just work*.

## Transfer

To Import from Quixel Bridge, run the `_RhinoBridgeStartServer` command inside of Rhino. After that the plugIn is listening for all *Custom Socket Export*s from Quixel Bridge. Imports will be queued up and imported when ready. It is recommended to import in *Wireframe* mode, as Rhino takes quite a long time to render the assets the first time when using *Rendered* mode.

![Import Material into Rhino](https://github.com/DerLando/RhinoBridge/blob/master/Documentation/Images/Rhino_bridge_import.png)

![Export from quixel bridge](https://github.com/DerLando/RhinoBridge/blob/master/Documentation/Images/Quixel_socket_export.png)

![Import 3d asset into Rhino](https://github.com/DerLando/RhinoBridge/blob/master/Documentation/Images/Rhino_bridge_import_asset.png)

## Customization

*RhinoBridge* exposes an `Options page` where some settings can be customized

### Server settings

 - Port Number: The port number on which quixel bridge exports will be run

### Import Settings

 - Material geometry: The geometry type associated with a material when it is imported
   - Sphere
   - Plane
   - Cube
   - None *(No geometry, the material just gets added to the materials list of the document)*
 - Scale material import: **Experimental** feature to scale pbr materials on import to their expected size. Quixel Bridge materials mostly come in a *Centimeter* unit system, so if you are working in another system, this might come handy
 - Asset geometry: The geometry type an asset should be imported as
   - Block: The asset will be blocked on import. Useful if you want to use multiple copies
   - Mesh: The asset will be imported as single meshes.

## Installation

You can get the latest version of *RhinoBridge* either via the package manager inside of Rhino, using the `_PackageManager` command, or download the latest installable *.rhp* file from https://github.com/DerLando/RhinoBridge/releases/latest

## Issues

Feel free to raise any issues using the Issue tracker on this repository

## Changes

For changes between different versions see https://github.com/DerLando/RhinoBridge/blob/master/CHANGELOG.md
