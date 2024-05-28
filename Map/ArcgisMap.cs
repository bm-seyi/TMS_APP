using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;
using Esri.ArcGISRuntime.Ogc;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Symbology;

namespace TMS_APP.ArcgisMap
{

    internal class MapViewModel : INotifyPropertyChanged
    {

        public MapViewModel()
        {
            tmsMap = InitializeMap();

            string[] lines  =
            [
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/airport_colored.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/altrincham_colored.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/ashton_coloured.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/bury_coloured.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/city_centre_coloured.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/east_didsbury_coloured.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/eccles_coloured.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/oldham_rochdale_coloured.kml",
                "https://github.com/Big-Man-Seyi/metrolinkData/raw/main/MetrolinkMap/Dual%20Line%20Coloured/trafford_centre_coloured.kml"
            ];

            foreach (string line in lines)
            {
                tmsMap.OperationalLayers.Add(AddLayer(line));
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Map? _map;
        public Map? tmsMap
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

            Map map = new Map(BasemapStyle.ArcGISDarkGray)
            {
                InitialViewpoint = new Viewpoint(new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84), 100000)
            };

            return map;
        }

        private static KmlLayer AddLayer(string url) => new KmlLayer(new KmlDataset(new Uri(url)));
    }

}