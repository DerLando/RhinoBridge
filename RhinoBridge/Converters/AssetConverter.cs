using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;
using Rhino;
using Rhino.DocObjects;
using Rhino.Render;
using RhinoBridge.Data;
using Texture = bridge_c_sharp_plugin.Texture;

namespace RhinoBridge.Converters
{
    /// <summary>
    /// Converts asset to a Rhino friendly format
    /// </summary>
    public static class AssetConverter
    {
        /// <summary>
        /// Converts an asset to a material 
        /// </summary>
        /// <param name="asset">asset to convert</param>
        public static Material Convert(Asset asset)
        {
            var mat = new Material
            {
                Name = asset.name
            };

            // create new bpr style rendermaterial
            var pbr = RenderContentType
                    .NewContentFromTypeId(
                        ContentUuids.PhysicallyBasedMaterialType)
                as RenderMaterial;

            // this could be nicer with pbr materials in rhino 7
            foreach (var texture in asset.textures)
            {
                //switch (GetTextureType(texture))
                //{
                //    case TextureType.Albedo:
                //        mat.SetBitmapTexture(texture.path);
                //        break;
                //    case TextureType.AmbientOcclusion:
                //        break;
                //    case TextureType.Cavity:
                //        break;
                //    case TextureType.Displacement:
                //        break;
                //    case TextureType.Gloss:
                //        break;
                //    case TextureType.Normal:
                //        mat.SetBumpTexture(texture.path);
                //        break;
                //    case TextureType.Roughness:
                //        break;
                //    case TextureType.Specular:
                //        break;
                //    case TextureType.Undefined:
                //        break;
                //}
                var convertedTexture = Convert(texture);

                mat.SetTexture(convertedTexture, convertedTexture.TextureType);
            }

            mat.DiffuseColor = GetAverageColor(asset);

            return mat;

        }

        /// <summary>
        /// Gets the <see cref="TextureType"/> associated with a given <see cref="bridge_c_sharp_plugin.Texture"/>
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static TextureType GetTextureType(bridge_c_sharp_plugin.Texture texture)
        {
            if (texture.type == "albedo") return TextureType.Albedo;
            if (texture.type == "ao") return TextureType.AmbientOcclusion;
            if (texture.type == "cavity") return TextureType.Cavity;
            if (texture.type == "displacement") return TextureType.Displacement;
            if (texture.type == "gloss") return TextureType.Gloss;
            if (texture.type == "normal") return TextureType.Normal;
            if (texture.type == "roughness") return TextureType.Roughness;
            if (texture.type == "specular") return TextureType.Specular;

            return TextureType.Undefined;
        }

        public static Rhino.DocObjects.TextureType ExtractType(Texture texture)
        {
            if (texture.type == "albedo") return Rhino.DocObjects.TextureType.PBR_BaseColor;
            if (texture.type == "ao") return Rhino.DocObjects.TextureType.PBR_AmbientOcclusion;
            if (texture.type == "cavity") return Rhino.DocObjects.TextureType.PBR_ClearcoatBump;
            if (texture.type == "displacement") return Rhino.DocObjects.TextureType.PBR_Displacement;
            if (texture.type == "gloss") return Rhino.DocObjects.TextureType.PBR_Clearcoat;
            if (texture.type == "normal") return Rhino.DocObjects.TextureType.Bump;
            if (texture.type == "roughness") return Rhino.DocObjects.TextureType.PBR_Roughness;
            if (texture.type == "specular") return Rhino.DocObjects.TextureType.PBR_Specular;

            return Rhino.DocObjects.TextureType.None;
        }

        /// <summary>
        /// Extracts the texture information of a given texture
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static TextureInformation ExtractInformation(Texture texture)
        {
            // TODO: Extract child slot name
            return new TextureInformation(texture.path, ExtractType(texture));
        }

        /// <summary>
        /// Creates a Rhino Texture from a TextureInformation
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public static Rhino.DocObjects.Texture FromInformation(in TextureInformation information)
        {
            return new Rhino.DocObjects.Texture
            {
                FileName = information.FilePath,
                TextureType = information.Type
            };
        }

        public static Rhino.DocObjects.Texture Convert(Texture texture)
        {
            return FromInformation(ExtractInformation(texture));
        }

        /// <summary>
        /// Gets the average color of an asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static Color GetAverageColor(Asset asset)
        {
            return ColorTranslator.FromHtml(asset.averageColor);
        }
    }
}
