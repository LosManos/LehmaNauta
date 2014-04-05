using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LehmaNauta.Common
{
	public class Logging:ILogging
	{
		private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

		public void MethodEnd(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		)
		{
			_logger.Trace(FormatCallerData(memberName, sourceFilePath, sourceLineNumber));
		}

		public void MethodEnd(
			object returnValue = null, 
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		)
		{
			_logger.Trace(
				string.Format("{0}\rReturn value:{1}",
				FormatCallerData(memberName, sourceFilePath, sourceLineNumber),
				null == returnValue ? "<null>" : returnValue)
			);
		}

		public void MethodStart(
	        [CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		)
		{
			_logger.Trace( FormatCallerData(memberName, sourceFilePath, sourceLineNumber ));
		}

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

		/// <summary>This method returns the caller data, 
		/// like name of method and source code file in a common format.
		/// <para>
		/// Note \r as return and not \r\n as usual. I don't know why but in this/my case
		/// \r\n will return two new rows. \n works as \r.
		/// </para>
		/// </summary>
		/// <param name="memberName"></param>
		/// <param name="sourceFilePath"></param>
		/// <param name="sourceLineNumber"></param>
		/// <returns></returns>
		private static string FormatCallerData(
			string memberName, 
			string sourceFilePath, 
			int sourceLineNumber)
		{
			return string.Format(
				"Method:{0}\rPathfile:{1}\rLineno:{2}",
				memberName,
				sourceFilePath,
				sourceLineNumber);
		}
	}
}
