/*
 * СТРУКТУРНЫЕ ПАТТЕРНЫ
 * 
 * Глава_13: Фасад (Facade)
 * 
 * - предоставляет унифицированный интерфейс вместо набора 
 *   интерфейсов некоторой подсистемы. Фасад определяет 
 *   интерфейс более высокого уровня, который упрощает использование подсистемы
 */

class NICPc
{
    public void C0()
        => Console.WriteLine("осуществляется программирование регистров микросхемы Host Bridge");
    public void C1()
        => Console.WriteLine("с помощью последовательных циклов запись/чтение определяется тип памяти");
    public void C2()
        => Console.WriteLine("проверяются первые 256 Кб памяти, для использования как транзитный буфер");
    public void C6()
        => Console.WriteLine("по спец. алгоритму определяется наличие, тип и параметры External Cache.");
    public void CF()
        => Console.WriteLine("определяется тип процессора, а результат помещается в CMOS");
    public void Step05()
        => Console.WriteLine("осуществляется проверка и инициализация контроллера клавиатуры");
    public void Step07()
        => Console.WriteLine("проверяется функционирование CMOS и напряжение питания батареи");
    public void StepBE()
        => Console.WriteLine("программируются конфигурационные регистры Host Bridge и PIIX значениями, взятыми из BIOS");
    public void Step0A()
        => Console.WriteLine("генерируется таблица векторов прерываний, а также первичная настройка подсистемы управления");
    public void Step0B()
        => Console.WriteLine("проверяется контрольная сумма блока ячеек BIOS");
    public void Step0C()
        => Console.WriteLine("инициализируется блок переменных BIOS");
    public void Step0D0E()
        => Console.WriteLine("определяется наличие видеоадаптера путём проверки наличия сигнатуры 55AA");
    public void Step3031()
        => Console.WriteLine("определяется объём Base Memory и External Memory, вступительный экран");
    public void Step3D()
        => Console.WriteLine("инициализируется PS/2 mouse.");
    public void Step41()
        => Console.WriteLine("производится инициализация подсистемы гибких дисков.");
    public void Step45()
        => Console.WriteLine("инициализируется сопроцессор FPU");
    public void StepF()
        => Console.WriteLine("Приветствие");
}

class Facade
{
    NICPc pc;

    public Facade(NICPc pc)
    {
        this.pc = pc;
    }

    public void Power()
    {
        pc.C0();
        pc.C1();
        pc.C2();
        pc.C6();
        pc.CF();
        pc.Step05();
        pc.Step07();
        pc.StepBE();
        pc.Step0A();
        pc.Step0B();
        pc.Step0C();
        pc.Step0D0E();
        pc.Step3031();
        pc.Step3D();
        pc.Step41();
        pc.Step45();
        pc.StepF();
    }
}



class Program
{
    public static void Main(string[] args)
    {
        new Facade(new NICPc()).Power();
    }
}