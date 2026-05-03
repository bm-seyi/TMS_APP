using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Microsoft.Extensions.Logging;
using TMS.Application.Interfaces.HubClients;
using TMS.Domain.DTOs;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace TMS.App.ViewModels;

public sealed class HubViewModel : ObservableObject
{
    private readonly ILogger<HubViewModel> _logger;
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.App");

    public HubViewModel(ILogger<HubViewModel> logger, IMapHubClient mapHubClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Map = new Map(BasemapStyle.ArcGISDarkGray);
        mapHubClient.MapLinesLoaded += OnMapLinesLoaded;
    }

    public Map Map { get; }
    public GraphicsOverlay LinesOverlay { get; } = new();

    private void OnMapLinesLoaded(IEnumerable<MapLinesDTO> mapLinesDTO)
    {
        using Activity? _ = _activitySource.StartActivity("HubViewModel.OnMapLinesLoaded");
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            LinesOverlay.Graphics.Clear();

            IEnumerable<MapPoint> mapPoints = mapLinesDTO.Select(x => new MapPoint(x.Longitude, x.Latitude, SpatialReferences.Wgs84));
            
            Polyline polyline = new Polyline(mapPoints);

            Graphic graphic = new Graphic(polyline)
            {
                Symbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 3)
            };

            LinesOverlay.Graphics.Add(graphic);
        });
    }
}