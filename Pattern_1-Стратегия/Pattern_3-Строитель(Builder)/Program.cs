/*
 * ПОРОЖДАЮЩИЕ ПАТТЕРНЫ
 * 
 * Глава_11: Строитель (Builder)
 * 
 * - строитель отделяет конструирование сложного объекта 
 *   от его представления, так что в результате одного и того же
 *   процесса конструирования могут получаться разные представления.
 */

//Описать модель некоего адреса
class Address
{
    public string Street { get; set; }
    public string House { get; set; }

    public Address(string street, string house)
    {
        Street = street;
        House = house;
    }
}
//представление с точки зрения паттерна
class AddressElement
{
    public string Title { get; set; }
    public string Value { get; set; }
    private List<AddressElement> Elements { get; set; }

    public void AddElement(string title, string value)
    {
        Elements.Add(new AddressElement(title, value));
    }

    public AddressElement(string title, string value)
    {
        Elements = new List<AddressElement>();
        Title = title;
        Value = value;
    }

    private string Print(string indent = "") => "";
    public override string ToString() => "";
}

//конструирует корневую главную ячейку
class AddressBuilder
{
    AddressElement address;

    public static AddressBuilder Build(string title, string value)
        => new AddressBuilder { address = new AddressElement(title, value) };

    private AddressBuilder() { }

    public AddressBuilder AddItem(string title, string value)
    {
        address.AddElement(title, value);
        return this;
    }

    public override string ToString() => address.ToString();
}

//классическое представление паттерна
class Address_Classic
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? House { get; set; }
    public string? Number { get; set; }
    public override string ToString()
    {
        return $"Country: {(string.IsNullOrEmpty(Country) ? "unknown" : Country)}\n" +
               $"City: {(string.IsNullOrEmpty(City) ? "unknown" : City)}\n" +
               $"Street: {(string.IsNullOrEmpty(Street) ? "unknown" : Street)}\n" +
               $"House: {(string.IsNullOrEmpty(House) ? "unknown" : House)}\n" +
               $"Number: {(string.IsNullOrEmpty(Number) ? "unknown" : Number)}\n";
    }
}

interface IAddressBuilder
{
    IAddressBuilder SetCountry(string country);
    IAddressBuilder SetCity(string city);
    IAddressBuilder SetStreet(string street);
    IAddressBuilder SetHouse(string house);
    IAddressBuilder SetNumber(string number);
    Address_Classic Build();
}

class AddressBuilder_Classic : IAddressBuilder
{
    Address_Classic address_Classic;
    public AddressBuilder_Classic() => address_Classic = new Address_Classic();
    public Address_Classic Build() => address_Classic;

    public IAddressBuilder SetCity(string city)
    {
        address_Classic.City = city;
        return this;
    }

    public IAddressBuilder SetCountry(string country)
    {
        address_Classic.Country = country;
        return this;
    }

    public IAddressBuilder SetHouse(string house)
    {
        address_Classic.House = house;
        return this;
    }

    public IAddressBuilder SetNumber(string number)
    {
        address_Classic.Number = number;
        return this;
    }

    public IAddressBuilder SetStreet(string street)
    {
        address_Classic.Street = street;
        return this;
    }
}


class Program
{
    public static void Main(string[] args)
    {
        AddressBuilder addressBuilder =
            AddressBuilder
            .Build("Адрес", "Рабочий адрес")
            .AddItem("Индекс", "214000")
            .AddItem("страна", "РФ");

        Console.WriteLine("");
        
        //Классический конструктор

        Address_Classic address_Classic = new AddressBuilder_Classic()
            .SetCountry("country")
            .SetCity("Moscow")
            .SetHouse("Home")
            .SetNumber("25")
            .Build();

        Console.WriteLine(address_Classic);
        
        address_Classic = new AddressBuilder_Classic()
            .SetCountry("New York")
            .SetStreet("Skobelevskaya")
            .Build();

        Console.WriteLine(address_Classic);
    }
}