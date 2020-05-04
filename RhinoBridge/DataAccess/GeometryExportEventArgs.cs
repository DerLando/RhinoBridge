using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoBridge.Data;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Event args vor geometry export events
    /// </summary>
    public class GeometryExportEventArgs: EventArgs
    {
        public GeometryInformation GeometryInformation { get; }

        public GeometryExportEventArgs(GeometryInformation geometryInformation)
        {
            GeometryInformation = geometryInformation;
        }
    }
}
