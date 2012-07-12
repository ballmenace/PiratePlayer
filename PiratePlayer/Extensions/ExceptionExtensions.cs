using System.ComponentModel;

namespace PiratePlayer.Extensions
{
	public static class ExceptionExtensions
	{
		public static bool IsCancelled(this Win32Exception ex)
		{
			return ex.ErrorCode == -2147467259;
		}
	}
}