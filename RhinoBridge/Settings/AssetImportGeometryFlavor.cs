using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.DocObjects;

namespace RhinoBridge.Settings
{
    /// <summary>
    /// The different types of geometry an asset can be imported as
    /// </summary>
    public enum AssetImportGeometryFlavor
    {
        /// <summary>
        /// Assets come in by default as Meshes and only have to be passed through
        /// </summary>
        Mesh,

        /// <summary>
        /// The asset will be saved as a <see cref="InstanceDefinition"/>
        /// This is useful if you plan on distributing multiple copies of the asset
        /// </summary>
        Block
    }
}
