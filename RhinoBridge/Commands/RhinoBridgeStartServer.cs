using System;
using Rhino;
using Rhino.Commands;

namespace RhinoBridge.Commands
{
    public class RhinoBridgeStartServer : Command
    {
        static RhinoBridgeStartServer _instance;
        public RhinoBridgeStartServer()
        {
            _instance = this;
        }

        ///<summary>The only instance of the RhinoBridgeStartServer command.</summary>
        public static RhinoBridgeStartServer Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "RhinoBridgeStartServer"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.

            RhinoBridgePlugIn.Instance.StartServer();

            var port = 24981;
            RhinoApp.WriteLine($"RhinoBridge server listening for custom socket exports on port {port}");

            return Result.Success;
        }
    }
}