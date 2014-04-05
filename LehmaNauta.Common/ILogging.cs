using System.Runtime.CompilerServices;
namespace LehmaNauta.Common
{
	public interface ILogging
	{
		void MethodEnd(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		);

		void MethodEnd(
			object returnValue = null,
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		);

		void MethodStart(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		);

		void UT_LoggAll();
	}
}
