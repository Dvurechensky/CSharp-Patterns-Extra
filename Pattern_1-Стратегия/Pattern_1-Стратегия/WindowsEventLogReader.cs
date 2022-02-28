namespace Pattern_1_Стратегия
{
    public class WindowsEventLogReader : ILogReader
    {
        public List<LogEntry> Read()
        {
            return new List<LogEntry>()
            {
                new LogEntry()
                {
                    DateTime = DateTime.Now,
                    Message = "WindowsEventLogReader",
                    Severity = Severity.Debug
                }
            };
        }
    }
}
