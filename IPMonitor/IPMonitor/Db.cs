using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPMonitor
{
	public static class Db
	{
		private static object syncLock = new object();
		public static void AddRow()
		{
			using (SQLiteConnection conn = new SQLiteConnection(ServiceWrapper.cfg.dbFile))
			{
				conn.Devices.Add(new Device());
				conn.SaveChanges();
			}
		}
	}

	public class SQLiteConnection : DbContext
	{
		public SQLiteConnection(string connString) : base(connString)
		{
		}
		public DbSet<Device> Devices { get; set; }
	}

	public class Device
	{
		public Device()
		{
		}
		/// <summary>
		/// Unique ID (primary key) of the device.
		/// </summary>
		[Key]
		public int DeviceID;
		/// <summary>
		/// The IP address of the device.
		/// </summary>
		public IPAddress Address;
		/// <summary>
		/// MAC address for devices where this is obtainable, otherwise null.
		/// </summary>
		public string MAC;
		/// <summary>
		/// Vendor name which can be found by querying online services for the MAC address.
		/// </summary>
		public string Vendor;
		/// <summary>
		/// The name of the device.
		/// </summary>
		public string Name;
	}
}
