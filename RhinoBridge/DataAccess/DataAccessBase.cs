using System;
using Rhino;
using Rhino.DocObjects;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Abstract base class for everything that needs to have
    /// data access to a <see cref="RhinoDoc"/>
    /// </summary>
    public abstract class DataAccessBase
    {
        /// <summary>
        /// Internal reference to the <see cref="RhinoDoc"/> this has access to
        /// </summary>
        protected RhinoDoc _doc;

        protected DataAccessBase()
        {
            _doc = RhinoDoc.ActiveDoc;
        }

        protected DataAccessBase(RhinoDoc doc)
        {
            _doc = doc;
        }

        /// <summary>
        /// Gets the underlying <see cref="RhinoObject"/> instance for a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal RhinoObject GetObjectFromId(Guid id)
        {
            return new ObjRef(id).Object();
        }
    }
}