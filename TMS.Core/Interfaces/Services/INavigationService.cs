namespace TMS.Core.Interfaces.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route);
        Task GoBackAsync();
    }
}