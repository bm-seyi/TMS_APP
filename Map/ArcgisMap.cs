using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Ogc;

namespace TMS_APP.ArcgisMap
{

    internal class MapViewModel : INotifyPropertyChanged
    {

        public MapViewModel()
        {
            Map newMap = InitializeMap();

            newMap.OperationalLayers.Add(kmlLayer("https://raw.githubusercontent.com/Big-Man-Seyi/metrolinkData/main/Metrolink_Lines_Functional.kml"));
            newMap.OperationalLayers.Add(kmlLayer("https://raw.githubusercontent.com/Big-Man-Seyi/metrolinkData/main/Metrolink_Stops_Functional.kml"));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Map? _map;
        public Map? TMSMap
        {
            get => _map;
            set
            {
                _map = value;
                OnPropertyChanged();
            }
        }

        private Map InitializeMap()
        {

            TMSMap = new Map(BasemapStyle.ArcGISDarkGray);
            MapPoint StartingPoint = new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84);
            TMSMap.InitialViewpoint = new Viewpoint(StartingPoint, 100000);

            return TMSMap;
        }

        private static KmlLayer kmlLayer(string url) => new KmlLayer(new KmlDataset(new Uri(url)));

    }

}