using ActiproSoftware.Windows.Input;
using ArcGIS.Core.Events;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Linq;
using System.Windows.Input;

namespace SketchCompletedEvent.NotFiring.Repro
{
    internal class Dockpane1ViewModel : DockPane
    {
        private const string _dockPaneID = "SketchCompletedEvent_NotFiring_Repro_Dockpane1";
        private const string _layerName = "A polygon layer";

        protected Dockpane1ViewModel() 
        {
            _ = QueuedTask.Run(() =>
            {
                var layer = MapView.Active.Map.GetLayersAsFlattenedList().FirstOrDefault(l => l.Name == _layerName);
                if (layer == null)
                {
                    var layerParams = new LayerCreationParams(new Uri("https://services.arcgis.com/2JyTvMWQSnM2Vi8q/arcgis/rest/services/a_polygon_feature_layer/FeatureServer/0"))
                    {
                        Name = _layerName
                    };

                    LayerFactory.Instance.CreateLayer<FeatureLayer>(layerParams, MapView.Active.Map);
                }
            });


            ActivateVertexEditingCommand = new DelegateCommand<object>(ExecuteActivateVertexEditingCommand);
        }

        private async void ExecuteActivateVertexEditingCommand(object obj)
        {
            await QueuedTask.Run(async () =>
            {

                MapView.Active.Map.ClearSelection();
                var layer = MapView.Active.Map.GetLayersAsFlattenedList().FirstOrDefault(l => l.Name == _layerName) as FeatureLayer;
                if (layer == null)
                {
                    MessageBox.Show("Could not find a layer to work with");
                    return;
                }

                var whereClause = $"ID = '73'";
                layer.Select(new ArcGIS.Core.Data.QueryFilter() { WhereClause = whereClause });

                MapView.Active.ZoomToSelected();

                SubscriptionToken canceledToken = null;
                SubscriptionToken completedToken = null;

                canceledToken = ArcGIS.Desktop.Mapping.Events.SketchCanceledEvent.Subscribe(ags =>
                {
                    ArcGIS.Desktop.Mapping.Events.SketchCanceledEvent.Unsubscribe(canceledToken);
                    ArcGIS.Desktop.Mapping.Events.SketchCompletedEvent.Unsubscribe(completedToken);
                    MessageBox.Show("Hit from SketchCanceledEvent");
                });
                completedToken = ArcGIS.Desktop.Mapping.Events.SketchCompletedEvent.Subscribe(ags =>
                {
                    ArcGIS.Desktop.Mapping.Events.SketchCanceledEvent.Unsubscribe(canceledToken);
                    ArcGIS.Desktop.Mapping.Events.SketchCompletedEvent.Unsubscribe(completedToken);
                    MessageBox.Show("Hit from SketchCompletedEvent");
                });

                await FrameworkApplication.SetCurrentToolAsync(null);
                var editCommand = FrameworkApplication.GetPlugInWrapper("esri_editing_EditVerticesModifyFeature") as ICommand;
                if (editCommand != null && editCommand.CanExecute(null))
                {
                    editCommand.Execute(null);
                }
                else
                {
                    MessageBox.Show("Vertex editing command was not allowed to run");
                }
            });
        }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
        private string _heading = "My DockPane";
        public string Heading
        {
            get => _heading;
            set => SetProperty(ref _heading, value);
        }

        public DelegateCommand<object> ActivateVertexEditingCommand { get; private set; }
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class Dockpane1_ShowButton : Button
    {
        protected override void OnClick()
        {
            Dockpane1ViewModel.Show();
        }
    }
}
