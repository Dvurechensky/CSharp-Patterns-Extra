/*
 * СТРУКТУРНЫЕ ПАТТЕРНЫ
 * 
 * Глава_15: Компоновщик (Composite)
 * 
 * -  компонует объекты в древовидные структуры для представления 
 *    иерархий «часть — целое». Позволяет клиентам единообразно 
 *    трактовать индивидуальные и составные объекты
 */

class UIElement
{
    public UIElement(string name) 
    {
        Children = new List<UIElement>();
    }
    public List<UIElement> Children { get; set; }
    public string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    protected virtual string Draw(string p = "") { return p; }
    public override string ToString() => $"{Draw()}";
}

class TextBox : UIElement
{
    public TextBox(string name = "textBox") : base(name) { }
}

class Label : UIElement
{
    public Label(string name = "label") : base(name) { }
}

class Panel : UIElement
{
    public Panel(string name = "panel") : base(name) { }
}

class TextBlock : UIElement
{
    public TextBlock(string name = "textBlock") : base(name) { }
}

/// <summary>
/// Пример отправки писем группе адресатов
/// </summary>
interface IEMail
{
    void Send();
    string Name { get; set; }
}

class Group : IEMail
{
    public Group(params IEMail[] es) => Append(es);

    public List<IEMail> eMails = new();

    public string Name { get; set; }

    public void Append(params IEMail[] es) {
        foreach (var item in es) eMails.Add(item);
    }

    public void Send() {
        foreach(var item in eMails) item.Send();
    }
}

class EMail : IEMail
{
    public string Name { get; set; }

    public void Send() => Console.WriteLine($"Send {Name}");
}

/// <summary>
/// Генератор файловой системы
/// </summary>
abstract class IFileSystem
{
    protected virtual IFileSystem AddItem(IFileSystem element) => this;
    public abstract void PrintInfo(string w = "");
    public string Title { get; set; }
}

class Document : IFileSystem
{
    public override void PrintInfo(string w = "")
    {
        Console.WriteLine($"{w}{Title}");
    }
}

class Folder : IFileSystem
{
    private readonly List<IFileSystem> fileSystem;
    public Folder() => fileSystem = new();

    public IFileSystem AddElement(params IFileSystem[] element)
    {
        foreach (var item in element) AddItem(item);
        return this;
    }

    public override void PrintInfo(string w = "")
    {
        Console.WriteLine($"{w}{Title}");
        foreach (var item in fileSystem) item.PrintInfo(w);
    }

    protected override IFileSystem AddItem(IFileSystem element)
    {
        fileSystem.Add(element);
        return this;
    }
}


public class Program
{
    public static void Main(string[] argv)
    {
        #region 1
        var panel1 = new Panel("panel1");
        var tb1 = new TextBox("textBox1");
        var tb2 = new TextBox("textBox2");
        panel1.Children.Add(tb1);
        var panel2 = new Panel("panel2");
        var tb3 = new TextBox("textBox3");
        var lbl1 = new Label("label1");
        lbl1.Children.Add(new TextBlock("textBlock1"));
        panel2.Children.Add(tb3);
        panel2.Children.Add(lbl1);
        panel1.Children.Add(panel2);
        panel1.Children.Add(tb2);

        Console.WriteLine(panel1);
        #endregion
        #region 2
        EMail c = new EMail() { Name = "nik" };
        c.Send();
        EMail r = new EMail() { Name = "nikr" };
        r.Send();
        EMail u = new EMail() { Name = "niku" };
        u.Send();
        EMail d = new EMail() { Name = "nikd" };
        d.Send();

        Group cr = new Group(c, r, u);
        cr.Send();
        
        //3
        new Folder() { Title = "C:/"}
            .AddElement(new Folder() { Title = "Windows/"}
                .AddElement(new Folder() { Title = "System32/" }
                    .AddElement(new Document() { Title = "system.xml"}),
                    new Folder() { Title = "Driver/"}
                        .AddElement(new Folder() { Title = "etc/"}
                            .AddElement(new Document() { Title = "jp.json"}),
                            new Folder() { Title = "etc2"})))
            .PrintInfo();
        #endregion
    }
}