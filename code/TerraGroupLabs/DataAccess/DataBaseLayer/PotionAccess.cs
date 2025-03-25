using Microsoft.Extensions.Configuration;
using Models.Model;

namespace DataAccess.DataBaseLayer;

public class PotionAccess : IPotionAccess
{
    #region Properties

    private IConfiguration _configuration { get; set; }

    #endregion


    #region Constructors

    public PotionAccess(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region Methods
    
    
    public Potion GetPotionById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Potion> GetAllPotions()
    {
        throw new NotImplementedException();
    }

    public void AddPotion(Potion potion)
    {
        throw new NotImplementedException();
    }

    public void UpdatePotion(Potion potion)
    {
        throw new NotImplementedException();
    }

    public void DeletePotion(int id)
    {
        throw new NotImplementedException();
    }
    

    #endregion

}