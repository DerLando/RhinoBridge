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
            {Text = "Material geometry", VerticalAlignment = VerticalAlignment.Center};
        private EnumDropDown<TexturePreviewGeometryType> eDD_PreviewType = new EnumDropDown<TexturePreviewGeometryType>();

        private Label lbl_ShouldScale = new Label
        {
            Text = "Scale material import",
            ToolTip =
                "PBR materials don't have a sense of scale, this option will try to scale them to a reasonable displacement value",
            VerticalAlignment = VerticalAlignment.Center
        };
        private CheckBox cB_ShouldScale = new CheckBox();

        private Label lbl_AssetGeometryType = new Label
            {Text = "Asset geometry", VerticalAlignment = VerticalAlignment.Center};
        private EnumDropDown<AssetImportGeometryFlavor> eDD_AssetGeometryType = new EnumDropDown<AssetImportGeometryFlavor>();

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

            cB_ShouldScale.CheckedBinding.Bind(Model, m => m.ShouldScale);

            eDD_AssetGeometryType.SelectedValueBinding.Bind(Model, m => m.GeometryFlavor);

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
            group.AddRow(new DynamicRow(new Control[]{lbl_ShouldScale, cB_ShouldScale}));
            group.AddRow(new DynamicRow(new Control[]{lbl_AssetGeometryType, eDD_AssetGeometryType}));
            layout.Add(group.Create(layout));

            // Add null and set content
            layout.Add(null);
            Content = layout;

        }


    }
}
