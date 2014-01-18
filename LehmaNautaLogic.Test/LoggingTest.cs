using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace LehmaNautaLogic.Test
{
	[TestClass]
	public class LoggingTest
	{
		/// <summary>This test doesn't really test anything but running a simple log
		/// so we know it doesn't throw an exception.
		/// </summary>
		[TestMethod]
		public void SimpleLoggingTest()
		{
			LehmaNauta.Common.Helpers.Logging.ExampleEventSource.Log.MyEvent("LNeventSrc.MyEventSroucexx");
		}
	}
}
