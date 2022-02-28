namespace Pattern_1_Стратегия
{
    public class LogFileReader : ILogReader
    {
        public List<LogEntry> Read()
        {
            return new List<LogEntry>() 
            { 
                new LogEntry() 
                { 
                    DateTime = DateTime.Now, 
                    Message = "LogFileReader", 
                    Severity = Severity.Debug
                } 
            };
        }
    }
}
