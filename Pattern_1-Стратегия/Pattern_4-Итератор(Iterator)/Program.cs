using System.Collections;

/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_4: Итератор
 * 
 * - представляет доступ ко всем элементам составного объекта, не раскрывая его внутреннего представления
 */

/*
 * пример техники "перечисляемый-перечислитель"
 */

interface IEnumerable
{
    IEnumerator GetEnumenator();
}

class Bank : IEnumerable
{
    List<Banknote> bankVault = new List<Banknote>()
    {
        new Banknote(), new Banknote(),
        new Banknote(), new Banknote()
    };


    public Banknote this[int index]
    {
        get
        {
            return bankVault[index];
        }
        set 
        { 
            bankVault.Insert(index, value); 
        }
    }

    public int Count { get { return bankVault.Count; } }  

    public IEnumerator GetEnumenator()
    {
        return new Cashier(this);
    }
}

class Banknote
{
    public string Nominal = "100 $";
}

interface IEnumerator
{
    bool MoveNext();
    void Reset();
    object Current { get; }
}


class Cashier : IEnumerator
{
    private Bank bank;
    private int current = -1;

    public Cashier(Bank bank)
    {
        this.bank = bank;
    }

    public bool MoveNext()
    {
        if(current < bank.Count - 1)
        {
            current++;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        current = -1;
    }
    
    public object Current
    {
        get
        {
            return bank[current];
        }
    }
}



class Program
{
    public static void Main(string[] args)
    {
        IEnumerable bank = new Bank();

        IEnumerator cashier = bank.GetEnumenator();

        while(cashier.MoveNext())
        {
            if(cashier.Current is Banknote banknote)
                Console.WriteLine(banknote.Nominal);
        }
    }
}