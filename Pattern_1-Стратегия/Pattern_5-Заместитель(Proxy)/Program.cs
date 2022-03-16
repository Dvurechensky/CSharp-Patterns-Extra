/*
 * СТРУКТУРНЫЕ ПАТТЕРНЫ
 * 
 * Глава_16: Заместитель (Proxy)
 * 
 * -  является суррогатом другого объекта и контролирует доступ к нему.
 */

//абстрактная сущность которая может бегать
public abstract class ActionObject
{
    public abstract void Run();
}

public class Human : ActionObject
{
    public int Age { get; set; }
    public Human ()
    {
        Age = new Random().Next(14, 70);
    }
    public override void Run() => Console.WriteLine("RUN!");
    public Human Clone()
    {
        Human temp = new Human() { Age = this.Age };
        return temp;
    }
}

public class AvatarV1 : ActionObject
{
    Human human;
    public AvatarV1(Human human)
    {
        this.human = human.Clone();
        //или
        //this.human = human;
        //если есть(нужен) доступ к исходному объекту
    }

    public int AvatarAge => human.Age;
    public override void Run()
    {
        Console.WriteLine("AvatarV1 Run");
        human.Run();
    }
}

//proxy сервер
class Client
{
    private string id;
    public string Id { get => id; set => id = value; }
    public Client(string id = "#2022") => this.id = id;
}

interface IServer
{
    void AccessGranted(Client user);
    void AccessClosed(Client user);
}

class Server : IServer
{
    public Server() => Console.WriteLine("Сервер создан");
    public void AccessClosed(Client user)
    {
        Console.WriteLine("Closed");
    }

    public void AccessGranted(Client user)
    {
        Console.WriteLine("Granted");
    }
}

class ServerProxy : IServer
{
    private Lazy<Server> server;
    public ServerProxy() { }

    public void AccessClosed(Client client)
    {
        if (server == null)
        {
            Console.WriteLine("Unknown user");
        }
        else
        {
            server.Value.AccessGranted(user: client);
        }
    }

    public void Autentification(Client client)
    {
        if (client.Id != "#2022") return;
        Console.WriteLine("OK");
        server = new();
        AccessGranted(client);
    }

    public void AccessGranted(Client client)
    {
        if(server == null)
        {
            Console.WriteLine("Access Close");
            return;
        }    
        server.Value.AccessClosed(user: client);
    }
}

class Program
{
    public static void Main(string[] argv)
    {
        Human human = new Human();
        human.Run();
        AvatarV1 avatar = new AvatarV1(human);
        avatar.Run();
        //proxy прослойка между сервером 
        ServerProxy proxy = new();
        proxy.Autentification(new Client() { });
        proxy.AccessGranted(new Client() { });
        proxy.AccessClosed(new Client() { });
    }
}