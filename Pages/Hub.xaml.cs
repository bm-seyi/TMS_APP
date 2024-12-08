using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Geometry;
using Align = Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Ogc;
using Esri.ArcGISRuntime.Reduction;
using System.Xml;
using TMS_APP.Utilities;
using TMS_APP.Utilities.API.Schema;
using Microsoft.Extensions.Logging;
using Esri.ArcGISRuntime.Symbology;
using System.Linq.Expressions;


namespace TMS_APP.Pages
{
    public partial class Hub : ContentPage 
    {
        private readonly IApiUtilities _apiUtilities;
        private readonly ILogger<Hub> _logger;
        private readonly GraphicsOverlay _graphicsOverlay = new GraphicsOverlay();
        private readonly GraphicsOverlay _polylinesOverlay = new GraphicsOverlay();
        const string linesUrl = "https://raw.githubusercontent.com/Big-Man-Seyi/metrolinkData/main/MetrolinkResources/Map/Network/Metrolink.kml";
        const string apiUrl = "https://api.tomtom.com/";
        const string endpoint = "/traffic/services/5/incidentDetails";
        private bool _isUpdating = false;
        private HashSet<Graphic> pointGraphics = new HashSet<Graphic>();
        string currentDir = Environment.CurrentDirectory;
        string apiKey = Environment.GetEnvironmentVariable("tomtomKey") ?? throw new ArgumentNullException(nameof(apiKey));
        public Hub(IApiUtilities apiUtilities, ILogger<Hub> logger)
        { 
            InitializeComponent();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _apiUtilities = apiUtilities ?? throw new ArgumentNullException(nameof(apiUtilities));
            InitializeMap();
        }

        private async void InitializeMap()
        {
            Map map =  new Map(BasemapStyle.ArcGISDarkGray)
            {
                InitialViewpoint = new Viewpoint(new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84), 100000)
            };

            mapView.Map = map;

            // Adding Overlays to the Map View
            mapView.GraphicsOverlays?.Add(_polylinesOverlay);
            mapView.GraphicsOverlays?.Add(_graphicsOverlay);
            mapView.Map.OperationalLayers.Add(AddLayer(linesUrl));
            LoadKmlPoints();
            LoadTomTomAPI();

            await mapView.Map.LoadAsync();

            mapView.ViewpointChanged += OnViewPointChanged;
        }

        private static KmlLayer AddLayer(string url) => new KmlLayer(new KmlDataset(new Uri(url)));

