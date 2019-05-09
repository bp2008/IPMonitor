using System.Reflection;

namespace IPMonitor
{
	public static class IPMonitorGlobals
	{
		public static string Version
		{
			get
			{
				return Assembly.GetEntryAssembly().GetName().Version.ToString();
			}
		}
	}
}