namespace Models;

public class Class1
{
    public int MyProperty { get; set; }
    public string? MyNullableString { get; set; }
    
    public Class1(int myProperty, string? myNullableString)
    {
        MyProperty = myProperty;
        MyNullableString = myNullableString;
    }
    
    
    public override string ToString()
    {
        return $"MyProperty: {MyProperty}, MyNullableString: {MyNullableString}";
    }
    
    
    
}