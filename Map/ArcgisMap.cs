using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Geometry;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace TMS_APP.ArcgisMap
{

    internal class MapViewModel : INotifyPropertyChanged
    {

        public MapViewModel()
        {
               // Create a new map with a 'topographic vector' basemap.
            TMSMap = new Map(BasemapStyle.ArcGISDarkGray);

            var StartingPoint = new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84);
            TMSMap.InitialViewpoint = new Viewpoint(StartingPoint, 100000);

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

    }

}