namespace Models;

public class Potion
{
    public int id {get;set;}
    public string name {get;set;}
    public double price {get;set;}

    public Potion(int id_,string name_,double price_)
    {
        this.id = id_;
        this.name = name_;
        this.price = price_;
    }
    
    
    
    
}