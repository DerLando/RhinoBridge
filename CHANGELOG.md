# 0.1.0

Initial MVP
- Importing of Assets and Materials
- PlugIn Options page with some options exposed

# 0.2.0

## Thread safety

 - Use queue based import system for assets
 - Use queue based import system for materials
 - Implement queues as singletons
 - Make the server handle out a queue to avoid races

## Importing

 - Dynamically import assets from event stream
 - Get rid of `_RhinoBridgeImport3dAsset` Command, this is a breaking change
 - Re-use the same queue for all data types to import
 - Implement asset import as blocks
 - Expose option to choose import geometry flavor of assets

## UI

 - Fix options page not refreshing when `OnDefaults` is called