using Newtonsoft.Json;

namespace Models.Model;

public class Potion
{
    
    [JsonProperty("id")]
    public int Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("price")]
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
