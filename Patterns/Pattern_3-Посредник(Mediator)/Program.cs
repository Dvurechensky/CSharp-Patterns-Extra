/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_3: Фабричный метод(виртуальный конструктор)
 * 
 * - определяет объект, инкапсулирующий способ взаимодействия множества объектов.
 * - это клей, связывающий несколько независимых классов между собой. 
 *   Он избавляет классы от необходимости ссылаться друг на друга, 
 *   позволяя тем самым их независимо изменять и анализировать
 */

/// <summary>
/// Посредники
/// Предоставляет интерфейс для организации 
/// процесса по обмену информацией между объектами типа Colleague.
/// </summary>
abstract class Mediator
{
    public abstract void Send(string message, Colleague colleague);
}

/// <summary>
/// Конкретный посредник
/// Реализует алгоритм взаимодействия между объектами-коллегами
/// </summary>
class ConcreteMediator : Mediator
{
    public Fermer Fermer { get; set; }
    public Cannery Cannery { get; set; }
    public Shop Shop { get; set; }

    public override void Send(string message, Colleague colleague)
    {
        if(colleague == Fermer)
        {
            Cannery.MakeKetchup(message);
        }
        if(colleague == Cannery)
        {
            Shop.SellKetshup(message);
        }
    }
}

/// <summary>
/// Коллеги
/// Предоставляет интерфейс для организации процесса 
/// взаимодействия объектов-коллег с объектом типа Mediator.
/// </summary>
abstract class Colleague
{
    protected Mediator mediator;

    public Colleague(Mediator mediator)
    {
        this.mediator = mediator; //передаем ссылку на абстрактный класс посредника
    }
}

/// <summary>
/// Фермер
/// Каждый объект-коллега знает только об объекте-медиаторе.
/// Все объекты-коллеги обмениваются информацией
/// только через посредника (медиатора).
/// </summary>
class Fermer : Colleague
{
    public Fermer(Mediator mediator)
        : base(mediator) { }

    public void GrowTomato()
    {
        string tomato = "Tomato";
        Console.WriteLine($"{this.GetType().Name} raised {tomato}");
        mediator.Send(tomato, this);
    }
}

/// <summary>
/// Завод
/// </summary>
class Cannery : Colleague
{
    public Cannery(Mediator mediator) 
        : base(mediator) { }

    public void MakeKetchup(string message)
    {
        string ketchup = message + " Ketchup";
        Console.WriteLine($"{this.GetType().Name} produced {ketchup}");
        mediator.Send(ketchup, this);
    }
}

/// <summary>
/// Магазин
/// </summary>
class Shop : Colleague
{
    public Shop(Mediator mediator) 
        : base(mediator) { }
    public void SellKetshup(string message)
    {
        Console.WriteLine($"{this.GetType().Name} sold {message}");
    }
}

class Program
{
    public static void Main(string[] args)
    {
        ConcreteMediator mediator = new ConcreteMediator();
        var cannery = new Cannery(mediator);
        var fermer = new Fermer(mediator);
        var shop = new Shop(mediator);

        mediator.Cannery = cannery;
        mediator.Fermer = fermer;
        mediator.Shop = shop;

        fermer.GrowTomato();
    }
}