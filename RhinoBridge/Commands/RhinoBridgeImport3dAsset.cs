using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using bridge_c_sharp_plugin;
using Rhino;
using Rhino.Commands;
using RhinoBridge.Data;
using RhinoBridge.DataAccess;
using RhinoBridge.Factories;

namespace RhinoBridge.Commands
{
    [CommandStyle(Style.ScriptRunner)]
    public class RhinoBridgeImport3dAsset : Command
    {
        private GeometryInformation[] _geometryInfos = new GeometryInformation[0];
        private Asset _asset;

        /// <summary>
        /// Sets the geometry information this command will import
        /// </summary>
        /// <param name="infos"></param>
        public void SetInfos(IEnumerable<GeometryInformation> infos)
        {
            _geometryInfos = infos.ToArray();
        }

        /// <summary>
        /// Sets the asset this command will import
        /// </summary>
        /// <param name="asset"></param>
        public void SetAsset(Asset asset)
        {
            _asset = asset;
        }

        /// <summary>
        /// Helper function to determine if the command can run, given the information
        /// it has stored in its backing fields
        /// </summary>
        /// <returns></returns>
        private bool CanRun()
        {
            return _geometryInfos.Length > 0;
        }

        static RhinoBridgeImport3dAsset _instance;
        public RhinoBridgeImport3dAsset()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RhinoBridgeImport3dAsset command.</summary>
        public static RhinoBridgeImport3dAsset Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RhinoBridgeImport3dAsset"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // If we cannot run, notify the user of such
            if (!CanRun())
            {
                RhinoApp.WriteLine($"No assets queued for import, try running export from bridge again.");
                return Result.Nothing;
            }

            // disable viewport drawing
            doc.Views.RedrawEnabled = false;

            // create data access endpoints
            var propAccess = new PropData(doc);
            var matAccess = new MaterialData(doc);

            // create the render material for the asset
            var mat = RenderContentFactory.CreateMaterial(_asset, doc, RhinoBridgePlugIn.FBX_UNIT_SYSTEM);

            // Add it to the document
            matAccess.AddRenderMaterial(mat);

            // iterate over all geometry informations
            foreach (var geometryInformation in _geometryInfos)
            {
                // Add them to the document, textured
                propAccess.AddTexturedGeometry(geometryInformation, mat);
            }

            // Re-enable viewport redrawing
            doc.Views.RedrawEnabled = true;

            // Manually redraw the viewport once
            doc.Views.Redraw();

            // reset geometry infos so we can't run again
            _geometryInfos = new GeometryInformation[0];

            return Result.Success;
        }
    }
}