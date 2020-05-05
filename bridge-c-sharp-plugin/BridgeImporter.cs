/*

>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


██████╗ ██████╗ ██╗██████╗  ██████╗ ███████╗    ██╗███╗   ██╗████████╗███████╗ ██████╗ ██████╗  █████╗ ████████╗██╗ ██████╗ ███╗   ██╗
██╔══██╗██╔══██╗██║██╔══██╗██╔════╝ ██╔════╝    ██║████╗  ██║╚══██╔══╝██╔════╝██╔════╝ ██╔══██╗██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║
██████╔╝██████╔╝██║██║  ██║██║  ███╗█████╗      ██║██╔██╗ ██║   ██║   █████╗  ██║  ███╗██████╔╝███████║   ██║   ██║██║   ██║██╔██╗ ██║
██╔══██╗██╔══██╗██║██║  ██║██║   ██║██╔══╝      ██║██║╚██╗██║   ██║   ██╔══╝  ██║   ██║██╔══██╗██╔══██║   ██║   ██║██║   ██║██║╚██╗██║
██████╔╝██║  ██║██║██████╔╝╚██████╔╝███████╗    ██║██║ ╚████║   ██║   ███████╗╚██████╔╝██║  ██║██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║
╚═════╝ ╚═╝  ╚═╝╚═╝╚═════╝  ╚═════╝ ╚══════╝    ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝

>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Quixel AB - Megascans Project

The Megascans Integration for Custom Exports was written in C# (.Net 4.0)

Megascans : https://megascans.se

This integration gives you a LiveLink between Megascans Bridge and Custom Exports. The source code is all exposed
and documented for you to use it as you wish (within the Megascans EULA limits, that is).
We provide a set of useful functions for importing json data from Bridge.

We've tried to document the code as much as we could, so if you're having any issues
please send me an email (ajwad@quixel.se) for support.

Main function is responsible for starting a thread that listens to the specified port (specified in Bridge_server.cs) for JSON data..

PrintProperties is an example method that demonstrates how you the data is stored in objects.

*/





