/*
 * СТРУКТУРНЫЕ ПАТТЕРНЫ
 * 
 * Глава_12: Адаптер (Adapter)
 * 
 * - преобразует интерфейс одного класса в интерфейс другого, который 
 *   ожидают клиенты. Адаптер делает возможной совместную работу 
 *   классов с несовместимыми интерфейсами
 */

class Motorcycle { }

//Что адаптируем
class Voskhod : Motorcycle 
{
    public void Sound() => Console.WriteLine("DRDRDR");
}

//Цель на которую нужно ориентироваться
//при адаптации
interface Isport
{
    void MakeNoise();
}

//Пример готового объекта
//Адаптированного под цель
class Honda : Motorcycle, Isport
{
    public void MakeNoise() => Console.WriteLine("hooondaaa");
}

//Адаптер
//Адаптирует простой класс под цель
class TuningVoskhod : Isport
{
    Voskhod moto;
    public TuningVoskhod(Voskhod moto) => this.moto = moto;
    public void MakeNoise()
    {
        Console.WriteLine("trsh");
        moto.Sound();
    }
}

class Program
{
    public static void Main(string[] args)
    { 
        var tun = new TuningVoskhod(new Voskhod());
        tun.MakeNoise();
    }
}