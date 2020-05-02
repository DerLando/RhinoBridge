using bridge_c_sharp_plugin;
using Rhino.DocObjects;
using Rhino.Render;
using RhinoBridge.Data;
using RhinoBridge.Errors;
using Texture = bridge_c_sharp_plugin.Texture;

namespace RhinoBridge.Converters
{
    /// <summary>
    /// Converts asset to a Rhino friendly format
    /// </summary>
    public static class AssetConverter
    {
        /// <summary>
        /// Extracts the <see cref="TextureType"/> from a given <see cref="Texture"/>
        /// </summary>
        /// <param name="texture">The texture to extract the type from</param>
        /// <returns></returns>
        public static TextureType ExtractType(Texture texture)
        {
            if (texture.type == "albedo") return TextureType.PBR_BaseColor;
            if (texture.type == "ao") return TextureType.PBR_AmbientOcclusion;
            if (texture.type == "cavity") return TextureType.PBR_ClearcoatBump;
            if (texture.type == "displacement") return TextureType.PBR_Displacement;
            if (texture.type == "gloss") return TextureType.PBR_Clearcoat;
            if (texture.type == "normal") return TextureType.Bump;
            if (texture.type == "roughness") return TextureType.PBR_Roughness;
            if (texture.type == "specular") return TextureType.PBR_Specular;

            throw new TextureTypeNotImplementedException(texture.type);
        }

        /// <summary>
        /// Extracts the <see cref="AssetType"/> from a given <see cref="Asset"/>
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static AssetType ExtractType(Asset asset)
        {
            if (asset.type == "surface") return AssetType.Surface;
            if (asset.type == "3d") return AssetType.Prop;

            throw new AssetTypeNotImplementedException(asset.type);
        }

        /// <summary>
        /// Extracts the texture information of a given texture
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public static TextureInformation ExtractInformation(Texture texture)
        {
            // Extract the texture type
            var type = ExtractType(texture);

            // convert type to slotName
            var slotName = RenderMaterial.PhysicallyBased.ChildSlotNames.FromTextureType(type);

            return new TextureInformation(texture.path, type, slotName);
        }

        /// <summary>
        /// Extracts the <see cref="GeometryFormat"/> of a given <see cref="Geometry"/>
        /// </summary>
        /// <param name="geometry">The geometry to extract the format of</param>
        /// <returns></returns>
        public static GeometryFormat ExtractFormat(Geometry geometry)
        {
            if (geometry.format == "fbx") return GeometryFormat.Fbx;

            throw new GeometryFormatNotImplementedException(geometry.format);
        }

        /// <summary>
        /// Extracts the relevant <see cref="GeometryInformation"/> from a given <see cref="Geometry"/>
        /// </summary>
        /// <param name="geometry">The geometry to extract information from</param>
        /// <returns></returns>
        public static GeometryInformation ExtractInformation(Geometry geometry)
        {
            // Extract the geometry format
            var format = ExtractFormat(geometry);

            return new GeometryInformation(format, geometry.path);
        }

    }
}