using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace bridge_c_sharp_plugin
{
    public sealed class BridgeImporter
    {
        #region Private fields
        /// <summary>
        /// backing assets that can be imported
        /// </summary>
        private readonly Queue<Asset> _assets = new Queue<Asset>();

        /// <summary>
        /// lazy backing instance
        /// </summary>
        private static readonly Lazy<BridgeImporter>
            _lazy = new Lazy<BridgeImporter>(() => new BridgeImporter());

        #endregion

        #region Public properties

        /// <summary>
        /// Gives information if we have assets to export
        /// </summary>
        public bool CanExport => _assets.Count > 0;

        /// <summary>
        /// The only instance of the <see cref="BridgeImporter"/>
        /// </summary>
        public static BridgeImporter Instance => _lazy.Value;

        #endregion

        /// <summary>
        /// default constructor
        /// </summary>
        private BridgeImporter()
        {

        }

        #region Public methods

        public void AddAssets(string jsonData)
        {
            //Parsing JSON array for multiple assets.
            var jArray = jsonData;
            var assetsJsonArray = JArray.Parse(jArray);
            foreach (var jsonAsset in assetsJsonArray)
            {
                //Parsing JSON data.
                _assets.Enqueue(ImportMegascansAssets(jsonAsset.ToObject<JObject>()));
            }

            OnImportsReceived(EventArgs.Empty);
        }

        /// <summary>
        /// Gets the next asset to import.
        /// Check <seealso cref="BridgeImporter.CanExport"/> first!
        /// </summary>
        /// <returns></returns>
        public Asset GetNextAsset()
        {
            return _assets.Dequeue();
        }

        #endregion

        /// <summary>
        /// Raised when the importer receives new assets to import
        /// from the <see cref="BridgeServer"/>
        /// </summary>
        public event EventHandler ImportsReceived;

        void OnImportsReceived(EventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            var handler = ImportsReceived;

            handler?.Invoke(this, e);
        }

        //public delegate void AssetImportEventHandler(AssetExportEventArgs e);

        //public static event AssetImportEventHandler RaiseAssetImport;

        //static void OnRaiseAssetImport(AssetExportEventArgs e)
        //{
        //    // Make a temporary copy of the event to avoid possibility of
        //    // a race condition if the last subscriber unsubscribes
        //    // immediately after the null check and before the event is raised.
        //    var handler = RaiseAssetImport;

        //    // Event will be null if there are no subscribers
        //    // TODO: we can still get a race condition here
        //    if (handler != null)
        //    {
        //        handler.GetInvocationList().
        //    }
        //}

        /// <summary>
        /// Imports a Megascans Asset from a <see cref="JObject"/>
        /// </summary>
        /// <param name="objectList"></param>
        /// <returns></returns>
        static Asset ImportMegascansAssets (JObject objectList)
        {
            Asset asset = new Asset();
            //Parsing asset properties.
            asset.name = (string)objectList["name"];
            asset.id = (string)objectList["id"];
            asset.type = (string)objectList["type"];
            asset.category = (string)objectList["category"];
            asset.path = (string)objectList["path"];
            asset.averageColor = (string)objectList["averageColor"];
            asset.activeLOD = (string)objectList["activeLOD"];
            asset.textureMimeType = (string)objectList["textureFormat"];
            asset.meshVersion = (int)objectList["meshVersion"];
            asset.resolution = (string)objectList["resolution"];
            asset.resolutionValue = int.Parse((string)objectList["resolutionValue"]);
            asset.isCustom = (bool)objectList["isCustom"];
            //Initializing asset component lists to avoid null reference error.
            asset.textures = new List<Texture>();
            asset.geometry = new List<Geometry>();
            asset.lodList = new List<GeometryLOD>();
            asset.packedTextures = new List<PackedTextures>();
            asset.meta = new List<MetaElement>();
            //Parse and store geometry list.
            JArray meshComps = (JArray)objectList["meshList"];

            foreach (JObject obj in meshComps)
            {
                Geometry geo = new Geometry();
                geo.name = (string)obj["name"];
                geo.path = (string)obj["path"];
                geo.type = (string)obj["type"];
                geo.format = (string)obj["format"];

                asset.geometry.Add(geo);
            }
            //Parse and store LOD list.
            JArray lodComps = (JArray)objectList["lodList"];

            foreach (JObject obj in lodComps)
            {
                GeometryLOD geo = new GeometryLOD();
                geo.name = (string)obj["name"];
                geo.path = (string)obj["path"];
                geo.type = (string)obj["type"];
                geo.format = (string)obj["format"];
                geo.lod = (string)obj["lod"];

                asset.lodList.Add(geo);
            }
            //Parse and store meta data list.
            JArray metaData = (JArray)objectList["meta"];

            foreach (JObject obj in metaData)
            {
                MetaElement mElement = new MetaElement();
                mElement.name = (string)obj["name"];
                mElement.value = (string)obj["value"];
                mElement.key = (string)obj["key"];

                asset.meta.Add(mElement);
            }
            //Parse and store textures list.
            JArray textureComps = (JArray)objectList["components"];

            foreach (JObject obj in textureComps)
            {
                Texture tex = new Texture();
                tex.name = (string)obj["name"];
                tex.path = (string)obj["path"];
                tex.type = (string)obj["type"];
                tex.format = (string)obj["format"];
                tex.resolution = (string)obj["resolution"];

                asset.textures.Add(tex);
            }
            //Parse and store channel packed textures list.
            JArray packedTextureComps = (JArray)objectList["packedTextures"];

            foreach (JObject obj in packedTextureComps)
            {
                PackedTextures tex = new PackedTextures();
                tex.name = (string)obj["name"];
                tex.path = (string)obj["path"];
                tex.type = (string)obj["type"];
                tex.format = (string)obj["format"];
                tex.resolution = (string)obj["resolution"];

                tex.channelsData.Red.type = (string)obj["channelsData"]["Red"][0];
                tex.channelsData.Red.channel = (string)obj["channelsData"]["Red"][1];
                tex.channelsData.Green.type = (string)obj["channelsData"]["Green"][0];
                tex.channelsData.Green.channel = (string)obj["channelsData"]["Green"][1];
                tex.channelsData.Blue.type = (string)obj["channelsData"]["Blue"][0];
                tex.channelsData.Blue.channel = (string)obj["channelsData"]["Blue"][1];
                tex.channelsData.Alpha.type = (string)obj["channelsData"]["Alpha"][0];
                tex.channelsData.Alpha.channel = (string)obj["channelsData"]["Alpha"][1];
                tex.channelsData.Grayscale.type = (string)obj["channelsData"]["Grayscale"][0];
                tex.channelsData.Grayscale.channel = (string)obj["channelsData"]["Grayscale"][1];

                asset.packedTextures.Add(tex);
            }
            //Parse and store categories list.
            JArray categories = (JArray)objectList["categories"];
            asset.categories = new string[categories.Count];
            for (int i = 0; i < categories.Count; ++i)
            {
                asset.categories[i] = (string)categories[i];
            }
            //Parse and store tags list.
            JArray tags = (JArray)objectList["tags"];
            asset.tags = new string[tags.Count];
            for (int i = 0; i < tags.Count; ++i)
            {
                asset.tags[i] = (string)tags[i];
            }

            return asset;
        }
    }
}
