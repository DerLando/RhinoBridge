using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;
using Rhino;
using Rhino.Render;
using RhinoBridge.Commands;
using RhinoBridge.Converters;
using RhinoBridge.Data;
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

        /// <summary>
        /// The Asset this machine is currently processing
        /// </summary>
        private readonly Asset _asset;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="args"></param>
        public ImportEventMachine(AssetExportEventArgs args)
        {
            _asset = args.Asset;
        }

        /// <summary>
        /// Execute the machine to process its contents
        /// </summary>
        public void Execute()
        {
            // get the asset type
            var type = AssetConverter.ExtractType(_asset);

            // switch execution based on the asset type
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
        }

        /// <summary>
        /// Handles processing of 3d assets
        /// </summary>
        private void Execute_Prop()
        {
            // create data access endpoints
            var matAccess = new MaterialData(_doc);

            // create the render material for the asset
            var mat = RenderContentFactory.CreateMaterial(_asset, _doc, RhinoBridgePlugIn.FBX_UNIT_SYSTEM);

            // Add it to the document
            // TODO: This might also not be thread safe, although i never saw it crash
            matAccess.AddRenderMaterial(mat);

            // get all geometry infos
            var geoInfos = from geom in _asset.geometry select geom.ToGeometryInformation();

            // Add to the import queue
            AssetImportQueue.Instance.AddPackage(new AssetImportPackage(mat, geoInfos, _doc));
        }

        /// <summary>
        /// Handles processing of 'raw' materials with no geometry attached
        /// </summary>
        private void Execute_Surface()
        {
            // create the render material from the asset
            var mat = RhinoBridgePlugIn.Instance.ShouldScaleMaterials
                ? RenderContentFactory.CreateMaterial(_asset, _doc, RhinoBridgePlugIn.FBX_UNIT_SYSTEM)
                : RenderContentFactory.CreateMaterial(_asset, _doc);

            // Add to the import queue
            AssetImportQueue.Instance.AddPackage(new MaterialImportPackage(mat, _doc));
        }
    }
}
