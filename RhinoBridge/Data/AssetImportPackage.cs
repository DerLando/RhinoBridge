using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Render;
using RhinoBridge.DataAccess;

namespace RhinoBridge.Data
{
    /// <summary>
    /// A Data package handling asset imports
    /// </summary>
    public class AssetImportPackage : IImportablePackage
    {
        private readonly RenderMaterial _material;
        private readonly IEnumerable<GeometryInformation> _geometryInfos;
        private readonly RhinoDoc _doc;

        public AssetImportPackage(RenderMaterial material, IEnumerable<GeometryInformation> geometryInfos, RhinoDoc doc)
        {
            _material = material;
            _geometryInfos = geometryInfos;
            _doc = doc;
        }

        public override string ToString()
        {
            return _material.Name;
        }

        #region Import

        /// <summary>
        /// The delegate used for import
        /// </summary>
        /// <param name="material"></param>
        /// <param name="geometry"></param>
        private delegate void AddGeometry(RenderMaterial material, GeometryInformation geometry);

        /// <summary>
        /// The handler handling <see cref="AddGeometry"/>, wrapping <seealso cref="PropData.AddTexturedGeometry"/>
        /// </summary>
        /// <param name="material"></param>
        /// <param name="geometry"></param>
        private void AddGeometryHandler(RenderMaterial material, GeometryInformation geometry)
        {
            // get data access
            var propData = new PropData(_doc);

            // Add the geometry
            propData.AddTexturedGeometry(geometry, material);
        }

        /// <summary>
        /// Write the contents of the package to the <see cref="Rhino.RhinoDoc"/>
        /// </summary>
        public void WriteToDocument()
        {
            // create delegate handler
            AddGeometry handler = AddGeometryHandler;

            // iterate over geometries
            foreach (var geometryInformation in _geometryInfos)
            {
                // Invoke import on Rhinos main thread
                RhinoApp.InvokeOnUiThread(handler, new Object[]{_material, geometryInformation});
            }
        }

        #endregion
    }
}
