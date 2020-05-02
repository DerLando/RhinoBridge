using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.DocObjects;
using Rhino.Render;

namespace RhinoBridge.Data
{
    /// <summary>
    /// Texture information
    /// </summary>
    public readonly struct TextureInformation
    {
        /// <summary>
        /// The file path of the texture image of this texture
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        /// The Type as defined inside of Rhinocommon
        /// </summary>
        public readonly TextureType Type;

        /// <summary>
        /// Child slot name of texture type
        /// </summary>
        private readonly string ChildSlotName;

        public TextureInformation(string filePath, TextureType type, string childSlotName)
        {
            FilePath = filePath;
            Type = type;
            ChildSlotName = childSlotName;
        }
    }
}
