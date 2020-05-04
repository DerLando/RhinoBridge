using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Commands;
using Rhino.Render;
using RhinoBridge.Data;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Handles access to 3d prop data in a rhino document
    /// </summary>
    public class PropData : DataAccessBase
    {
        /// <summary>
        /// Command to call inside of Rhino
        /// </summary>
        private const string COMMAND = "-_Import";

        /// <summary>
        /// Options to supply to the command
        /// </summary>
        private const string OPTIONS = "Unweld=No  ImportMeshesAsSubDs=No  ImportLights=No  ImportCameras=No ";

        public PropData(RhinoDoc doc) : base(doc) { }

        /// <summary>
        /// Adds the given geometry to the document
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public Guid AddGeometry(GeometryInformation information)
        {
            // deselect all objects
            DeselectAllObjects();

            // run the import script
            var script = BuildScript(information);
            RhinoApp.RunScript(script, false);

            // the imported asset will be selected
            return _doc.Objects.GetSelectedObjects(false, false).First().Id;
        }

        /// <summary>
        /// Adds the 3d prop with the correct textures applied
        /// </summary>
        /// <param name="information"></param>
        public void AddTexturedGeometry(GeometryInformation information, RenderMaterial material)
        {
            // add the geometry
            var id = AddGeometry(information);

            // create a new instance of material access
            var matAccess = new MaterialData(_doc);

            // apply the texture
            matAccess.TextureExistingGeometry(material, id);

        }

        private string BuildScript(GeometryInformation information)
        {
            return $"{COMMAND} \"{information.FilePath}\" {OPTIONS} _Enter";
        }

        private static void DeselectAllObjects()
        {
            RhinoApp.RunScript($"_Escape", false);
        }
    }
}
