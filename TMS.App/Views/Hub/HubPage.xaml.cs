using TMS.App.ViewModels;

namespace TMS.App.Views.Hub;

public partial class HubPage : ContentPage
{
    public HubPage(HubViewModel hubViewModel)
    {
        InitializeComponent();
        BindingContext = hubViewModel ?? throw new ArgumentNullException(nameof(hubViewModel));
    }
}


       