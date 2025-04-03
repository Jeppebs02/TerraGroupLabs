using DataAccess;
using DataAccess.DataBaseLayer;
using PotionModels = Models.Model.Potion.Potion;

namespace WebAPI.BusinessLogicLayer;

public class PotionLogic
{
    
    private IPotionAccess _potionAccess { get; set; }
    
    public PotionLogic(IConfiguration inConfiguration)
    {
        //We pass the config to the potion access layer
        _potionAccess = new PotionAccess(inConfiguration);
    }
    
    public PotionModels GetPotionById(int id)
    {
        return _potionAccess.GetPotionById(id);
    }
    
    public List<PotionModels> GetAllPotions()
    {
        return _potionAccess.GetAllPotions();
    }
    
    public bool AddPotion(PotionModels potion)
    {
        return _potionAccess.AddPotion(potion);
    }
    
    
    public bool DeletePotion(int id)
    {
        return _potionAccess.DeletePotion(id);
    }
    
    public bool UpdatePotion(PotionModels potion)
    {
        return _potionAccess.UpdatePotion(potion);
    }
    
    
}