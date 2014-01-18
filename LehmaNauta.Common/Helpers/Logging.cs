using System.Diagnostics.Tracing;
namespace LehmaNauta.Common.Helpers
{
	/// <summary>This is the base class for logging. "class"... rather namespace
	/// masquerading as class.
	/// We use ETW.
	/// A somewhat lacking tutorial:
	/// http://blogs.msdn.com/b/vancem/archive/2012/07/09/logging-your-own-etw-events-in-c-system-diagnostics-tracing-eventsource.aspx
	/// More info on how to name logs here:
	/// http://blogs.msdn.com/b/vancem/archive/2012/08/14/etw-in-c-controlling-which-events-get-logged-in-an-system-diagnostics-tracing-eventsource.aspx
	/// </summary>
	public static class Logging
	{
		/// <summary>This is the base class all ETW event sources should inherit from.
		/// </summary>
		public abstract class LehmaNautaEventSource : EventSource
		{
			protected const string EventSourceName = "LehmaNauta";
			protected enum EventId
			{
				MyEvent = 1
			}
			protected class Keywords
			{
				public const EventKeywords Requests = (EventKeywords)0x0001;
				public const EventKeywords Debug = (EventKeywords)0x0002;
			}
		}

		[EventSource(Name = LehmaNautaEventSource.EventSourceName)]
		public class ExampleEventSource : LehmaNautaEventSource
		{
			public static ExampleEventSource Log = new ExampleEventSource();

			[Event( (int)EventId.MyEvent)]//, Keywords=Keywords.Debug)]
			public void MyEvent(string message)
			{
				WriteEvent( (int)EventId.MyEvent, message);
			}
		}

	}
}
