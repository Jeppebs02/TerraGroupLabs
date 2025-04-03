using Models.Model;
using Models.Model.Potion;
using PotionModels = Models.Model.Potion.Potion;


namespace DataAccess;

public interface IPotionAccess
{
    PotionModels GetPotionById(int id);
    
    List<Potion> GetAllPotions();
    
    bool AddPotion(Potion potion);
    
    bool UpdatePotion(Potion potion);
    
    bool DeletePotion(int id);
}
