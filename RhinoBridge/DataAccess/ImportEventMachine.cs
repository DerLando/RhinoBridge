using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;
using Rhino;
using RhinoBridge.Commands;
using RhinoBridge.Converters;
using RhinoBridge.Extensions;
using RhinoBridge.Factories;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Handles the execution of an import event
    /// </summary>
    public class ImportEventMachine : DataAccessBase
    {
        public delegate void PropImportEventHandler(GeometryExportEventArgs e);

        public static event PropImportEventHandler RaisePropImport;

        static void OnRaisePropImport(GeometryExportEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            var handler = RaisePropImport;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                handler.Invoke(e);
            }
        }

        private readonly Asset _asset;

        public ImportEventMachine(AssetExportEventArgs args)
        {
            _asset = args.Asset;
        }

        public void Execute()
        {
            // Give some feedback
            RhinoApp.WriteLine($"Importing asset {_asset.name}");

            // get the asset type
            var type = AssetConverter.ExtractType(_asset);

            switch (type)
            {
                case Data.AssetType.Surface:
                    Execute_Surface();
                    break;
                case Data.AssetType.Prop:
                    Execute_Prop();
                    break;
                case Data.AssetType.Plant:
                    Execute_Prop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //// Redraw scene and give feedback
            //RhinoApp.WriteLine($"Finished importing {_asset.name}");
        }

        private void Execute_Prop()
        {
            //// get prop data access
            //var propData = new PropData(_doc);

            //// iterate over asset geometries
            //foreach (var geometry in _asset.geometry)
            //{
            //    // get asset geometry info
            //    var info = geometry.ToGeometryInformation();

            //    // add geometry
            //    propData.AddTexturedGeometry(info);

            //    // TODO: Add materials
            //}

            RhinoApp.WriteLine(
                $"Dynamic import of 3d assets is not currently supported. Please run the {RhinoBridgeImport3dAsset.Instance.EnglishName} command instead.");

            // set backing fields on command
            RhinoBridgeImport3dAsset.Instance.SetInfos(from geom in _asset.geometry select geom.ToGeometryInformation());
            RhinoBridgeImport3dAsset.Instance.SetAsset(_asset);
        }

        private void Execute_Surface()
        {
            // create the render material from the asset
            var mat = RenderContentFactory.CreateMaterial(_asset, _doc, RhinoBridgePlugIn.FBX_UNIT_SYSTEM);

            // get data access
            var materialData = new MaterialData(_doc);

            // add a different type of preview geometry, depending on the settings
            switch (RhinoBridgePlugIn.Instance.PreviewType)
            {
                case Settings.TexturePreviewGeometryType.Sphere:
                    materialData.AddTexturedSphere(mat);
                    break;
                case Settings.TexturePreviewGeometryType.Plane:
                    materialData.AddTexturedPlane(mat);
                    break;
                case Settings.TexturePreviewGeometryType.Cube:
                    materialData.AddTexturedCube(mat);
                    break;
                case Settings.TexturePreviewGeometryType.None:
                    materialData.AddRenderMaterial(mat);
                    break;
            }

            RhinoApp.WriteLine($"Finished importing {_asset.name}");

        }
    }
}
