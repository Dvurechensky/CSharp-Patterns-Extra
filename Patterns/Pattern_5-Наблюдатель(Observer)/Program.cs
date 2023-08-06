using System.Collections;
/*
 * ПАТТЕРНЫ ПОВЕДЕНИЯ
 * 
 * Глава_5: Наблюдатель
 * 
 * - определяет зависимость типа «один ко многим»» (один издатель ко многим подписчикам) между объектами 
 * таким образом, что при изменении состояния одного объекта все зависящие от него 
 * оповещаются об этом и автоматически обновляются
 * 
 * -описывает правильные способы организации процесса подписки на определенные события
 */

/*
 * Модель вытягивания (Pull model)
 */
/// <summary>
/// Обозреватели газеты
/// </summary>
abstract class Observer
{
    public abstract void Update();
}

/// <summary>
/// Издатели газеты
/// </summary>
abstract class Subject
{
    ArrayList observers = new ArrayList();

    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }

    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (Observer observer in observers)
            observer.Update();
    }
}

/// <summary>
/// Конкретный издатель(издатель газеты) (наблюдаемый)
/// </summary>
class ConcreteSubject
    : Subject
{
    public string State { get; set; }
}

/// <summary>
/// Конкретный обозреватель (наблюдатель)
/// </summary>
class ConcreteObserver
    : Observer
{
    string observerState;
    ConcreteSubject subject;

    public ConcreteObserver(ConcreteSubject subject)
    {
        this.subject = subject;
    }

    public override void Update()
    {
        observerState = subject.State;
    }
}

/*
 * Модель проталкивания (Push model)
 */
/// <summary>
/// Обозреватели газеты
/// </summary>
abstract class Observer_Push
{
    public abstract void Update(string state);  // получает уведомление о новостях
}

/// <summary>
/// Издатели газеты
/// </summary>
abstract class Subject_Push
{
    ArrayList observers = new ArrayList();

    public abstract string State { get; set; }

    /// <summary>
    /// Добавить новость в газету
    /// </summary>
    /// <param name="observer">обозреватель газеты</param>
    public void Attach(Observer_Push observer)
    {
        observers.Add(observer);
    }

    /// <summary>
    /// Удалить новость из газеты
    /// </summary>
    /// <param name="observer">обозреватель газеты</param>
    public void Detach(Observer_Push observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (Observer_Push observer in observers)
            observer.Update(State);
    }
}

/// <summary>
/// Конкретный издатель(издатель газеты)
/// </summary>
class ConcreteSubject_Push
    : Subject_Push
{
    public override string State { get; set; }
}

/// <summary>
/// Конкретный обозреватель
/// </summary>
class ConcreteObserver_Push
    : Observer_Push
{
    string observerState;
    ConcreteSubject_Push subject;

    public ConcreteObserver_Push(ConcreteSubject_Push subject)
    {
        this.subject = subject;
    }

    public override void Update(string state)
    {
        observerState = subject.State;
    }
}

/// <summary>
/// Пример №2
/// </summary>
class Event { }
class LoggerEvent : Event { }
class AlertEvent : Event { }

/// <summary>
/// Класс, за которым можно наблюдать
/// получая оповещения о происходящих в нем событиях
/// </summary>
class TicTokUser : IObservable<Event>
{
    public List<Subscription> Subscriptions { get; init; }

    public TicTokUser() => Subscriptions = new List<Subscription>();

    public void Alert()
    {
        Random r = new Random();
        foreach (var sub in Subscriptions)
        {
            Event e = r.Next(2) switch
            {
                0 => new LoggerEvent(),
                _ => new AlertEvent()
            };
            sub.observer.OnNext(e);
        }
    }

    /*
        - IObserver - тот, кто хочет получать
        уведомления от наблюдающего класса
        - IDisposable - как правило реализуют 
        с целью возможной отписки
    */
    public IDisposable Subscribe(IObserver<Event> observer)
    {
        var sub = new Subscription(this, observer);
        Subscriptions.Add(sub);
        return sub;
    }
}

