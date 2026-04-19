namespace TMS.Application.Interfaces.Services
{
    public interface IAlertService
    {
        Task ShowAlertAsync(string title, string message, string cancel);
    }
}