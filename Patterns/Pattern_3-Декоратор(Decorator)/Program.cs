/*
 * СТРУКТУРНЫЕ ПАТТЕРНЫ
 * 
 * Глава_14: Декоратор (Decorator)
 * 
 * -  динамически добавляет объекту новые обязанности. Является гибкой 
 *    альтернативой порождению подклассов с целью расширения функциональности
 */

/// <summary>
/// Пример №1
/// </summary>
public sealed class Car
{
    public string Model { get; set; }
    public Car(string model, IEnumerable<string> options)
    {

    }
    public List<string> Options { get; protected set; }
    public int GetPrice() { return 0; }
    public void Move() { }
    public void Stop() { }
    public override string ToString()
    {
        return base.ToString();
    }
}

class MyCar
{
    protected Car baseCar;
    public MyCar(string model, IEnumerable<string> options)
    {
        baseCar = new Car(model, options);
    }
    public List<string> Options { get => baseCar.Options; }
    public int GetPrice() => baseCar.GetPrice() + 100;

    public void Move() { baseCar.Move(); }
    public void Stop() { baseCar.Stop(); }

    public override string ToString()
    {
        return $"{baseCar?.ToString()} +++";
    }
}

/// <summary>
/// Пример №2 (Динамический декоратор)
/// </summary>
class Pizza
{
    public virtual string MakePizza() => "Dough =>";
}

class ChickenPizza : Pizza
{
    Pizza pizza;
    public ChickenPizza(Pizza pizza)
    {
        this.pizza = pizza;
    }
    public override string MakePizza()
        => pizza.MakePizza() + "ChickenPizza =>";
}

class MeatPizza : Pizza
{
    Pizza pizza;
    public MeatPizza(Pizza pizza)
    {
        this.pizza = pizza;
    }
    public override string MakePizza()
        => pizza.MakePizza() + "MeatPizza =>";
}

class CheesePizza : Pizza
{
    Pizza pizza;
    public CheesePizza(Pizza pizza)
    {
        this.pizza = pizza;
    }
    public override string MakePizza()
        => pizza.MakePizza() + "CheesePizza =>";
}

/// <summary>
/// Пример №3 (статический декоратор - используем обобщение)
/// </summary>
public abstract class Pizza_Abstr
{
    public virtual string MakePizza() => string.Empty;
}

public class ChickPizza : Pizza_Abstr
{
    public ChickPizza() { }
    public override string MakePizza() => "ChickPizza => ";
}

public class Pepper<T> : Pizza_Abstr
    where T : Pizza_Abstr, new ()
{
    T pizza;
    public Pepper() => pizza = new T();
    public override string MakePizza()
    {
        return $"{pizza.MakePizza()} Pepper =>";
    }
}

public class Olives<T> : Pizza_Abstr
    where T : Pizza_Abstr, new()
{
    T pizza;
    public Olives() => pizza = new T();
    public override string MakePizza()
    {
        return $"{pizza.MakePizza()} Olives =>";
    }
}

class Program
{
    public static void Main(string[] argv)
    {
        #region 2
        Pizza chickenPizza = new ChickenPizza(new Pizza());
        Console.WriteLine(chickenPizza.MakePizza());    // Dough -> ChickenPizza ->
        var chickenCheesePizza = new CheesePizza(new ChickenPizza(new Pizza()));
        Console.WriteLine(chickenCheesePizza.MakePizza());
        #endregion

        #region 3
        Pizza_Abstr pizza_Abstr = new ChickPizza();
        Console.WriteLine(pizza_Abstr.MakePizza());
        Pizza_Abstr chickenOlivesPizza = new Olives<ChickPizza>();
        Console.WriteLine(chickenOlivesPizza.MakePizza());
        #endregion
    }
}