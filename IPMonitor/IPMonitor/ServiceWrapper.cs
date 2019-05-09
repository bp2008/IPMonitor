using System;
using BPUtil;

namespace IPMonitor
{
	public class ServiceWrapper
	{
		public static Config cfg;

		static ServiceWrapper()
		{
			cfg = new Config();
			cfg.Load();
			cfg.SaveIfNoExist();
		}

		WebServer server;

		public void Start()
		{
			server = new WebServer(cfg.httpPort, cfg.httpsPort);
			server.Start();
		}

		public void Stop()
		{
			Try.Catch(() => { server?.Stop(); server = null; });
		}
	}
}