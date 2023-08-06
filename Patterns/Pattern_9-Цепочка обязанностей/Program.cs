/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_7_3: Цепочка обязанностей
 * 
 * -  позволяет избежать привязки отправителя запроса к его получателю, 
 *    давая шанс обработать запрос нескольким объектам
 *    
 * -  позволяет пробрасывать действия от одной компоненты к другой
 */

class Roshan
{
    public Roshan(int hp = 30) => Hp = hp;
    public int Hp { get; init; }
}

interface IHero
{
    int Hp { get; set; }
    Hero Next { get; set; }
    void MustWin(Roshan enemy);
}

class Hero : IHero
{
    public int Hp { get; set; }
    public Hero Next { get; set; }
    public string Name => GetType().Name;
    public Hero(int hp, Hero hero) { }
    public void MustWin(Roshan enemy)
    {
        if (Hp > enemy.Hp) { Console.WriteLine($"{Name} smog"); }
        else
        {
            if(Next != null)
            {
                Console.WriteLine($"{Name} ne smog. Poprobyet {Next.GetType().Name}");
                Next.MustWin(enemy);
                return;
            }
            Console.WriteLine($"{Name} ne smog. Brag pobedil.");
        }
    }
}

class Ursa : Hero
{
    public Ursa(int hp = 50, Hero hero = null) : base(hp, hero) { }
}

class Timbersaw : Hero
{
    public Timbersaw(int hp = 70, Hero hero = null) : base(hp, hero) { }
}

class Axe : Hero
{
    public Axe(int hp = 110, Hero hero = null) : base(hp, hero) { }
}


/// <summary>
/// Пример №2
/// </summary>
public class Heros
{
    public string Name;
    public int Attack { get; set; }
    public int Armor { get; set; }

    public Heros(string name, int attak, int armor)
    {
        Name = name;
        Attack = attak;
        Armor = armor;
    }

    public override string ToString() => $"{Name}: [{Attack}, {Armor}]\n";
}

public class Effect
{
    protected Heros heros;
    protected Effect next;

    public Effect(Heros heros) => this.heros = heros;
    public void Add(Effect effect)
    {
        if (next != null) next.Add(effect);
        else next = effect;
    }

    /// <summary>
    /// Выполнение действия
    /// </summary>
    public virtual void Handle() => next?.Handle();
}

public class DoubleDamageRune : Effect
{
    public DoubleDamageRune(Heros heros)
        : base(heros) { }

    public override void Handle()
    {
        Console.WriteLine($"{heros.Name} активировал руну двойного урона");
        heros.Attack *= 2;
        base.Handle();
    }
}

public class Halberd : Effect
{
    public Halberd(Heros heros)
        : base(heros) { }

    public override void Handle()
    {
        Console.WriteLine("Существо лишено сил, от чего оно не может атаковать");
        heros.Attack = 0;
    }
}

/// <summary>
/// К какому игровому персонажу какой эффект нужно применить
/// * Самый сложный пример - с брокером событий
/// </summary>
public class Query
{
    public string HeroName { get; set; }
    public GameEffect TypeEffect { get; set; }
    public Characteristic Args { get; set; }

    public Query(string heroName, GameEffect typeEffect, Characteristic values)
    {
        HeroName = heroName;
        TypeEffect = typeEffect;
        Args = values;
    }
}

public enum GameEffect
{
    DubleDamageRunes = 1,
    ArmorBonus = 2,
    HalbertEffects = 3,
    ArcaneRune = 4
}

public struct Characteristic
{
    public int Attack { get; set; }
    public int Armor { get; set; }
}

/// <summary>
/// Брокер который отвечает за эффекты применяемые к персонажу
/// </summary>
public class Game
{
    public event EventHandler<Query> Queries;
    public void CallQuery(object s, Query q) => Queries?.Invoke(s, q);
}

/// <summary>
/// В какой игре к какому персонажу применить какую характеристику
/// </summary>
public class Effects : IDisposable
{
    protected Heross heross;
    protected Game game;
    public Effects(Heross heross, Game game)
    {
        this.heross = heross;
        this.game = game;
        game.Queries += Handlle;
    }

    public virtual void Handlle(object sender, Query e) => Console.WriteLine("Basic Effect");
    public void Dispose() => game.Queries -= Handlle;
}

public class DoubleDamageRunes : Effects
{
    public DoubleDamageRunes(Heross heross, Game game) : base(heross, game) { }
    public override void Handlle(object sender, Query e)
    {
        if(e.HeroName == heross.Name
            && e.TypeEffect == (GameEffect.DubleDamageRunes | GameEffect.HalbertEffects))
        {
            e.Args = new Characteristic
            {
                Armor = e.Args.Armor,
                Attack = 2 * e.Args.Attack
            };
        }
    }
}

public class Halberds : Effects
{
    public Halberds(Heross heross, Game game) : base(heross, game) { }
    public override void Handlle(object sender, Query e)
    {
        if (e.HeroName == heross.Name
            && e.TypeEffect == GameEffect.HalbertEffects)
        {
            e.Args = new Characteristic
            {
                Armor = 1000,
                Attack = 0
            };
        }
    }
}

public class Heross
{
    private readonly Game game;
    public string Name;
    Characteristic characteristic;
    public int Attack 
    {
        get
        {
            Query query = new (Name, GameEffect.DubleDamageRunes | GameEffect.ArmorBonus, characteristic);
            game.CallQuery(this, query);
            return query.Args.Attack;
        }
    }
    public int Armor
    {
        get
        {
            Query query = new (Name, GameEffect.DubleDamageRunes | GameEffect.ArmorBonus, characteristic);
            game.CallQuery(this, query);
            return query.Args.Armor;
        }
    }

    public Heross(string name, int attack, int armor, Game game)
    {
        Console.WriteLine($"Создан герой {name}");
        Name = name;
        characteristic.Attack = attack;
        characteristic.Armor = armor;
        this.game = game;
    }

    public override string ToString() => $"{Name} : [{Attack}, {Armor}]\n";
}


class Program
{
    public static void Main(string[] args)
    {
        var enemy = new Roshan(109);

        var ursa = new Ursa();
        var timber = new Timbersaw();
        var axe = new Axe();

        ursa.Next = timber;
        timber.Next = axe;

        ursa.MustWin(enemy);

        var heros = new Heros("Queen Of Pain", armor: 4, attak: 80);
        Console.WriteLine(heros);

        var game = new Effect(heros);
        game.Add(new DoubleDamageRune(heros)); game.Handle();
        Console.WriteLine(heros);

        game.Add(new Halberd(heros)); game.Handle(); Console.WriteLine(heros);

        var dota = new Game();
        var heross = new Heross("Queen Of Pain", attack: 80, armor: 4, dota);
        Console.WriteLine(heross);
        var dd = new DoubleDamageRunes(heross, dota);
        Console.WriteLine(heross);
        dd.Dispose(); Console.WriteLine(heross);
        var hb = new Halberds(heross, dota);
        Console.WriteLine(heross);
        hb.Dispose(); Console.WriteLine(heross);
    }
}