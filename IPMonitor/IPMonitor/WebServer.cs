using System;
using System.Collections.Generic;
using System.IO;
using BPUtil;
using BPUtil.SimpleHttp;
using Newtonsoft.Json;

namespace IPMonitor
{
	public class WebServer : HttpServer
	{
		public WebServer(int httpPort, int httpsPort) : base(httpPort, httpsPort)
		{
		}

		public override void handleGETRequest(HttpProcessor p)
		{
#if DEBUG
			DirectoryInfo WWWDirectory = new DirectoryInfo(Globals.WritableDirectoryBase + "../../www");
#else
				DirectoryInfo WWWDirectory = new DirectoryInfo(Globals.WritableDirectoryBase + "www");
#endif
			string wwwDirectoryBase = WWWDirectory.FullName.Replace('\\', '/').TrimEnd('/') + '/';
			FileInfo fi = new FileInfo(wwwDirectoryBase + p.requestedPage);
			string targetFilePath = fi.FullName.Replace('\\', '/');
			if (!targetFilePath.StartsWith(wwwDirectoryBase) || targetFilePath.Contains("../"))
			{
				p.writeFailure("400 Bad Request");
				return;
			}
			if (!fi.Exists)
			{
				p.writeFailure();
				return;
			}
			string mime = Mime.GetMimeType(fi.Extension);
			if (p.requestedPage.StartsWith(".well-known/acme-challenge/"))
				mime = "text/plain";
			if (fi.LastWriteTimeUtc.ToString("R") == p.GetHeaderValue("if-modified-since"))
			{
				p.writeSuccess(mime, -1, "304 Not Modified");
				return;
			}
			p.writeSuccess(mime, fi.Length, additionalHeaders: GetCacheLastModifiedHeaders(TimeSpan.FromHours(1), fi.LastWriteTimeUtc));
			p.outputStream.Flush();
			using (FileStream fs = fi.OpenRead())
			{
				fs.CopyTo(p.rawOutputStream);
			}
			p.rawOutputStream.Flush();
		}

		private List<KeyValuePair<string, string>> GetCacheLastModifiedHeaders(TimeSpan maxAge, DateTime lastModifiedUTC)
		{
			List<KeyValuePair<string, string>> additionalHeaders = new List<KeyValuePair<string, string>>();
			additionalHeaders.Add(new KeyValuePair<string, string>("Cache-Control", "max-age=" + (long)maxAge.TotalSeconds + ", public"));
			additionalHeaders.Add(new KeyValuePair<string, string>("Last-Modified", lastModifiedUTC.ToString("R")));
			return additionalHeaders;
		}

		public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
		{
			if (p.requestedPage == "api")
			{
				string input = inputData.ReadToEnd();
				ApiRequest req = JsonConvert.DeserializeObject<ApiRequest>(input);
			}
		}

		protected override void stopServer()
		{
		}
	}
}