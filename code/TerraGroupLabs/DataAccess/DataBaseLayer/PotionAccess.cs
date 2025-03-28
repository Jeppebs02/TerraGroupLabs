using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Models.Model;
using Dapper;

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
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM Potions WHERE id = @id";
            return connection.QuerySingleOrDefault<Potion>(query, new { id });
        }
    }

    public List<Potion> GetAllPotions()
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM Potions";
            return connection.Query<Potion>(query).ToList();
        }
    }

    public bool AddPotion(Potion potion)
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO Potions (name, price) VALUES (@Name, @Price)";
            int rowsAffected = connection.Execute(query, potion);
            return rowsAffected > 0;
        }
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
