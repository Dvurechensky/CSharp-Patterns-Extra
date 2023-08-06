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



class SimpleSet<T>
{
    int index;//какой именно элемент нужно возвращать
    readonly T[] storage;
    public SimpleSet(params T[] args)
    {
        storage = args;
        Reset();
    }

    //свойство возвращающее элеент нашего хранилища
    public T Current => storage[index++];
    //можно ли продолжать обход массива
    public bool MoveNext => index < storage.Length;
    //обнуление значение индекса обхода
    public void Reset() => index = 0;
}

class SimleSetYeild<T> : IEnumerable<T>// IEnumerator<T>,
{
    //int index;
    readonly T[] storage;
    public SimleSetYeild(params T[] args)
    {
        storage = args;
        //Reset();
    }

    //public T Current => storage[index++];

    //object System.Collections.IEnumerator.Current => Current;

    //public void Dispose(){}

    public IEnumerator<T> GetEnumerator()
    {
        var len = storage.Length;
        for (int i = 0; i < len; i++)
            yield return storage[i]; //позволяет вернуть значение и перейти на следующую итерацию цикла
    }

    //public bool MoveNext() => index < storage.Length;
    //public void Reset() => index = 0;

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();
}

class Dish : IEnumerable<int>
{
    public Dish(string Name, int Weight, int Proteins = 0, int Fats = 0, int Carbohydrates = 0)
    {
        this.Name = Name;
        this.Weight = Weight;
        //this.Proteins = Proteins;
        //this.Fats = Fats;
        nutrients = new Dictionary<string, int>
        {
            ["Proteins"] = Proteins,
            ["Fats"] = Fats,
            ["Carbohydrates"] = Carbohydrates
        };
    }

    //допы
    readonly Dictionary<string, int> nutrients;
    public int Proteins => nutrients[nameof(Proteins)];
    public int Fats => nutrients[nameof(Fats)];
    public int Carbohydrates => nutrients[nameof(Carbohydrates)];

    public double AverageNutrients => nutrients.Values.Average();

    //init - только в конструкторе может изменять значения свойства
    public string Name { get; init; } 
    public int Weight { get; init; }
    //public int Proteins { get; set; }
    //public int Fats { get; set; }
    //public int Average => (Proteins + Fats) / 2;
    public override string ToString()
    {
        return $"{nameof(Name)} : {Name} {nameof(Weight)} : {Weight}";
    }


    public IEnumerator<int> GetEnumerator() => nutrients.Values.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

class Menu : IEnumerable<Dish>
{
    public Dish Pizza { get; init; }
    public Dish Coffee { get; init; }
    public Dish Soup { get; init; }

    public IEnumerator<Dish> GetEnumerator()
    {
        //В каком порядке перечислять
        yield return Pizza; 
        yield return Coffee;
        yield return Soup;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => (System.Collections.IEnumerator)this;
}

public class Node<T>
{
    public T Value { get; set; }
    public Node<T> Left { get; set; }
    public Node<T> Right { get; set; }
    public Node<T> Parent { get; set; }
    public Node (T value) => Value = value; 
    public Node (T value, Node<T> left = null, Node<T> right = null) 
        : this(value)
    {
        Left = left;
        Right = right;
        if(Left != null) Left.Parent = this;
        if(Right != null) Right.Parent = this;
    }

    public override string ToString() => Value.ToString();
}

//обход самого дерева
public class InOrderIterator<T>
{
    public Node<T> Current { get; set; }
    private readonly Node<T> root;
    private bool detourStarted;
    //принимает корневой элемент с которого начнёт обход
    public InOrderIterator(Node<T> root) 
    {
        this.root = Current = root;
        Reset();
    }

    public void Reset()
    {
        Current = root;
        while(Current.Left != null) Current = Current.Left;
        detourStarted = !true;
    }

    public bool MoveNext()
    {
        if (!detourStarted)
        {
            detourStarted = true;
            return true;
        }

        if (Current.Right != null)
        {
            Current = Current.Right;
            while (Current.Left != null) Current = Current.Left;
            return true;
        }
        else
        {
            var temp = Current.Parent;
            while(temp != null && Current == temp.Right)
            {
                Current = temp;
                temp = temp.Parent;
            }
            Current = temp;
            return Current != null;
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

        //стандартное реализация паттерна Итератор
        var array =  Enumerable.Range(1, 10).ToArray();
        foreach (var item in array)
            Console.WriteLine($"array: {item} ");

        SimpleSet<int> set = new (array);
        set.Reset();
        while(set.MoveNext)
            Console.WriteLine($"set: {set.Current}");

        //Вторая реализация паттерна
        SimleSetYeild<int> setyeild = new(array);
        foreach (var item in setyeild) Console.WriteLine($"setyeild: {item}");

        Menu menu = new()
        {
            Coffee = new("Latte", 200, 111, 222, 333),
            Soup = new("Tom ", 230),
            Pizza = new("Margarita", 600),
        };

        //foreach (var dish in menu) Console.WriteLine(dish);
        foreach (var dish in menu)
        {
            Console.WriteLine($"{dish} - Average: {dish.AverageNutrients} [ ");
            foreach (var st in dish)
            {
                Console.WriteLine($"{nameof(st)} {st}");
            }
            Console.WriteLine("] ");
        }

        //пример обхода деревьев (паттерн)
        var three = new Node<string>(value: "html",
        left: new(value: "head",
                    left: new(value: "title"),
                    right: new(value: "meta")),
        right: new(value: "body",
                    left: new(value: "h1",
                                left: new(value: "a")),
                                right: new(value: "ul",
                                            left: new(value: "li"),
                                            right: new(value: "li")))
        );

        var rlr = new InOrderIterator<string>(three);
        while (rlr.MoveNext()) Console.WriteLine($"{rlr.Current}");
        Console.WriteLine();
        rlr.Reset();
        while (rlr.MoveNext()) Console.WriteLine($"{rlr.Current}");
    }
}

