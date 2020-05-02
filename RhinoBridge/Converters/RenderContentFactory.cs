using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bridge_c_sharp_plugin;
using Rhino;
using Rhino.Render;
using RhinoBridge.Extensions;

namespace RhinoBridge.Converters
{
    public static class RenderContentFactory
    {
        /// <summary>
        /// Creates a RenderMaterial, which can be added to a rhino document
        /// from a given Asset
        /// </summary>
        /// <param name="asset">The asset to convert</param>
        /// <param name="doc"> The Rhino document to create the RenderMaterial for</param>
        /// <returns></returns>
        public static RenderMaterial CreateMaterial(Asset asset, RhinoDoc doc)
        {
            // create empty material, to fill with asset textures
            var pbr = CreateEmptyMaterial();

            // set name
            pbr.Name = asset.name;

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
            }

            // return material
            return pbr;
        }

        /// <summary>
        /// Creates a new <see cref="RenderMaterial"/> Instance
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
