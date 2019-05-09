using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace IPMonitor
{
	public partial class MainService : ServiceBase
	{
		ServiceWrapper wrapper = new ServiceWrapper();

		public MainService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			wrapper.Start();
		}

		protected override void OnStop()
		{
			wrapper.Stop();
		}
	}
}
