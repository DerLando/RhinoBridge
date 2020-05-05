using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.UI;

namespace RhinoBridge.UI.Views
{
    public class PluginSettingsPage : OptionsDialogPage
    {
        /// <summary>
        /// Control thats gets drawn as content
        /// </summary>
        private PluginSettingsPageControl _control;

        public PluginSettingsPage(string englishPageTitle) : base(englishPageTitle)
        {
        }

        public override object PageControl => _control ?? (_control = new PluginSettingsPageControl());

        /// <summary>
        /// OnApply override to handle a user clicking "OK" at the bottom of the options page
        /// </summary>
        /// <returns>true</returns>
        public override bool OnApply()
        {
            RhinoBridgePlugIn.Instance.SetPort(_control.Model.Port);
            RhinoBridgePlugIn.Instance.SetPreviewType(_control.Model.PreviewType);
            RhinoBridgePlugIn.Instance.SetShouldScale(_control.Model.ShouldScale);
            RhinoBridgePlugIn.Instance.SetAssetGeometryType(_control.Model.GeometryFlavor);
            return true;
        }

        /// <summary>
        /// Handle user pressing on Cancel
        /// </summary>
        public override void OnCancel()
        {
            // Do nothing
        }

        /// <summary>
        /// We want to show a defaults button
        /// </summary>
        public override bool ShowDefaultsButton => true;

        /// <summary>
        /// Handle the user clicking on the defaults button
        /// </summary>
        public override void OnDefaults()
        {
            RhinoBridgePlugIn.Instance.RestoreDefaultSettings();
            _control.Model.LoadSettings();

            // TODO: instead of newing up, reset the model properties so we hit PropertyChanged Events

            this.Modified = true;
        }
    }
}
