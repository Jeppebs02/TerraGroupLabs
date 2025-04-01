using DataAccess;
using DataAccess.DataBaseLayer;
using Models.Model;

namespace WebAPI.BusinessLogicLayer;

public class PotionLogic
{
    
    private IPotionAccess _potionAccess { get; set; }
    
    public PotionLogic(IConfiguration inConfiguration)
    {
        //We pass the config to the potion access layer
        _potionAccess = new PotionAccess(inConfiguration);
    }
    
    public Potion GetPotionById(int id)
    {
        return _potionAccess.GetPotionById(id);
    }
    
    public List<Potion> GetAllPotions()
    {
        return _potionAccess.GetAllPotions();
    }
    
    public bool AddPotion(Potion potion)
    {
        return _potionAccess.AddPotion(potion);
    }
    
    
    public bool DeletePotion(int id)
    {
        return _potionAccess.DeletePotion(id);
    }
    
    public bool UpdatePotion(Potion potion)
    {
        return _potionAccess.UpdatePotion(potion);
    }
    
    
}