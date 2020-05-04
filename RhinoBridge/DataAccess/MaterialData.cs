using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Render;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Handles access to Material data in a rhino document
    /// </summary>
    public class MaterialData : DataAccessBase
    {
        public MaterialData(RhinoDoc doc) : base(doc) { }

        /// <summary>
        /// Add an material to the document
        /// </summary>
        /// <param name="material">The material to add</param>
        /// <returns></returns>
        public int AddMaterial(Material material)
        {
            return _doc.Materials.Add(material);
        }

        /// <summary>
        /// Add a RenderMaterial to the document
        /// </summary>
        /// <param name="material">The material to add</param>
        /// <returns></returns>
        public bool AddRenderMaterial(RenderMaterial material)
        {
            // Test if material is already in table
            if (_doc.Materials.Find(material.Name, true) != -1)
                return false;
            
            _doc.RenderMaterials.Add(material);
            return true;
        }

        /// <summary>
        /// Adds a textured sphere with the given material assigned to it
        /// </summary>
        /// <param name="material">The material to assign to the sphere</param>
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

        /// <summary>
        /// Adds a textured sphere with the given material assigned to it
        /// </summary>
        /// <param name="material">The material to assign to the sphere</param>
        public void AddTexturedSphere(RenderMaterial material)
        {
            // create sphere
            var sphere = new Sphere(Plane.WorldXY, 2);

            // add sphere to object table
            var id = _doc.Objects.AddSphere(sphere);

            // Texture the sphere
            TextureExistingGeometry(material, id);
        }

        /// <summary>
        /// Gets the underlying <see cref="RhinoObject"/> instance for a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private RhinoObject GetObjectFromId(Guid id)
        {
            return new ObjRef(id).Object();
        }

        /// <summary>
        /// Assigns a <see cref="RenderMaterial"/> to a <see cref="RhinoObject"/>
        /// </summary>
        /// <param name="material"></param>
        /// <param name="id"></param>
        private void AssignMaterialToObject(RenderMaterial material, Guid id)
        {
            // get the object
            var obj = GetObjectFromId(id);

            // call self with object
            AssignMaterialToObject(material, obj);
        }

        /// <summary>
        /// Assigns a <see cref="RenderMaterial"/> to a <see cref="RhinoObject"/>
        /// </summary>
        /// <param name="material"></param>
        /// <param name="obj"></param>
        private void AssignMaterialToObject(RenderMaterial material, RhinoObject obj)
        {
            obj.Attributes.MaterialSource = ObjectMaterialSource.MaterialFromObject;
            obj.RenderMaterial = material;
            obj.CommitChanges();
        }

        /// <summary>
        /// Tries to find a document assigned version of the given render material
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        private RenderMaterial TryFindDocumentMaterial(RenderMaterial material)
        {
            return _doc.RenderMaterials.First(m => m.Name == material.Name);
        }

        /// <summary>
        /// Textures a geometry, already existing inside of the rhino document
        /// </summary>
        /// <param name="material"></param>
        /// <param name="id"></param>
        public void TextureExistingGeometry(RenderMaterial material, Guid id)
        {
            // Add material to table
            var isNew = AddRenderMaterial(material);

            // test if material already exists
            if (!isNew)
            {
                var found = TryFindDocumentMaterial(material);
                material = found;
            }

            // assign material
            AssignMaterialToObject(material, id);
        }
        

    }
}
