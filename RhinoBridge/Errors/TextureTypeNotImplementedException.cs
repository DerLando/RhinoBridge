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
    public class TextureTypeNotImplementedException : Exception
    {
        public TextureTypeNotImplementedException()
        {
            
        }

        public TextureTypeNotImplementedException(string textureType)
            : base($"Texture type {textureType} is not currently supported!")
        {

        }
    }
}