        private async void OnViewPointChanged(object? sender, EventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                try
                {
                    await Task.Delay(1000);
                    DynamicExtent();
                }
                finally
                {
                    _isUpdating = false;
                }
            }
        }
        public void DynamicExtent()
        {
            if (mapView.VisibleArea != null && mapView.VisibleArea.SpatialReference != null)
            {
                _graphicsOverlay.Graphics.Clear();

                foreach (Graphic graphic in pointGraphics)
                {
                    if (graphic.Geometry != null)
                    {
                        var mapPoint1 = GeometryEngine.Project(graphic.Geometry, mapView.VisibleArea.SpatialReference);
                        if (mapView.VisibleArea.Extent.Contains(mapPoint1) && mapView.MapScale <= 10000)
                        {
                            _graphicsOverlay.Graphics.Add(graphic);
                        }
                    }

                }
            }
        }

        private async Task<TomTomRoot?> GetTomTomAPIAsync()
        {
            Dictionary<string, string> payload = new Dictionary<string, string>
            {
                {"key", apiKey},
                {"bbox", "-2.334,53.355,-1.925,53.630"},
                {"categoryFilter", "1,3,8,9,11,14"},
                {"fields", "{incidents{type,geometry{type,coordinates},properties{id,iconCategory,magnitudeOfDelay,events{description,code,iconCategory},startTime,endTime,from,to,length,delay,roadNumbers,lastReportTime}}}"}
            };

            try
            {
               TomTomRoot tomTomRoot = await _apiUtilities.GetDataFromAPI<TomTomRoot>(apiUrl, endpoint, payload);
               _logger.LogInformation("Data Successfully Retrieved from Tom Tom API");
               return tomTomRoot;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                if (ex.InnerException != null)
                {
                    _logger.LogError(ex.InnerException.Message);
                }
                return null;
            }
        }

        private async void LoadTomTomAPI()
        {
            TomTomRoot? root = await GetTomTomAPIAsync();

            if (root == null || !root.Incidents.Any()) return;

            SimpleLineSymbol simpleLineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 2);

            HashSet<string> UniqueIncidents = new HashSet<string>();

            foreach (Incidents incident in root.Incidents)
            {   
                ProcessIncident(incident, UniqueIncidents, simpleLineSymbol);
            }

        }
        
        private void ProcessIncident(Incidents incidents, HashSet<string> uniqueIncidents, SimpleLineSymbol simpleLineSymbol)
        {
            string from = incidents.Properties.from;
            string to = incidents.Properties.to;

            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) return;

            if (!uniqueIncidents.Contains(from) && !uniqueIncidents.Contains(to))
            {
                AddTomTomPointsGraphic(incidents);
                uniqueIncidents.Add(from);
                uniqueIncidents.Add(to);
            }

            AddTomTomPolylinesGraphic(incidents, simpleLineSymbol);
        }

        private void AddTomTomPointsGraphic(Incidents incidents)
        {
            List<double> geometry = incidents.Geometry.Coordinates[0];

            MapPoint mapPoint = new MapPoint(geometry[0], geometry[1], SpatialReferences.Wgs84);

            PictureMarkerSymbol pictureMarkerSymbol = new PictureMarkerSymbol(new Uri(IconSelector(incidents.Properties.iconCategory)))
            {
                Height = 40,
                Width = 40
            };

            LoadGraphicPoints(mapPoint, pictureMarkerSymbol, pointGraphics);
        }

        private void AddTomTomPolylinesGraphic(Incidents incidents, SimpleLineSymbol simpleLineSymbol)
        {
            HashSet<MapPoint> mapPolyLines = new HashSet<MapPoint>();
            foreach (var geometry in incidents.Geometry.Coordinates)
            {
                mapPolyLines.Add(new MapPoint(geometry[0], geometry[1], SpatialReferences.Wgs84));
            }

            Polyline polyline = new Polyline(mapPolyLines);
            Graphic polyGraphic = new Graphic(polyline, simpleLineSymbol);

            _polylinesOverlay.Graphics.Add(polyGraphic);
        }

        private void LoadKmlPoints()
        {
            SimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Yellow, 10);

            string path = DirCombine("Resources/MetNetwork/RailwayStationNode.kml");
            XmlDocument kmlDocument = new XmlDocument();
            kmlDocument.Load(path);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(kmlDocument.NameTable);
            namespaceManager.AddNamespace("kml", "http://www.opengis.net/kml/2.2"); // DevSkim: ignore DS137138

            XmlNodeList xmlNodeList = kmlDocument.GetElementsByTagName("Placemark");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                try
                {
                    (double, double)? Coordinates = GetCoordinatesPoints(xmlNode, namespaceManager);
                    if (Coordinates == null)
                    {
                        _logger.LogWarning("Coordinates could not be extracted for a Placemark. XML Node details: {XmlNodeDetails}", xmlNode.OuterXml);
                        continue;
                    }

                    var (lat, lng) = Coordinates.Value;

                    string name = GetCoordinatesName(xmlNode, namespaceManager) ?? "Undefined";
                    

                    MapPoint mapPoint = new MapPoint(lat, lng, SpatialReferences.Wgs84);

                    TextSymbol textSymbol = new TextSymbol(name, System.Drawing.Color.White, 10, Align.HorizontalAlignment.Center, Align.VerticalAlignment.Bottom)
                    {
                        OffsetY = -20
                    };

                    LoadGraphicPoints(mapPoint, simpleMarkerSymbol, pointGraphics);
                    LoadGraphicPoints(mapPoint, textSymbol, pointGraphics);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error Processing Placemark: {1}", ex.Message);

                    if (ex.InnerException != null)
                    {
                        _logger.LogError("Inner Exception: {1}", ex.InnerException.Message);
                    }
                }
            }
            
        }

        private (double lat, double lng)? GetCoordinatesPoints(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager)
        {
            XmlNode? pointsNode = xmlNode.SelectSingleNode("kml:Point/kml:coordinates", xmlNamespaceManager);
            if (pointsNode == null) return null;
                
            string[] coordinates = pointsNode.InnerText.Trim().Split(",");

            if (coordinates.Length >= 2 && double.TryParse(coordinates[0], out double lat) && double.TryParse(coordinates[1], out double lng))
            {
                return (lat, lng);
            }

            return null;
        }

        private static string? GetCoordinatesName(XmlNode xmlNode, XmlNamespaceManager xmlNamespaceManager) => xmlNode.SelectSingleNode("kml:name", xmlNamespaceManager)?.InnerText.Trim();
        
        private static void LoadGraphicPoints(MapPoint mapPoint, Symbol symbol, HashSet<Graphic> graphics) => graphics.Add(new Graphic(mapPoint, symbol));
 
        private string IconSelector(int type)
        {
            string folderPath = "Resources/Images/Icons/traffic/";
            switch (type)
            {
                case 8:
                return DirCombine(folderPath + "forbidden.png");

                case 9:
                return DirCombine(folderPath + "road-work.png");

                default:
                return DirCombine(folderPath + "warning.png");
            }
        }

        private string DirCombine(string path) => Path.Combine(currentDir, path);

    }
}

