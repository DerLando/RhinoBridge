using System.Collections.Generic;

namespace bridge_c_sharp_plugin
{
    //This class represents the data structure of Bridge exported JSON.
    public struct Asset
    {
        public string resolution;
        public int resolutionValue;
        public string category;
        public string type;
        public string id;
        public string name;
        public string path;
        public string textureMimeType;
        public string averageColor;
        public string activeLOD;
        public string[] tags;
        public string[] categories;

        public bool isCustom;
        public int meshVersion;

        public List<Texture> textures;
        public List<Geometry> geometry;
        public List<GeometryLOD> lodList;
        public List<PackedTextures> packedTextures;
        public List<MetaElement> meta;
    }

    public struct Texture
    {
        public string name;
        public string path;
        public string resolution;
        public string format;
        public string type;
    }

    public struct Geometry
    {
        public string path;
        public string name;
        public string format;
        public string type;
    }

    public struct GeometryLOD
    {
        public string lod;
        public string path;
        public string name;
        public string format;
        public string type;
    }

    public struct PackedTextures
    {
        public string name;
        public string path;
        public string resolution;
        public string format;
        public string type;
        public ChannelsData channelsData;
    }

    public struct ChannelsData
    {
        public ChannelInfo Red;
        public ChannelInfo Green;
        public ChannelInfo Blue;
        public ChannelInfo Alpha;
        public ChannelInfo Grayscale;
    }

    public struct ChannelInfo
    {
        public string type;
        public string channel;
    }

    public struct MetaElement
    {
        public string value;
        public string name;
        public string key;
    }
}
