using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPUtil;

namespace IPMonitor
{
	public class Config : SerializableObjectBase
	{
		public int httpPort = 42180;
		public int httpsPort = 42181;
		public string dbFile = "db.s3db";
	}
}
