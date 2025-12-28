using Esri.ArcGISRuntime.Mapping;
using Microsoft.Extensions.Logging;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Symbology;


namespace TMS_APP.Pages
{
    public partial class Hub : ContentPage
    {
        private GraphicsOverlay _linesOverlay = new GraphicsOverlay();
        private readonly ILogger<Hub> _logger;
        public Hub(ILogger<Hub> logger)
        {
            InitializeComponent();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializeMap();
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
    }
}

       