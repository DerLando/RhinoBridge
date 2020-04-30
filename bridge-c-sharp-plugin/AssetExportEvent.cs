using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bridge_c_sharp_plugin
{
    /// <summary>
    /// Event args for asset events
    /// </summary>
    public class AssetExportEventArgs: EventArgs
    {

        public Asset Asset { get; }

        public AssetExportEventArgs(Asset asset)
        {
            Asset = asset;
        }
    }
}
