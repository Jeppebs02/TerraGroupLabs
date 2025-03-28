using Models.Model;

namespace DataAccess;

public interface IPotionAccess
{
    Potion GetPotionById(int id);
    
    List<Potion> GetAllPotions();
    
    bool AddPotion(Potion potion);
    
    bool UpdatePotion(Potion potion);
    
    bool DeletePotion(int id);
}
