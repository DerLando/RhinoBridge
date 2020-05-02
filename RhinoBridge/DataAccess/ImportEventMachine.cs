using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;
using Rhino;
using RhinoBridge.Converters;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Handles the execution of an import event
    /// </summary>
    public class ImportEventMachine : DataAccessBase
    {
        private readonly Asset _asset;

        public ImportEventMachine(AssetExportEventArgs args)
        {
            _asset = args.Asset;
        }

        public void Execute()
        {
            // Give some feedback
            RhinoApp.WriteLine($"Importing asset {_asset.name}");

            // create the render material from the asset
            var mat = RenderContentFactory.CreateMaterial(_asset, _doc);

            // get data access
            var materialData = new MaterialData(_doc);

            // add preview geometry
            materialData.AddTexturedSphere(mat);

            // Redraw scene and give feedback
            RhinoApp.WriteLine($"Finished importing {_asset.name}");
        }
    }
}
