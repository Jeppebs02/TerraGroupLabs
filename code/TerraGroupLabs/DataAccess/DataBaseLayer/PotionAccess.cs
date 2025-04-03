using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using PotionModels = Models.Model.Potion.Potion;
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


    public PotionModels GetPotionById(int id)
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM Potions WHERE id = @id";
            return connection.QuerySingleOrDefault<PotionModels>(query, new { id });
        }
    }

    public List<PotionModels> GetAllPotions()
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "SELECT * FROM Potions";
            return connection.Query<PotionModels>(query).ToList();
        }
    }

    public bool AddPotion(PotionModels potion)
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "INSERT INTO Potions (name, price) VALUES (@Name, @Price)";
            int rowsAffected = connection.Execute(query, potion);
            return rowsAffected > 0;
        }
    }

    public bool UpdatePotion(PotionModels potion)
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "UPDATE Potions SET name = @Name, price = @Price WHERE id = @Id";
            int rowsAffected = connection.Execute(query, potion);
            return rowsAffected > 0;
        }
    }

    public bool DeletePotion(int id)
    {
        string? connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            string query = "DELETE FROM Potions WHERE id = @id";
            int rowsAffected = connection.Execute(query, new { id });
            return rowsAffected > 0;
        }
    }
    

    #endregion

}
