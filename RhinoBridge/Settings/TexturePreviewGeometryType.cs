using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoBridge.Settings
{
    /// <summary>
    /// The different types of geometry to apply a newly added texture to
    /// </summary>
    public enum TexturePreviewGeometryType
    {
        /// <summary>
        /// The texture will be applied to a sphere
        /// </summary>
        Sphere,

        /// <summary>
        /// The texture will be applied to a plane
        /// </summary>
        Plane,

        /// <summary>
        /// The texture will be applied to a cube
        /// </summary>
        Cube,

        /// <summary>
        /// The texture will be added, but no preview geometry will be created
        /// </summary>
        None
    }
}
