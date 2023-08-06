/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_6: Посетитель
 * 
 * - добавление поведения в иерархию объектов, не изменяя их классы
 */

/*
 * При добавлении нового поведения в IAnimal нарушаются SOLID принципы, 
 * для этого и существует подход Visitor чтобы внедрять поведение в конкретные классы наследники
 */
interface IAnimal
{
    void Move();
}

class Cat : IAnimal
{
    public void Move() => Console.WriteLine("Kradetsy besshumno");
}

class Dog : IAnimal
{
    public void Move() => Console.WriteLine("beshit");
}

class Bird : IAnimal
{
    public void Move() => Console.WriteLine("fly");
}

interface IVisitor
{
    void Make(Cats cat);
    void Make(Dogs cat);
    void Make(Birds cat);
}

/// <summary>
/// Реализация Visitor №1
/// </summary>
abstract class Animals
{
    public abstract void Accept(IVisitor visitor);
}

class Cats : Animals
{
    public override void Accept(IVisitor visitor) => visitor.Make(this); 
}

class Dogs : Animals
{
    public override void Accept(IVisitor visitor) => visitor.Make(this);
}

class Birds : Animals
{
    public override void Accept(IVisitor visitor) => visitor.Make(this);
}

class Kiwi : Birds
{
    public override void Accept(IVisitor visitor) => visitor.Make(this);
}

class VoiceVisitor : IVisitor
{
    public virtual void Make(Dogs dogs) => Console.WriteLine("Gav");
    public virtual void Make(Cats cat) => Console.WriteLine("My");
    public virtual void Make(Birds cat) => Console.WriteLine("Chirik");
}

/// <summary>
/// Visitor для птички Киви
/// </summary>
class UpdateVoiceVisitor : VoiceVisitor
{
    public override void Make(Birds bird)
    {
        if (bird is Kiwi) Console.WriteLine("Киви что-то там...");
        else base.Make(bird);
    }
}

class MoveVisitor : IVisitor
{
    public void Make(Cats cat) => Console.WriteLine("Kradet");
    public void Make(Dogs cat) => Console.WriteLine("Beg");
    public void Make(Birds cat) => Console.WriteLine("Fly");
}

class Program
{
    public static void Main(string[] args)
    {
        Dogs dogs = new Dogs();
        Cats cats = new Cats();
        Birds birds = new Birds();
        
        var voice = new VoiceVisitor();
        var move = new MoveVisitor();

        dogs.Accept(voice);
        cats.Accept(voice);
        birds.Accept(voice);

        dogs.Accept(move);
        cats.Accept(move);
        birds.Accept(move);

        var updateVoiceVisitor = new UpdateVoiceVisitor();
        var kiwi = new Kiwi();

        kiwi.Accept(updateVoiceVisitor);
        dogs.Accept(updateVoiceVisitor);
        cats.Accept(updateVoiceVisitor);
        birds.Accept(updateVoiceVisitor);
    }
}