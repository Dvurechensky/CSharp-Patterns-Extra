/*
 * ПОРОЖДАЮЩИЕ ПАТТЕРНЫ
 * 
 * Глава_8: Сиглтон, Одиночка (Singleton)
 * 
 * - гантирует, что у класса есть только один экземпляр, 
 *   и предоставляет глобальную точку доступа к нему
 */

class Singleton
{
    private Singleton()
    {
        Data = 28;
        MoreData = 90;
    }

    public int Data { get; set; }
    public int MoreData { get; set; }

    //static Singleton uniqueInstance;

    //public static Singleton Instance()
    //{
    //    if (uniqueInstance == null)
    //        uniqueInstance = new Singleton();
    //    return uniqueInstance;
    //}

    //отложенный синглтон (ленивый одиночка)
    static Lazy<Singleton> uniqueInstance = new Lazy<Singleton>(() => new Singleton());
    public static Singleton Instance => uniqueInstance.Value;
}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine(Singleton.Instance.Data);
    }
}