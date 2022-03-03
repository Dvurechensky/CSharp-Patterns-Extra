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

//Посредники
//Предоставляет интерфейс для организации процесса по обмену информацией между объектами типа Colleague.
abstract class Mediator
{
    public abstract void Send(string message, Colleague colleague);
}

//Конкретный посредник
//Реализует алгоритм взаимодействия между объектами-коллегами
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

//Коллеги
//Предоставляет интерфейс для организации процесса взаимодействия объектов-коллег с объектом типа Mediator.
abstract class Colleague
{
    protected Mediator mediator;

    //конструктор класса принимает в себя объект коллеги
    public Colleague(Mediator mediator)
    {
        //мы передаем ссылку на абстрактный класс посредника не явно в конструктор всех Коллег
        this.mediator = mediator;
    }
}

//Фермер
//Каждый объект-коллега знает только об объекте-медиаторе. Все объекты-коллеги
//обмениваются информацией только через посредника (медиатора).
class Fermer : Colleague
{
    public Fermer(Mediator mediator) 
        : base(mediator)
    {
    }

    public void GrowTomato()
    {
        string tomato = "Tomato";
        Console.WriteLine($"{this.GetType().Name} raised {tomato}");
        mediator.Send(tomato, this);
    }
}

//Завод
class Cannery : Colleague
{
    public Cannery(Mediator mediator) 
        : base(mediator)
    {
    }

    public void MakeKetchup(string message)
    {
        string ketchup = message + " Ketchup";
        Console.WriteLine($"{this.GetType().Name} produced {ketchup}");
        mediator.Send(ketchup, this);
    }
}

//Магазин
class Shop : Colleague
{
    public Shop(Mediator mediator) 
        : base(mediator)
    {
    }

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