using Models.Model;

namespace DataAccess;

public interface IPotionAccess
{
    Potion GetPotionById(int id);
    
    List<Potion> GetAllPotions();
    
    void AddPotion(Potion potion);
    
    void UpdatePotion(Potion potion);
    
    void DeletePotion(int id);
}
