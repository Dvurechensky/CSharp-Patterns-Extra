/*
 * ПОРОЖДАЮЩИЕ ПАТТЕРНЫ
 * 
 * Глава_9: Абстрактная фабрика (Abstract Factory)
 * 
 * - скрыть сложную логику инициализации
 * - упростить поддержку функционала и его дополнение
 * (крайне рекдо используется)
 */

using BenchmarkDotNet.Attributes;

/// <summary>
/// Фабрика
/// </summary>
interface ICar
{
    void Drive();
}

class RacingCar : ICar
{
    public void Drive() => Console.WriteLine("Ты на гоночном болиде едешь!");
}

class ConcreteCar : ICar
{
    public void Drive() => Console.WriteLine("Ты на бетономешалке едешь!");
}

class UnknownCar : ICar
{
    public void Drive() => Console.WriteLine("Ты на неизвестном едешь!");
}

enum TypeCar
{
    Truck,
    Racing
}

class CarFactory
{
    public static ICar ProductCar(TypeCar type)
    {
        switch (type)
        {
            case TypeCar.Truck: return new RacingCar();
            case TypeCar.Racing: return new ConcreteCar();
            default: return new UnknownCar();
        }
    }
}

[MemoryDiagnoser]
[RankColumn]
public class Benchmark
{
    [Benchmark]
    public void ArrayListBench()
    {
        CarFactory.ProductCar(TypeCar.Truck).Drive();
    }

    [Benchmark]
    public void ListBench()
    {
        var test = CarFactory.ProductCar(TypeCar.Truck);
        test.Drive();
    }
}

/// <summary>
/// Абстрактная фабрика
/// </summary>
interface ICarFactory
{
    ICar ProductCar(TypeCar type);
}

class CarFactory_abstr : ICarFactory
{
    public ICar ProductCar(TypeCar type)
    {
        switch (type)
        {
            case TypeCar.Truck: return new ConcreteCar();
            case TypeCar.Racing: return new RacingCar();
            default: return new UnknownCar();
        }
    }
}

class TuningCarFactory_abstr : ICarFactory
{
    public ICar ProductCar(TypeCar type)
    {
        switch (type)
        {
            case TypeCar.Truck: return new ConcreteCar();
            case TypeCar.Racing: return new RacingCar();
            default: return new UnknownCar();
        }
    }
}

class AbstractFactory
{
    public static ICarFactory GetFactory(bool tuning)
        => tuning ? new TuningCarFactory_abstr() : new CarFactory_abstr();
}


class Program
{
    public static void Main(string[] args)
    {
        //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        CarFactory.ProductCar(TypeCar.Truck).Drive();
        CarFactory.ProductCar(TypeCar.Racing).Drive();
        CarFactory.ProductCar((TypeCar)4).Drive();

        AbstractFactory.GetFactory(true).ProductCar(TypeCar.Truck).Drive();

        var factory = AbstractFactory.GetFactory(false);
        ICar car = factory.ProductCar(TypeCar.Racing);
        car.Drive();
    }
}