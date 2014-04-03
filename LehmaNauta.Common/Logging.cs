using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LehmaNauta.Common
{
	public class Logging:ILogging
	{
		private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

		public void UT_LoggAll()
		{
			_logger.Trace("Sample trace message");
			_logger.Debug("Sample debug message");
			_logger.Info("Sample informational message");
			_logger.Warn("Sample warning message");
			_logger.Error("Sample error message");
			_logger.Fatal("Sample fatal error message");

			// alternatively you can call the Log() method 
			// and pass log level as the parameter.
			_logger.Log(NLog.LogLevel.Info, "Sample informational message");
		}
	}
}
