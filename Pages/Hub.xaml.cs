using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Ogc;
using Esri.ArcGISRuntime.Reduction;
using System.Xml;
using TMS_APP.Utilities.API;
using TMS_APP.Utilities.API.Schema;
using Microsoft.Extensions.Logging;

namespace TMS_APP.Pages
{
    public partial class Hub : ContentPage 
    {
        private readonly IApiUtilities _apiUtilities;
        private readonly ILogger<Hub> _logger;
        private readonly GraphicsOverlay _graphicsOverlay = new GraphicsOverlay();
        private readonly GraphicsOverlay _trafficOverlay = new GraphicsOverlay();
        private readonly GraphicsOverlay _polylinesOverlay = new GraphicsOverlay();
        private readonly string currentDir;
        private readonly string apiKey;
        const string linesUrl = "https://raw.githubusercontent.com/Big-Man-Seyi/metrolinkData/main/MetrolinkResources/Map/Network/Metrolink.kml";
        const string apiUrl = "https://api.tomtom.com/";
        const string endpoint = "/traffic/services/5/incidentDetails";
       

        // private double _lastZoomLevel = -1;
        // private bool _isUpdating = false;
        // private const double _zoomThreshold = 0.1;

        public Hub(IApiUtilities apiUtilities, ILogger<Hub> logger)
        {
            _logger =  logger;
            _apiUtilities = apiUtilities;
            currentDir = Environment.CurrentDirectory;
            apiKey = Environment.GetEnvironmentVariable("tomtomKey") ?? throw new ArgumentNullException(nameof(apiKey));
            InitializeComponent();
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
            mapView.GraphicsOverlays?.Add(_graphicsOverlay);
            mapView.GraphicsOverlays?.Add(_trafficOverlay);
            mapView.GraphicsOverlays?.Add(_polylinesOverlay);
            
            mapView.Map.OperationalLayers.Add(AddLayer(linesUrl));
            DisplayPoints();
            DisplayTomTom();

            await mapView.Map.LoadAsync();

            mapView.ViewpointChanged += OnExtentChanged;
        }

        private static KmlLayer AddLayer(string url) => new KmlLayer(new KmlDataset(new Uri(url)));
        List<MapPoint> points = new List<MapPoint>();
        private void DisplayPoints()
        {
            string path = DirCombine("TMS_APP/Resources/MetNetwork/RailwayStationNode.kml");
            XmlDocument kmlDocument = new XmlDocument();
            kmlDocument.Load(path);

            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(kmlDocument.NameTable);
            namespaceManager.AddNamespace("kml", "http://www.opengis.net/kml/2.2"); // DevSkim: ignore DS137138

            XmlNodeList xmlNodeList = kmlDocument.GetElementsByTagName("Placemark");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                XmlNode pointsNode = xmlNode.SelectSingleNode("kml:Point/kml:coordinates", namespaceManager) ?? throw new ArgumentNullException(nameof(xmlNode));
                
                string coordinates = pointsNode.InnerText.Trim();
                string[] coordparts = coordinates.Split(',');

                if (coordparts.Length >= 2)
                {
                    double lat = double.Parse(coordparts[1]);
                    double lon = double.Parse(coordparts[0]);
                    
                    MapPoint mapPoint = new MapPoint(lon, lat, SpatialReferences.Wgs84);
                    points.Add(mapPoint);
                    SimpleMarkerSymbol simpleMarkerSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Yellow, 10);
                    
                    _graphicsOverlay.Renderer = new SimpleRenderer(simpleMarkerSymbol);

                    Graphic graphic = new Graphic(mapPoint, simpleMarkerSymbol);
                    _graphicsOverlay.Graphics.Add(graphic);
                }
            }
            
        }
    
        public void DynamicExtent()
        {
            _graphicsOverlay.Graphics.Clear();
            if(mapView.VisibleArea != null && mapView.VisibleArea.SpatialReference != null)
            {
                foreach (MapPoint mapPoint in points)
                {
                    var mapPoint1 = GeometryEngine.Project(mapPoint, mapView.VisibleArea.SpatialReference);
                    if(mapView.VisibleArea.Extent.Contains(mapPoint1))
                    {
                        Graphic graphic = new Graphic(mapPoint);
                        _graphicsOverlay.Graphics.Add(graphic);
                    }
                }
            }
        }

        private void OnExtentChanged(object sender, EventArgs e)
        {
            DynamicExtent();
        }

        private async void DisplayTomTom()
        {
            Dictionary<string, string> payload =  new Dictionary<string, string>
            {
                {"key", apiKey},
                {"bbox", "-2.334,53.355,-1.925,53.630"},
                {"categoryFilter", "1,3,8,9,11,14"},
                {"fields", "{incidents{type,geometry{type,coordinates},properties{id,iconCategory,magnitudeOfDelay,events{description,code,iconCategory},startTime,endTime,from,to,length,delay,roadNumbers,lastReportTime}}}"}

            };

            TomTomRoot root = await _apiUtilities.GetDataFromAPI<TomTomRoot>(apiUrl, endpoint, payload);

            foreach (Incidents incident in root.Incidents)
            {   
                List<MapPoint> mapPoints = new List<MapPoint>();

                foreach (var geometry in incident.Geometry.Coordinates)
                {
                    mapPoints.Add(new MapPoint(geometry[0], geometry[1], SpatialReferences.Wgs84));
                }

                Polyline polyline  = new Polyline(mapPoints);

                SimpleLineSymbol simpleLineSymbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 2);

                Graphic polyGraphic = new Graphic(polyline, simpleLineSymbol);
                _polylinesOverlay.Graphics.Add(polyGraphic);
            }


        }         
        
        private string IconSelector(int type)
        {
            string folderPath = "TMS_APP/Resources/Images/Icons/traffic/";
            switch (type)
            {
                case 1:
                return DirCombine(folderPath + "accident.png");

                case 3:
                return DirCombine(folderPath + "roadWorks.png");

                case 8:
                return DirCombine(folderPath + "roadClosed.png");

                case 9:
                return DirCombine(folderPath + "roadWorks.png");

                case 11:
                return DirCombine(folderPath + "roadWorks.png");

                case 14:
                return DirCombine(folderPath + "roadWorks.png");

                default:
                return DirCombine(folderPath + "roadWorks.png");
            }
        }

        private string DirCombine(string path) => Path.Combine(currentDir, path);

    }
}

