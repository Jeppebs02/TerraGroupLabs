using SpacetimeDB;

public static partial class Module
{

    #region Tables

    [SpacetimeDB.Table(Name = "user", Public = true)]
    public partial class Person
    {
        [SpacetimeDB.AutoInc]
        [SpacetimeDB.PrimaryKey]
        public int Id;
        public string? Name;
        public int Age;
    }


    [Table(Name = "message", Public = true)]
    public partial class Message
    {
        public Identity Sender;
        public Timestamp Sent;
        public string Text = "";
    }

    #endregion


    #region Reducers

    [SpacetimeDB.Reducer]
    public static void Add(ReducerContext ctx, string name, int age)
    {
        var person = ctx.Db.Person.Insert(new Person { Name = name, Age = age });
        Log.Info($"Inserted {person.Name} under #{person.Id}");
    }


    [Reducer]
    public static void SetName(ReducerContext ctx, string name)
    {
        name = ValidateName(name);

        var user = ctx.Db.user.Identity.Find(ctx.Sender);
        if (user is not null)
        {
            user.Name = name;
            ctx.Db.user.Identity.Update(user);
        }
    }


    [SpacetimeDB.Reducer]
    public static void SayHello(ReducerContext ctx)
    {
        foreach (var person in ctx.Db.Person.Iter())
        {
            Log.Info($"Hello, {person.Name}!");
        }
        Log.Info("Hello, World!");
    }


    #endregion

    #region Methods

/// <summary>
/// 
/// </summary>
/// <param name="name"></param>
/// <returns></returns>
/// <exception cref="Exception"></exception>
    private static string ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception("Names must not be empty");
        }
        return name;
    }

    #endregion
}
