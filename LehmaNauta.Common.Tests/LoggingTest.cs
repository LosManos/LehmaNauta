using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UT = Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace LehmaNauta.Common.Tests
{
	[TestClass]
	public class LoggingTest
	{
		[TestMethod]
		public void UT_LoggAll_Given_Call_Should_Output()
		{
			//	#	Arrange.
			const string FileName = "log.txt";
			LehmaNauta.Common.ILogging cut = new LehmaNauta.Common.Logging();
			var fileInfo = new FileInfo(FileName);
			var originalFileSize = fileInfo.Exists ? fileInfo.Length : 0;

			//	#	Act.
			cut.UT_LoggAll();

			//	#	Assert.
			fileInfo = new FileInfo(FileName);	//	We've got to reread the file info as calling methods on FileInfo won't hit the file system again.
			var resultFileSize = fileInfo.Length;
			UT.Assert.IsTrue(originalFileSize < resultFileSize);
		}
	}
}
