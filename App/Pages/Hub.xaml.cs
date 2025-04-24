using Microsoft.Extensions.Logging;


namespace TMS_APP.Pages
{
    public partial class Hub : ContentPage
    {
        private readonly ILogger<Hub> _logger;
        public Hub(ILogger<Hub> logger)
        {
            InitializeComponent();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}

       