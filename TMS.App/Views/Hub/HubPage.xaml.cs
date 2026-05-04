using System.Diagnostics;
using Microsoft.Extensions.Logging;
using TMS.App.ViewModels;

namespace TMS.App.Views.Hub;

public partial class HubPage : ContentPage
{
    private readonly ILogger<HubPage> _logger;
    private readonly HubViewModel _hubViewModel;
    private static readonly ActivitySource _activitySource  = new ActivitySource("TMS.App");
    public HubPage(ILogger<HubPage> logger, HubViewModel hubViewModel)
    {
        InitializeComponent();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _hubViewModel = hubViewModel ?? throw new ArgumentNullException(nameof(hubViewModel));
        BindingContext = _hubViewModel;
    }

    protected override async void OnAppearing()
    {
        using Activity? _ = _activitySource.StartActivity("HubPage.OnAppearing");
        base.OnAppearing();
        await _hubViewModel.InitializeAsync();
    }

    protected override async void OnDisappearing()
    {
        using Activity? _ = _activitySource.StartActivity("HubPage.OnDisappearing");
        base.OnDisappearing();
        await _hubViewModel.CleanupAsync();
    }
}


       