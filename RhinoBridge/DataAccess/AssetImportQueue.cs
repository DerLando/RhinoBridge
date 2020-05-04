using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Render;
using RhinoBridge.Data;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Import queue for assets that handles thread-safe importing of larger number of assets
    /// </summary>
    public class AssetImportQueue
    {
        /// <summary>
        /// backing packages that still wait to be imported
        /// </summary>
        private Queue<AssetPackage> _packages = new Queue<AssetPackage>();

        /// <summary>
        /// the document to import the packages to
        /// </summary>
        private RhinoDoc _doc;

        /// <summary>
        /// This gives information if the queue can currently import something
        /// </summary>
        public bool CanImport => _packages.Count > 0;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="doc"></param>
        public AssetImportQueue(RhinoDoc doc)
        {
            _doc = doc;
        }

        /// <summary>
        /// Add another import package to the queue
        /// </summary>
        /// <param name="material"></param>
        /// <param name="infos"></param>
        public void AddPackage(RenderMaterial material, IEnumerable<GeometryInformation> infos)
        {
            _packages.Enqueue(new AssetPackage(material, infos));

            RhinoApp.WriteLine($"Added package '{material.Name}' to the queue.");
        }

        /// <summary>
        /// Updates the document this queue imports to
        /// </summary>
        public void UpdateDocument()
        {
            // don't change the document if we have still some work to do
            if (_packages.Count > 0)
                return;

            _doc = RhinoDoc.ActiveDoc;
        }

        #region Importing

        /// <summary>
        /// Delegate to be called inside of rhinos ui thread
        /// </summary>
        /// <param name="info"></param>
        /// <param name="mat"></param>
        private delegate void AddGeo(GeometryInformation info, RenderMaterial mat);

        /// <summary>
        /// Handles <see cref="AddGeo"/> as a wrapper around <seealso cref="PropData.AddTexturedGeometry"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="mat"></param>
        private void AddGeoHandler(GeometryInformation info, RenderMaterial mat)
        {
            new PropData(_doc).AddTexturedGeometry(info, mat);
        }

        /// <summary>
        /// Imports the next asset in the queue
        /// </summary>
        public void ImportNext()
        {
            // if the stack is empty we can relax
            if (_packages.Count == 0)
                return;

            _doc.Views.RedrawEnabled = false;

            // pop one package of
            var package = _packages.Dequeue();

            // create delegate handler
            AddGeo handler = AddGeoHandler;

            // iterate over all geometry informations
            foreach (var geometryInformation in package.Infos)
            {
                // Add them to the document, textured
                RhinoApp.InvokeOnUiThread(handler, new Object[] { geometryInformation, package.Material });
            }

            RhinoApp.WriteLine($"Imported package '{package.Material.Name}'.");

            _doc.Views.RedrawEnabled = true;

        }

        #endregion

    }

    /// <summary>
    /// Private helper data class to store a material together with its geometry information
    /// </summary>
    class AssetPackage
    {
        public readonly RenderMaterial Material;
        public readonly IEnumerable<GeometryInformation> Infos;

        public AssetPackage(RenderMaterial material, IEnumerable<GeometryInformation> infos)
        {
            Material = material;
            Infos = infos;
        }
    }
}
