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
    public class AssetTypeNotImplementedException : Exception
    {
        public AssetTypeNotImplementedException()
        {
            
        }

        public AssetTypeNotImplementedException(string assetType)
            : base($"Asset type {assetType} is not currently supported!")
        {

        }
    }
}
