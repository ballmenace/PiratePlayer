using System;
using System.Text;

namespace PiratePlayer.Extensions
{
	public static class TimeSpanExtensions
	{
		public static string PrettyFormat(this TimeSpan span)
		{
			if (span.Ticks < 0)
				return string.Empty;

			if (span == TimeSpan.Zero) 
				return "0 minutes";

			if (span.Days > 0)
				return string.Format("{0} day{1} ", span.Days, span.Days > 1 ? "s" : String.Empty);
			if (span.Hours > 0)
				return string.Format("{0} hour{1} ", span.Hours, span.Hours > 1 ? "s" : String.Empty);
			if (span.Minutes > 0)
				return string.Format("{0} minute{1} ", span.Minutes, span.Minutes > 1 ? "s" : String.Empty);

			return "Just now..";
		}
	}
}
