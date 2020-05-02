using bridge_c_sharp_plugin;
using Rhino;
using RhinoBridge.Converters;
using RhinoBridge.DataAccess;

namespace RhinoBridge
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class RhinoBridgePlugIn : Rhino.PlugIns.PlugIn

    {
        public RhinoBridgePlugIn()
        {
            Instance = this;
        }

        ///<summary>Gets the only instance of the RhinoBridgePlugIn plug-in.</summary>
        public static RhinoBridgePlugIn Instance
        {
            get; private set;
        }

        /// <summary>
        /// The <see cref="BridgeServer"/> that listens for export events
        /// of Bridge
        /// </summary>
        public BridgeServer Listener { get; set; }

        /// <summary>
        /// Starts the server
        /// </summary>
        public void StartServer()
        {
            // Check if we already have a server running
            if(Listener == null)
                Listener = new BridgeServer();

            //Starts the server in background.
            Listener.StartServer();

            // Subscribe to asset import events
            BridgeImporter.RaiseAssetImport += BridgeImporterOnRaiseAssetImport;
        }

        /// <summary>
        /// Handle Asset export events coming from quixel bridge
        /// </summary>
        /// <param name="e"></param>
        private void BridgeImporterOnRaiseAssetImport(AssetExportEventArgs e)
        {
            new ImportEventMachine(e).Execute();
        }

        /// <summary>
        /// Ends the server
        /// </summary>
        public void EndServer()
        {
            Listener?.EndServer();

            BridgeImporter.RaiseAssetImport -= BridgeImporterOnRaiseAssetImport;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.
        protected override void OnShutdown()
        {
            // make sure to disable the server
            EndServer();

            base.OnShutdown();
        }
    }
}