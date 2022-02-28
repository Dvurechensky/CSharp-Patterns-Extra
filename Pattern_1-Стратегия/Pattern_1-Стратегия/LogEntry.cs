namespace Pattern_1_Стратегия
{
    public enum Severity
    {
        Debug,
        Warning,
        Fatal
    }

    public struct LogEntry
    {
        public DateTime DateTime { get; set; }
        public Severity Severity { get; set; }
        public string Message { get; set; }
    }
}
