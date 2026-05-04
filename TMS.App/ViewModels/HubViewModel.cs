using System.Collections.ObjectModel;
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

public sealed partial class HubViewModel : ObservableObject
{
    private readonly ILogger<HubViewModel> _logger;
    private readonly IMapHubClient _mapHubClient;
    private static readonly ActivitySource _activitySource = new ActivitySource("TMS.App");

    public HubViewModel(ILogger<HubViewModel> logger, IMapHubClient mapHubClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapHubClient = mapHubClient ?? throw new ArgumentNullException(nameof(mapHubClient));
        mapHubClient.MapLinesLoaded += OnMapLinesLoaded;
    }

    public Map Map { get; } = new Map(BasemapStyle.ArcGISDarkGray) { InitialViewpoint = new Viewpoint(new MapPoint(-2.244644, 53.483959, SpatialReferences.Wgs84), 100000)};

    public GraphicsOverlayCollection GraphicsOverlays { get; } = [];

    private void OnMapLinesLoaded(IEnumerable<MapLinesDTO> mapLinesDTO)
    {
        using Activity? activity = _activitySource.StartActivity("HubViewModel.OnMapLinesLoaded");
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            GraphicsOverlay linesOverlay = new GraphicsOverlay();

            IEnumerable<MapPoint> mapPoints = mapLinesDTO.Select(x => new MapPoint(x.Longitude, x.Latitude, SpatialReferences.Wgs84));
            
            Polyline polyline = new Polyline(mapPoints);

            Graphic graphic = new Graphic(polyline)
            {
                Symbol = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.Red, 3)
            };

            linesOverlay.Graphics.Add(graphic);

            GraphicsOverlays.Add(linesOverlay);
        });
    }

    public async Task InitializeAsync() => await _mapHubClient.ConnectAsync();

    public async Task CleanupAsync()
    {
        using Activity? _ = _activitySource.StartActivity("HubViewModel.CleanupAsync");
        _mapHubClient.MapLinesLoaded -= OnMapLinesLoaded;
        await _mapHubClient.DisconnectAsync();
    }   
}