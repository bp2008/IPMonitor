using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BPUtil;
using BPUtil.Forms;

namespace IPMonitor
{
	static class Program
	{
		static ServiceWrapper sw = null;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			Globals.Initialize(exePath);

			if (Environment.UserInteractive)
			{
				string Title = "IPMonitor " + IPMonitorGlobals.Version + " Service Manager";
				string ServiceName = "IPMonitor";
				ButtonDefinition[] buttons = new ButtonDefinition[]
					{ new ButtonDefinition("Start Debug Mode", DebugMode_OnClick)
					};
				if (args.Length == 1 && args[0] == "debug")
					DebugMode_OnClick(null, null);

				try
				{
					Application.Run(new ServiceManager(Title, ServiceName, buttons));
				}
				finally
				{
					if (sw != null)
						sw.Stop();
				}
			}
			else
			{
				ServiceBase[] ServicesToRun;
				ServicesToRun = new ServiceBase[]
				{
					new MainService()
				};
				ServiceBase.Run(ServicesToRun);
			}
		}
		static void DebugMode_OnClick(object sender, EventArgs e)
		{
			sw = new ServiceWrapper();
			sw.Start();
		}
	}
}
