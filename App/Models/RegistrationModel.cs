
namespace TMS_APP.Models
{
    public class RegistrationModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RegistrationModels : RegistrationModel
    {
        public string clientId { get; } = "maui_client";
    }
}