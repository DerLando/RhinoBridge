using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoBridge.Errors
{
    /// <summary>
    /// Exception for unsupported texture types
    /// </summary>
    public class GeometryFormatNotImplementedException : Exception
    {
        public GeometryFormatNotImplementedException()
        {
            
        }

        public GeometryFormatNotImplementedException(string geometryFormat)
            : base($"Geometry format '{geometryFormat}' is not currently supported!")
        {

        }
    }
}
