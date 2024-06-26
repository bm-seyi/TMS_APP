using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.UI;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Ogc;
using Esri.ArcGISRuntime.Reduction;
using System.Xml;

namespace TMS_APP.Pages
{
    public partial class Hub : ContentPage 
    {
        private readonly GraphicsOverlay _graphicsOverlay = new GraphicsOverlay();

        public Hub()
        {
            InitializeComponent();
            InitializeMap();
        }

        private async void InitializeMap()
        {
            Map map = new Map(BasemapStyle.ArcGISDarkGray)
            {
                InitialViewpoint = new Viewpoint(new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84), 100000)
            };

            mapView.Map = map;
            mapView.GraphicsOverlays?.Add(_graphicsOverlay);
            
            const string linesUrl = "https://raw.githubusercontent.com/Big-Man-Seyi/metrolinkData/main/MetrolinkResources/Map/Network/Metrolink.kml";
            const string stopsUrl = "https://raw.githubusercontent.com/Big-Man-Seyi/metrolinkData/main/MetrolinkResources/Map/RailwayStationNode.kml";
            mapView.Map.OperationalLayers.Add(AddLayer(linesUrl));
            ClusterPoints(stopsUrl);

            await mapView.Map.LoadAsync();

            var simpleMarkerSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Teal, 12);
            var simpleRenderer = new SimpleRenderer(simpleMarkerSymbol);
            var clusteringFeatureReduction = new ClusteringFeatureReduction(simpleRenderer)
            {
                Radius = 60,
                IsEnabled = true,
                Renderer = simpleRenderer
            };
            _graphicsOverlay.FeatureReduction = clusteringFeatureReduction;
            _graphicsOverlay.FeatureReduction.IsEnabled = true;
            

        }

        private static KmlLayer AddLayer(string url) => new KmlLayer(new KmlDataset(new Uri(url)));

        private void ClusterPoints(string url)
        {
            XmlDocument kmlDocument = new XmlDocument();
            kmlDocument.Load(url);
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(kmlDocument.NameTable);
            namespaceManager.AddNamespace("kml", "http://www.opengis.net/kml/2.2");

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
                    const string pictureUrl = "https://github.com/Big-Man-Seyi/metrolinkData/blob/main/MetrolinkResources/Icons/stopIcon_small.png?raw=true";
                    
                   PictureMarkerSymbol pictureMarkerSymbol = new PictureMarkerSymbol(new Uri(pictureUrl))
                    {
                        Width = 40,
                        Height = 40
                    }
                   ;
                    _graphicsOverlay.Renderer = new SimpleRenderer(pictureMarkerSymbol);

                    Graphic graphic = new Graphic(mapPoint, pictureMarkerSymbol);
                    _graphicsOverlay.Graphics.Add(graphic);
                }
            }
            
        }

    }
}

