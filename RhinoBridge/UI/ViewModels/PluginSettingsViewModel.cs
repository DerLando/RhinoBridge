using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RhinoBridge.Settings;

namespace RhinoBridge.UI.ViewModels
{
    /// <summary>
    /// A view model representing the plugin settings
    /// </summary>
    public class PluginSettingsViewModel : ViewModelBase
    {
        #region private backing fields

        private int _port;
        private TexturePreviewGeometryType _previewType;
        private bool _shouldScale;
        private AssetImportGeometryFlavor _geometryFlavor;

        #endregion

        #region public properties

        /// <summary>
        /// The port of the bridge server
        /// </summary>
        public int Port { get => _port;
            set
            {
                _port = value;
                RaisePropertyChanged(nameof(Port));
            }

        }

        /// <summary>
        /// The type of preview geometry to use when importing materials
        /// </summary>
        public TexturePreviewGeometryType PreviewType
        {
            get => _previewType;
            set
            {
                _previewType = value;
                RaisePropertyChanged(nameof(PreviewType));
            }
        }

        /// <summary>
        /// If materials should be scaled on import
        /// </summary>
        public bool ShouldScale
        {
            get => _shouldScale;
            set
            {
                _shouldScale = value;
                RaisePropertyChanged(nameof(ShouldScale));
            }
        }

        /// <summary>
        /// The type of geometry an asset should be imported as
        /// </summary>
        public AssetImportGeometryFlavor GeometryFlavor
        {
            get => _geometryFlavor;
            set
            {
                _geometryFlavor = value;
                RaisePropertyChanged(nameof(GeometryFlavor));
            }
        }

        #endregion

        #region Constructor

        public PluginSettingsViewModel()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            Port = RhinoBridgePlugIn.Instance.Port;
            PreviewType = RhinoBridgePlugIn.Instance.PreviewType;
            ShouldScale = RhinoBridgePlugIn.Instance.ShouldScaleMaterials;
            GeometryFlavor = RhinoBridgePlugIn.Instance.AssetGeometryType;
        }

        #endregion
    }
}
