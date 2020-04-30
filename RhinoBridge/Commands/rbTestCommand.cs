using System;
using Rhino;
using Rhino.Commands;

namespace RhinoBridge.Commands
{
    public class rbTestCommand : Command
    {
        static rbTestCommand _instance;
        public rbTestCommand()
        {
            _instance = this;
        }

        ///<summary>The only instance of the rbTestCommand command.</summary>
        public static rbTestCommand Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "rbTestCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.

            // Start the bridge server
            RhinoBridgePlugIn.Instance.StartServer();

            return Result.Success;
        }
    }
}