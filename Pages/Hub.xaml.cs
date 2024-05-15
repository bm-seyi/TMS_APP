using Microsoft.Maui.Maps;

namespace TMS_APP.Pages;

public partial class Hub : ContentPage 
{
    public Hub()
    {
        InitializeComponent();
        NativeMap.MapType = MapType.Street;
    }
    
}