class Subscription : IDisposable
{
    private TicTokUser user;
    public IObserver<Event> observer;
    public Subscription(TicTokUser user, IObserver<Event> observer)
    {
        this.user = user;
        this.observer = observer;
    }

    /// <summary>
    /// Удаление наблюдаемого класса из перечня подписавшихся 
    /// </summary>
    public void Dispose() => user.Subscriptions.Remove(this);
}

/// <summary>
/// Конкретный подписчик
/// </summary>
class Observers : IObserver<Event>
{
    public Observers()
    {
        var user = new TicTokUser();
        var sub = user.Subscribe(this);
        user.Alert();
        user.Alert();
        sub.Dispose();
        user.Alert();
    }

    /// <summary>
    /// Наблюдение окончено - больше ничего
    /// происходить не будет
    /// </summary>
    public void OnCompleted() { }

    /// <summary>
    /// Обработка ошибки, явное оповещение
    /// </summary>
    /// <param name="error"></param>
    public void OnError(Exception error) { }

    /// <summary>
    /// Событие при получении данных
    /// </summary>
    /// <param name="value"></param>
    public void OnNext(Event value)
    {
        if (value is LoggerEvent) Console.WriteLine("Произошло событие OnNext value is LoggerEvent");
        if (value is AlertEvent) Console.WriteLine("Произошло событие OnNext value is AlertEvent");
    }
}

/// <summary>
/// Логика социальной сети (делегаты)
/// </summary>
class MediaFile
{
    public MediaFile (string fileName) => FileName = fileName;
    public string FileName { get; set; }
}

class Video : MediaFile
{
    public Video(string fileName) : base(fileName) { }
}

class Accaunt
{
    public string Nick { get; set; }
}

class TicTokUsers : Accaunt
{
    protected event Action<TicTokUsers, string> followers;
    public void Subscribe(TicTokUsers user)
    {
        Console.WriteLine($"{user.Nick} подписался на {Nick}");
        followers += user.Alert;
    }

    public void UnSubscribe(TicTokUsers user)
    {
        Console.WriteLine($"{user.Nick} отдписался на {Nick}");
        followers -= user.Alert;
    }

    public void Alert(TicTokUsers sender, string info)
    {
        if (sender != this) Console.WriteLine($"Лента {Nick}: У {sender.Nick} {info}");
        else Console.WriteLine($"У меня ({Nick}) {info}");
    }

    public void VideoPublishing(MediaFile media)
    {
        var fn = $"вышло видео '{media.FileName}'";
        Alert(this, fn);
        followers?.Invoke(this, fn);
    }
}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Pull model");
        ConcreteSubject subject_pull = new ConcreteSubject();       //инициализируем издателя газеты
        subject_pull.Attach(new ConcreteObserver(subject_pull));    //публикуем новость для читателя
        subject_pull.Attach(new ConcreteObserver(subject_pull));    //публикуем новость для читателя
        subject_pull.State = "Some state ...";
        subject_pull.Notify();
        Console.WriteLine("Push model");
        ConcreteSubject subject_push = new ConcreteSubject();
        subject_push.Attach(new ConcreteObserver(subject_push));    //публикуем новость для читателя
        subject_push.Attach(new ConcreteObserver(subject_push));    //публикуем новость для читателя
        subject_push.State = "Some state ...";
        subject_push.Notify();
        
        _ = new Observers();    //Пример 2 на TicTok
        TicTokUsers user1 = new() { Nick = "user_1" };
        TicTokUsers user2 = new() { Nick = "user_2" };
        user1.VideoPublishing(new MediaFile("Misl 1"));
        user1.Subscribe(user2);
        user1.VideoPublishing(new MediaFile("Misl 2"));
        user1.VideoPublishing(new MediaFile("Misl 3"));
        Console.WriteLine();
        user2.Subscribe(user1);
        user2.VideoPublishing(new MediaFile("History 1"));
        Console.ReadLine();
    }
}