/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_7_1: Команда
 * 
 * - конкретное действие представить в виде конкретного объекта
 */
using System.Collections.ObjectModel;

class Accaunt
{
    public string AccauntOwner { get; set; }
    public int Balance { get; set; }

    public Accaunt(string accauntOwner, int balance)
    {
        Balance = balance;
        AccauntOwner = accauntOwner;
    }

    public void Info()
    {
        Console.WriteLine($"{AccauntOwner}: ${Balance}");
    }
}

/// <summary>
/// Проверить факт выполнения операции
/// IOperation - единственный ответственный за действие
/// </summary>
interface IOperation
{
    void Execute();
    public bool IsComplete { get; }
}

/// <summary>
/// Шаблон команды пополнения кошелька
/// </summary>
class Deposit : IOperation
{
    private readonly Accaunt accaunt;
    private readonly int money;
    private bool isComplete;
    public bool IsComplete { get => isComplete; }
    public Deposit(Accaunt accaunt, int money)
    {
        this.accaunt = accaunt;
        this.money = money;
        isComplete = false;
    }

    /// <summary>
    /// Обработка выполения команды
    /// </summary>
    public void Execute()
    {
        accaunt.Balance += money;
        isComplete = true;
    }
}

/// <summary>
/// Команда снятия с кошелька
/// </summary>
class Withdraw : IOperation
{
    private readonly Accaunt accaunt;
    private readonly int money;
    private bool isComplete;
    public bool IsComplete { get => isComplete; }
    public Withdraw(Accaunt accaunt, int money)
    {
        this.accaunt = accaunt;
        this.money = money;
        isComplete = false;
    }

    /// <summary>
    /// Обработка выполения команды
    /// </summary>
    public void Execute()
    {
        if (accaunt.Balance - money < 0) return;
        accaunt.Balance -= money;
        isComplete = true;
    }
}

/// <summary>
/// коллекция команд
/// </summary>
class Operations : Collection<IOperation> { }

class OperationManager
{
    static public OperationManager Instance;
    static OperationManager() => Instance = new OperationManager();
    private Operations transactions;
    private OperationManager() => transactions = new Operations();

    public void AddOperation(IOperation operation) => transactions.Add(operation);

    public void ProcessOperations()
    {
        transactions.Where(op => !op.IsComplete)
                    .ToList()
                    .ForEach(op => op.Execute());
    }
}

class Program
{
    public static void Main()
    {
        Accaunt accaunt = new Accaunt("nik", 1000);
        accaunt.Info();
        new Deposit(accaunt, 1000).Execute();
        accaunt.Info();

        var manager = OperationManager.Instance;
        manager.AddOperation(new Deposit(accaunt, 1000));
        manager.AddOperation(new Deposit(accaunt, 1000));
        manager.AddOperation(new Withdraw(accaunt, 100));
        accaunt.Info();
        manager.ProcessOperations();
        accaunt.Info();
    }
}