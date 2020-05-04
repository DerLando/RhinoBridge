using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
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
        public Guid[] AddGeometry(GeometryInformation information)
        {
            // deselect all objects
            DeselectAllObjects();

            // run the import script
            var script = BuildScript(information);
            RhinoApp.RunScript(script, false);

            // get the ids
            var ids =
                (from rhinoObject
                    in _doc.Objects.GetSelectedObjects(false, false)
                select rhinoObject.Id)
                .ToArray();

            // props come in mapped with their z axis to the rhino y axis,
            // we need to rotate them around the x axis by pi/2
            foreach (var guid in ids)
            {
                var obj = GetObjectFromId(guid);
                RemapPropAxis(obj);
            }

            // the imported asset will be selected
            return ids;
        }

        /// <summary>
        /// Adds the 3d prop with the correct textures applied
        /// </summary>
        /// <param name="information"></param>
        public void AddTexturedGeometry(GeometryInformation information, RenderMaterial material)
        {
            // add the geometry
            var ids = AddGeometry(information);

            // create a new instance of material access
            var matAccess = new MaterialData(_doc);

            // apply the texture
            foreach (var guid in ids)
            {
                matAccess.TextureExistingGeometry(material, guid);
            }

        }

        /// <summary>
        /// Build the import script to be run by the rhino application
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        private string BuildScript(GeometryInformation information)
        {
            return $"{COMMAND} \"{information.FilePath}\" {OPTIONS} _Enter";
        }

        /// <summary>
        /// Deselect all objects in the rhino document
        /// </summary>
        private void DeselectAllObjects()
        {
            _doc.Objects.UnselectAll();
        }

        /// <summary>
        /// Remap the Z Axis of a prop from Rhino Y to Rhino Z
        /// </summary>
        /// <param name="obj"></param>
        private void RemapPropAxis(RhinoObject obj)
        {
            // Rotate geometry around x axis
            obj.Geometry.Transform(Transform.Rotation(Math.PI / 2.0, Vector3d.XAxis, Point3d.Origin));

            // commit changes
            obj.CommitChanges();
        }
    }
}
