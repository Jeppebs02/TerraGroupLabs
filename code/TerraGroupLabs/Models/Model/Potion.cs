namespace Models.Model;

public class Potion
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }

    public Potion(int id, string name, double price)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
    }
    
    public Potion()
    {
        
    }
}
