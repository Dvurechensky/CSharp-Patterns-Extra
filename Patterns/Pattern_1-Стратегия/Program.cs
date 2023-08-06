namespace Behavioral;

/*
 * На чем строилось:
 * СЕРГЕЙ ТЕПЛЯКОВ - Паттерны проектирования на платформе .Net
 * ШЕВЧУК, АХРИМЕНКО, КАСЬЯНОВ - Приемы объектно-ориентированного проектирования
 * 
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * Паттерн_1: Стратегия
 * 
 * ! является более контекстно зависимой операцией
 * 
 * Причины применения:
 * 1.необходимость инкапсуляции поведения или алгоритма
 * 2.необходимость замены поведения или алгоритма во время исполнения
 * 
 * Другими словами, стратегия обеспечивает точку расширения системы 
 * в определенной плоскости: класс-контекст (LogProcessor) принимает экземпляр стратегии (LogFileReader) 
 * и не знает, какой вариант стратегии он собирается использовать.
 * 
 * Особенность:
 * Передача интерфейса ILogReader классу LogProcessor увеличивает гибкость,
 * но в то же время повышает сложность. 
 * Теперь клиентам класса LogProcessor нужно решить, какую реализацию использовать, 
 * или переложить эту ответственность на вызывающий код.
 */

/*
 * ВМЕСТО
 * классической стратегии на основе наследования
 * можно использовать стратегию на основе делегатов
 * 
 * ПРИМЕР: Стратегия сортировки
 */

/*
 * ВАЖНО: Гибкость не бывает бесплатной, поэтому выделять стратегии стоит тогда, 
 * когда действительно нужна замена поведения во время исполнения.
 */
class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public override string ToString()
    {
        return string.Format($"ID={Id}, Name={Name}");
    }
}

/// <summary>
/// Реализует интерфейс сортировки
/// Добавлет возможность сортировки по ID по возрастающей
/// </summary>
class EmployeeByIdComparer : IComparer<Employee>
{
    int IComparer<Employee>.Compare(Employee? x, Employee? y)
    {
        if(x is Employee xx && y is Employee yy)
            return xx.Id.CompareTo(yy.Id);
        else
            return 0;
    }
}

/// <summary>
/// Фабричный класс для создания экземпляров IComparer
/// </summary>
class ComparerFactory
{
    public static IComparer<T> Create<T>(Comparison<T> comparer)
    {
        return new DelegateComparer<T>(comparer);
    }

    private class DelegateComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparer;

        public DelegateComparer(Comparison<T> comparer)
        {
            _comparer = comparer;
        }

        public int Compare(T? x, T? y)
        {
            if(x == null || y == null)
                return 0;
            return _comparer(x, y);
        }
    }
}

class Program
{
    public static void SortListId(List<Employee> list)
    {
        list.Sort(new EmployeeByIdComparer());  //используем функтор
    }

    public static void SortListName(List<Employee> list)
    {
        list.Sort((x, y) => x.Name.CompareTo(y.Name));  //используем делегат
    }

    static void Main(string[] args)
    {
        var logFileReader = new LogFileReader();

        /*
            создали делегат который принимает в себя метод,
            в результате выполнения которого возвращается List<LogEntry>
        */
        Func<List<LogEntry>> _import = () => logFileReader.Read();

        new LogProcessor(_import).ProcessLogs();

        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 8,
                Name = "asmus"
            },
            new Employee
            {
                Id = 1,
                Name = "robin"
            },
            new Employee
            {
                Id = 2,
                Name = "satan"
            },
            new Employee
            {
                Id = 5,
                Name = "dastin"
            }
        };

        SortListId(employees);      //отсортировали по id через функтор
        SortListName(employees);    //отсортировали по Name через делегат
        Console.WriteLine();

        var comparer = new EmployeeByIdComparer();
        
        var set = new SortedSet<Employee>(comparer);  
        
        /*
            конструктор принимает IComparable
            нет конструктора, принимающего делегат Comparison<T>
            можно создать небольшой адаптерный фабричный класс
        */
        var comparer_factory = ComparerFactory.Create<Employee>((x, y) => x.Id.CompareTo(y.Id));
        var set_factory = new SortedSet<Employee>(comparer_factory);    //он помещает сюда фабрику - ту что умеет создавать
    }
}
