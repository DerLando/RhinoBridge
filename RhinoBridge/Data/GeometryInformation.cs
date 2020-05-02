using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;

namespace RhinoBridge.Data
{
    /// <summary>
    /// Information Data container for <see cref="Geometry"/>
    /// </summary>
    public readonly struct GeometryInformation
    {
        /// <summary>
        /// File format the geometry mesh is saved in
        /// </summary>
        public readonly GeometryFormat Format;

        /// <summary>
        /// Location of the geometry mesh file
        /// </summary>
        public readonly string FilePath;

        public GeometryInformation(GeometryFormat format, string filePath)
        {
            Format = format;
            FilePath = filePath;
        }
    }
}
