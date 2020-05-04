using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;

namespace RhinoBridge.Data
{
    /// <summary>
    /// Possible types an <see cref="Asset"/> can have
    /// </summary>
    public enum AssetType
    {
        /// <summary>
        /// A material, without any geometry
        /// </summary>
        Surface,

        /// <summary>
        /// A 3d asset, with a material assigned
        /// </summary>
        Prop,

        /// <summary>
        /// A 3d plant asset
        /// </summary>
        Plant,
    }
}
