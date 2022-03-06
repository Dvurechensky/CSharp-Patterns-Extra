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
//Обозреватели газеты
abstract class Observer
{
    public abstract void Update();
}
//Издатели газеты
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
//Конкретный издатель(издатель газеты) (наблюдаемый)
class ConcreteSubject
    : Subject
{
    public string State { get; set; }
}
//Конкретный обозреватель (наблюдатель)
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
//Обозреватели газеты
abstract class Observer_Push
{
    public abstract void Update(string state); // получает уведомление о новостях
}
//Издатели газеты
abstract class Subject_Push
{
    ArrayList observers = new ArrayList();

    public abstract string State { get; set; }

    public void Attach(Observer_Push observer) //добавить новость в газету
    {
        observers.Add(observer);
    }

    public void Detach(Observer_Push observer) //удалить новость из газеты
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (Observer_Push observer in observers)
            observer.Update(State);
    }
}
//Конкретный издатель(издатель газеты)
class ConcreteSubject_Push
    : Subject_Push
{
    public override string State { get; set; }
}
//Конкретный обозреватель
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

//ПРИМЕР 2

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

    //                  IObserver - тот, кто хочет получать
    //                  уведомления от наблюдающего класса
    public IDisposable Subscribe(IObserver<Event> observer)
    {
        //IDisposable - как правило реализует с целью возможной отписки
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
    //удаление из перечня подписавшихся нашего наблюдаемого класса
    public void Dispose() => user.Subscriptions.Remove(this);
}

//конкретный подписчик
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

    public void OnCompleted()
    {
        //если наблюдение окончено больше ничего
        //происходить не будет
    }

    public void OnError(Exception error)
    {
        //происходит если произошла ошибка,
        //которую нужно обрабатывать
        //и об этом есть явное оповещение
    }

    public void OnNext(Event value)
    {
        //происходит в момент получения данных
        if (value is LoggerEvent) Console.WriteLine("Произошло событие OnNext value is LoggerEvent");
        if (value is AlertEvent) Console.WriteLine("Произошло событие OnNext value is AlertEvent");
    }
}

//логика целой социальной сети (делегаты)
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
        //if (followers != null) followers(this, fn);
        followers?.Invoke(this, fn);
    }
}

class Program
{
    public static void Main(string[] args)
    {
        //Модель вытягивания(Pull model)
        //инициализируем издателя газеты
        ConcreteSubject subject_pull = new ConcreteSubject();
        //публикуем новость для читателя
        subject_pull.Attach(new ConcreteObserver(subject_pull));
        //публикуем новость для читателя
        subject_pull.Attach(new ConcreteObserver(subject_pull));
        subject_pull.State = "Some state ...";
        subject_pull.Notify();
        //Модель проталкивания (Push model)
        ConcreteSubject subject_push = new ConcreteSubject();
        //публикуем новость для читателя
        subject_push.Attach(new ConcreteObserver(subject_push));
        //публикуем новость для читателя
        subject_push.Attach(new ConcreteObserver(subject_push));
        subject_push.State = "Some state ...";
        subject_push.Notify();

        //ПРИМЕР 2
        _ = new Observers();

        //ПРИМЕР на TICTOK
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