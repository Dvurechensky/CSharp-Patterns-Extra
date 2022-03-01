/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_2: Фабричный метод
 * 
 *  — это каркас, в который наследники могут
 * подставить реализации недостающих элементов.
 * 
 * - позволяет более четко определить «контракт» между базовым классом и потомками
 */

using Pattern_1_Стратегия;
using Pattern_2_Шаблонный_метод;
using System.ServiceModel;
using System.Text;

/*
 * Теперь все реализации читателей логов будут вынуждены следовать согласованному протоколу
 */
public abstract class LogReader
{
    private int _currentPosition;

    //Метод ReadlogEntry невиртуальный: определяет алгоритм импорта
    public IEnumerable<LogEntry> ReadLogEntry()
    {
        return ReadEntries(ref _currentPosition).Select(ParseLogEntry);
    }

    protected abstract IEnumerable<string> ReadEntries(ref int position);

    protected abstract LogEntry ParseLogEntry(string stringEntry);
}

/* 
 * 2.1: Локальный шаблонный метод на основе делегатов
 * 
 * ! является более контекстно зависимой операцией
 * 
 * Использование наследования является слишком тяжеловесным решением, 
 * поэтому в таких случаях применяется подход, при котором переменный шаг алгоритма задается делегатом
 */

//интерфейс сервиса сохранения записей
interface ILogSaver
{
    void UploadLogEntries(IEnumerable<LogEntry>logEntries);
    void UploadExceptions(IEnumerable<ExceptionLogEntry> exceptions);
}

//Прокси - класс инкапсулирует особенности работы
//с WCF - инфраструктурой
class LogSaverProxy : ILogSaver
{
    //подключение к службе 
    class LogSaverClient : ClientBase<ILogSaver>
    {
        public ILogSaver LogSaver
        {
            get { return Channel; }
        }
    }

    public void UploadExceptions(IEnumerable<ExceptionLogEntry> exceptions)
    {
        UseProxyClient(c => c.UploadExceptions(exceptions));
    }

    public void UploadLogEntries(IEnumerable<LogEntry> logEntries)
    {
        UseProxyClient(c => c.UploadLogEntries(logEntries));
    }

    private void UseProxyClient(Action<ILogSaver> accessor)
    {
        var client = new LogSaverClient();
        try
        {
            accessor(client.LogSaver);
            client.Close();
        }
        catch (CommunicationException e)
        {
            client.Abort();
            Console.WriteLine(e.Message);
        }
    }
}
/*
 * Подход на основе делегатов может не только применяться для определения 
 * локальных действий внутри класса, но и передаваться извне другому объекту 
 * в аргументах конструктора.
 */

/*
 * 2.2: Шаблонный метод на основе методов расширения
 */

public abstract class LogEntryBase
{
    public DateTime EntryDateTime { get; internal set; }
    public Severity Severity { get; internal set; }
    public string? Message { get; internal set; }

    //ExceptionLogEntry будет возвращать информацию об исключении
    public string? AdditionalInformation { get; internal set; }
}

public static class LogEntryBaseEx
{
    public static string GetText(this LogEntryBase logEntry)
    {
        var sb = new StringBuilder();

        sb.AppendFormat("{0}", logEntry.EntryDateTime)
            .AppendFormat("{0}", logEntry.Severity)
            .AppendLine(logEntry.Message)
            .AppendLine(logEntry.AdditionalInformation);

        return sb.ToString();
    }
}

class Program
{
    public static void Main(string[] args)
    {

    }
}