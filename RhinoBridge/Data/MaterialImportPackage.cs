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
    /// A Package that handles importing a PBR Material
    /// </summary>
    public class MaterialImportPackage : IImportablePackage
    {
        private readonly RenderMaterial _material;
        private readonly RhinoDoc _doc;

        public MaterialImportPackage(RenderMaterial material, RhinoDoc doc)
        {
            _material = material;
            _doc = doc;
        }

        public override string ToString()
        {
            return _material.Name;
        }

        #region Import

        /// <summary>
        /// Delegate to be called inside of rhinos ui thread
        /// </summary>
        /// <param name="mat"></param>
        private delegate void AddMaterial(RenderMaterial mat);

        /// <summary>
        /// Handles <see cref="MaterialImportPackage.AddMaterial"/> as a wrapper around <seealso cref="MaterialData.AddRenderMaterial"/>
        /// </summary>
        /// <param name="mat"></param>
        private void AddMaterialHandler(RenderMaterial mat)
        {
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
        }

        /// <summary>
        /// Write the contents of the package to the Rhino document
        /// </summary>
        public void WriteToDocument()
        {
            // create delegate handler
            AddMaterial handler = AddMaterialHandler;

            // Invoke import on Rhinos main thread
            RhinoApp.InvokeOnUiThread(handler, new Object[]{_material});
        }

        #endregion


    }
}
