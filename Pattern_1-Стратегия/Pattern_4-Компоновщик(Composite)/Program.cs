/*
 * СТРУКТУРНЫЕ ПАТТЕРНЫ
 * 
 * Глава_14: Компоновщик (Composite)
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

public class Program
{
    public static void Main(string[] argv)
    {
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
    }
}