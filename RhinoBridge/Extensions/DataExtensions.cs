using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;
using Rhino;
using Rhino.Render;
using RhinoBridge.Converters;
using RhinoBridge.Data;

namespace RhinoBridge.Extensions
{
    /// <summary>
    /// Conversion Extensions for different Data types
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Converts a texture to a TextureInformation
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static TextureInformation ToTextureInformation(this Texture texture)
        {
            return AssetConverter.ExtractInformation(texture);
        }

        /// <summary>
        /// Converts a TextureInformation to a SimulatedTexture
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static SimulatedTexture ToSimulatedTexture(this TextureInformation information)
        {
            return new SimulatedTexture
            {
                Filename = information.FilePath
            };
        }

        /// <summary>
        /// Converts a SimulatedTexture to a RenderTexture
        /// </summary>
        /// <param name="simTex"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static RenderTexture ToRenderTexture(this SimulatedTexture simTex, RhinoDoc doc)
        {
            return RenderTexture.NewBitmapTexture(simTex, doc);
        }
    }
}
