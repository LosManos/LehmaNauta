using System.Runtime.CompilerServices;
namespace LehmaNauta.Common
{
	public interface ILogging
	{
		/// <summary>This method logs that a method is ending.
		/// It should be called right before returning from a method that returns void.
		/// <para>
		/// Typically called like:
		///		logging.MethodEnd();
		///	</para>
		/// </summary>
		/// <param name="memberName"></param>
		/// <param name="sourceFilePath"></param>
		/// <param name="sourceLineNumber"></param>
		void MethodEnd(
			[CallerMemberName] string memberName = "",
			[CallerFilePath] string sourceFilePath = "",
			[CallerLineNumber] int sourceLineNumber = 0
		);

		/// <summary>This method logs that a method is ending with a value.
		/// It should be called right before returning from a method that returns something.
		/// <para>
		/// It is called ...WithReturnValue instead of just overloading MethodEnd because
		/// when calling MethodEnd without parameters the compiler don't know which method to call.
		/// </para>
		/// <para>
		/// Typically called like:
		///		logging.MethodEnd(returnValue);
		///	</para>
		/// </summary>
		/// <param name="returnValue"></param>
		/// <param name="memberName"></param>
		/// <param name="sourceFilePath"></param>
		/// <param name="sourceLineNumber"></param>
		void MethodEndWithReturnValue(
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
