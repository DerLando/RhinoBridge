using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using RhinoBridge.Settings;
using RhinoBridge.UI.ViewModels;

namespace RhinoBridge.UI.Views
{
    public class PluginSettingsPageControl : Panel
    {
        #region Model

        public PluginSettingsViewModel Model { get; set; }

        #endregion

        #region Elements

        // server settings
        private Label lbl_Port = new Label
            {Text = "Port number", ToolTip = "Port number to listen to", VerticalAlignment = VerticalAlignment.Center};
        private TextBox tB_Port = new TextBox();

        // import settings
        private Label lbl_PreviewType = new Label
            {Text = "Preview geometry flavor", VerticalAlignment = VerticalAlignment.Center};
        private EnumDropDown<TexturePreviewGeometryType> eDD_PreviewType = new EnumDropDown<TexturePreviewGeometryType>();

        #endregion

        public PluginSettingsPageControl()
        {
            // new up the view model
            Model = new PluginSettingsViewModel();
            DataContext = Model;

            // apply bindings
            tB_Port
                .TextBinding
                .BindDataContext(
                    Binding
                        .Property((PluginSettingsViewModel m) => m.Port)
                        .Convert(r => r.ToString(), int.Parse)
                );

            eDD_PreviewType.SelectedValueBinding.Bind(Model, m => m.PreviewType);

            // create the layout
            var layout = new DynamicLayout
            {
                Spacing = new Size(5, 5),
                Padding = new Padding(10)
            };

            // Add server settings group
            var group = new DynamicGroup();
            group.Title = "Server settings";
            group.AddRow(new DynamicRow(new Control[]{lbl_Port, tB_Port}));
            layout.Add(group.Create(layout));

            // Add import settings group
            group = new DynamicGroup();
            group.Title = "Import settings";
            group.AddRow(new DynamicRow(new Control[]{lbl_PreviewType, eDD_PreviewType}));
            layout.Add(group.Create(layout));

            // Add null and set content
            layout.Add(null);
            Content = layout;

        }


    }
}
