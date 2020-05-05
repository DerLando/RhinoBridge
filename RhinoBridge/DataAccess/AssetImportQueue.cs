using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino;
using Rhino.Render;
using RhinoBridge.Data;

namespace RhinoBridge.DataAccess
{
    /// <summary>
    /// Import queue for assets that handles thread-safe importing of larger number of assets
    /// This is implemented as a non-thread-safe singleton, you get access via <seealso cref="AssetImportQueue.Instance"/>
    /// Since the import queue is only ever called from <see cref="RhinoApp.Idle"/> and Rhino being single-threaded
    /// we can have the queue single-threaded also
    /// </summary>
    public sealed class AssetImportQueue
    {
        #region Private fields

        /// <summary>
        /// backing packages that still wait to be imported
        /// </summary>
        private readonly Queue<IImportablePackage> _packages = new Queue<IImportablePackage>();

        /// <summary>
        /// the document to import the packages to
        /// </summary>
        private RhinoDoc _doc;

        /// <summary>
        /// Lazy backing instance
        /// This gets evaluated on the first reference to the instance
        /// which is nice because we can guarantee Rhino is loaded and
        /// has an active document before that call is made
        /// </summary>
        private static readonly Lazy<AssetImportQueue> 
            _lazy = new Lazy<AssetImportQueue>(() => new AssetImportQueue());

        #endregion

        #region Public properties

        /// <summary>
        /// This gives information if the queue can currently import something
        /// </summary>
        public bool CanImport => _packages.Count > 0;

        /// <summary>
        /// The only instance of the <see cref="AssetImportQueue"/>
        /// </summary>
        public static AssetImportQueue Instance => _lazy.Value;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        private AssetImportQueue()
        {
            _doc = RhinoDoc.ActiveDoc;
        }

        #region Public methods

        /// <summary>
        /// Add another import package to the queue
        /// </summary>
        /// <param name="package">To package to add</param>
        public void AddPackage(IImportablePackage package)
        {
            _packages.Enqueue(package);

            RhinoApp.WriteLine($"Added package '{package}' to the queue.");
        }

        /// <summary>
        /// Updates the document this queue imports to
        /// </summary>
        public void UpdateDocument()
        {
            // don't change the document if we have still some work to do
            if (_packages.Count > 0)
                return;

            _doc = RhinoDoc.ActiveDoc;
        }

        #endregion


        #region Importing

        /// <summary>
        /// Imports the next asset in the queue
        /// </summary>
        public void ImportNext()
        {
            // if the stack is empty we can relax
            if (_packages.Count == 0)
                return;

            _doc.Views.RedrawEnabled = false;

            // pop one package of
            var package = _packages.Dequeue();

            // import the package
            package.WriteToDocument();

            // give feedback
            RhinoApp.WriteLine($"Imported package '{package}'.");

            _doc.Views.RedrawEnabled = true;

        }

        #endregion

    }

    /// <summary>
    /// Private helper data class to store a material together with its geometry information
    /// </summary>
    class AssetPackage
    {
        public readonly RenderMaterial Material;
        public readonly IEnumerable<GeometryInformation> Infos;

        public AssetPackage(RenderMaterial material, IEnumerable<GeometryInformation> infos)
        {
            Material = material;
            Infos = infos;
        }
    }
}
