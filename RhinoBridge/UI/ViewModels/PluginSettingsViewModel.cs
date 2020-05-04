using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoBridge.UI.ViewModels
{
    /// <summary>
    /// A view model representing the plugin settings
    /// </summary>
    public class PluginSettingsViewModel : ViewModelBase
    {
        #region private backing fields

        private int _port;

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

        #endregion

        #region Constructor

        public PluginSettingsViewModel()
        {
            Port = RhinoBridgePlugIn.Instance.Port;
        }

        #endregion
    }
}
