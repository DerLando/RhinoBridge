namespace RhinoBridge.Data
{
    /// <summary>
    /// Interface that abstracts data packages that know
    /// how to import themselves to a <see cref="Rhino.RhinoDoc"/>
    /// </summary>
    public interface IImportablePackage
    {
        /// <summary>
        /// Write the package to the <see cref="Rhino.RhinoDoc"/>
        /// </summary>
        void WriteToDocument();
    }
}