using System.Text.RegularExpressions;

namespace TMS_APP.Utilities;

public class Utils 
{
    public static bool IsValidEmail(string email)
	{
		Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
		return regex.IsMatch(email);
	}
	
}