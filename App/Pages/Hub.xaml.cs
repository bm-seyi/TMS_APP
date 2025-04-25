using Esri.ArcGISRuntime.Mapping;
using Microsoft.Extensions.Logging;
using TMS_APP.HubServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using TMS_APP.Models;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Symbology;


namespace TMS_APP.Pages
{
    public partial class Hub : ContentPage
    {
        private GraphicsOverlay _linesOverlay = new GraphicsOverlay();
        private readonly ILogger<Hub> _logger;
        private readonly ILinesHubService _linesHubService;
        public Hub(ILogger<Hub> logger, ILinesHubService linesHubService)
        {
            InitializeComponent();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _linesHubService = linesHubService ?? throw new ArgumentNullException(nameof(linesHubService));
            InitializeMap();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await _linesHubService.ConnectAsync(""); //TODO: Create necessary class to handle access token and pass it here

                _linesHubService.OnLinesDataReceived += OnLinesDataReceived;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Connection Error", ex.Message, "OK");
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            _linesHubService.OnLinesDataReceived -= OnLinesDataReceived;

            await _linesHubService.DisconnectAsync();
        }
        
        private async void InitializeMap()
        {
            Map map = new Map(BasemapStyle.ArcGISDarkGray)
            {
                InitialViewpoint = new Viewpoint(new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84), 100000)
            };

            mapView.Map = map;

            await mapView.Map.LoadAsync();
        }
        
        private void OnLinesDataReceived(List<LinesModel> linesData)
        {

            MainThread.BeginInvokeOnMainThread(() =>
            {

                _linesOverlay.Graphics.Clear();

                HashSet<MapPoint> mapPolyLines = new HashSet<MapPoint>();

                foreach (var line in linesData)
                {
                    MapPoint mapPoint = new MapPoint(line.Longitude, line.Latitude, SpatialReferences.Wgs84);
                    mapPolyLines.Add(mapPoint);
                }

                SimpleLineSymbol simpleLineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 2);

                Polyline polyline = new Polyline(mapPolyLines);
                Graphic polyGraphic = new Graphic(polyline, simpleLineSymbol);
                _linesOverlay.Graphics.Add(polyGraphic);

            });
        }
    }
}

       