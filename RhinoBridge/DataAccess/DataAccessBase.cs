using Rhino;

namespace RhinoBridge.DataAccess
{
    public abstract class DataAccessBase
    {
        protected RhinoDoc _doc;

        protected DataAccessBase()
        {
            _doc = RhinoDoc.ActiveDoc;
        }

        protected DataAccessBase(RhinoDoc doc)
        {
            _doc = doc;
        }
    }
}