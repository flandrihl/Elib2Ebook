using NLog.Targets;
using NLog;

namespace Elib2EbookApp
{
    public class EventLogTarget : Target
    {
        public event Action<LogEventInfo> EventReceived;

        public EventLogTarget()
        {
            LogManager
                .Setup()
                .LoadConfiguration(_ => _.ForLogger(LogLevel.Trace).WriteTo(this));
        }

        protected override void Write(LogEventInfo logEvent)
            => EventReceived?.Invoke(logEvent);
    }
}