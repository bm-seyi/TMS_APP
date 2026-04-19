namespace TMS.Application.Interfaces.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route);
        Task GoBackAsync();
    }
}