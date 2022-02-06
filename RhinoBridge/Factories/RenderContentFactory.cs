using bridge_c_sharp_plugin;
using Rhino;
using Rhino.DocObjects;
using Rhino.Render;
using RhinoBridge.Extensions;

namespace RhinoBridge.Factories
{
    /// <summary>
    /// Factory to create different types of <see cref="RenderContent"/>
    /// </summary>
    public static class RenderContentFactory
    {
        /// <summary>
        /// Creates a <see cref="RenderMaterial"/>, which can be added to a rhino document
        /// from a given Asset
        /// </summary>
        /// <param name="asset">The asset to convert</param>
        /// <param name="doc"> The Rhino document to create the RenderMaterial for</param>
        /// <returns></returns>
        public static RenderMaterial CreateMaterial(Asset asset, RhinoDoc doc)
        {
            // Call scale constructor with meaningless unitsystem,
            // this basically generates a material that is unscaled
            return CreateMaterial(asset, doc, doc.ModelUnitSystem);
        }

        /// <summary>
        /// Creates a <see cref="RenderMaterial"/>, which can be added to a rhino document
        /// from a given Asset and unit system
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="doc"></param>
        /// <param name="unitSystem"></param>
        /// <returns></returns>
        public static RenderMaterial CreateMaterial(Asset asset, RhinoDoc doc, UnitSystem unitSystem)
        {
            // TODO: asset has a 'meta' property which contains scan Area in meters and height (displacement) in meters
            // From that info we should be able to generate nicely scaled assets.
            // We just have to watch out for 3d assets here, as their material has to be differently scaled.
            // So maybe there are two constructors?

            // calculate displacement amount
            var displacementAmount = RhinoMath.UnitScale(unitSystem, doc.ModelUnitSystem) * 0.01;
            //var displacementAmount = 1.0;

            // create empty material, to fill with asset textures
            var pbr = CreateEmptyMaterial();

            // set name
            pbr.Name = $"{asset.name}-{asset.id}";

            // iterate over textures
            foreach (var texture in asset.textures)
            {
                // get texture information
                var information = texture.ToTextureInformation();

                // create render texture from it
                var renderTexture = information
                    .ToSimulatedTexture()
                    .ToRenderTexture(doc);

                // add render texture as a child
                pbr.SetChild(renderTexture, information.ChildSlotName);
                pbr.SetChildSlotOn(information.ChildSlotName, true, RenderContent.ChangeContexts.Ignore);

                // add bump / displacement
                switch (information.Type)
                {
                    case TextureType.Bump:
                        pbr.SetChildSlotAmount(information.ChildSlotName, 100.0, RenderContent.ChangeContexts.Ignore);
                        break;
                    case TextureType.PBR_Displacement:
                        pbr.SetChildSlotAmount(information.ChildSlotName, displacementAmount, RenderContent.ChangeContexts.Ignore);
                        break;
                    default:
                        break;
                }
            }

            // return material
            return pbr;
        }

        /// <summary>
        /// Creates a new empty <see cref="RenderMaterial"/> Instance
        /// </summary>
        /// <returns></returns>
        private static RenderMaterial CreateEmptyMaterial()
        {
            return RenderContentType
                    .NewContentFromTypeId(
                        ContentUuids.PhysicallyBasedMaterialType)
                as RenderMaterial;
        }
    }
}
