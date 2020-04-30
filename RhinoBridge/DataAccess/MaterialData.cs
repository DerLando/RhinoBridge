using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Handles access to Material data in a rhino document
    /// </summary>
    public class MaterialData : DataAccessBase
    {
        public int AddMaterial(Material material)
        {
            return _doc.Materials.Add(material);
        }

        public void AddTexturedSphere(Material material)
        {
            // create sphere
            var sphere = new Sphere(Plane.WorldXY, 2);

            // add material to material table
            var index = AddMaterial(material);

            // add sphere to object table
            var id = _doc.Objects.AddSphere(sphere);

            // get sphere object
            var sphereObject = new ObjRef(id).Object();

            // assign material
            sphereObject.Attributes.MaterialSource = ObjectMaterialSource.MaterialFromObject;
            sphereObject.Attributes.MaterialIndex = index;
            sphereObject.CommitChanges();
        }

    }
}
