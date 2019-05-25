public class Person
{
    private string firstName;
    private string lastName;

    public string FirstName { get { return firstName; } set { firstName = value; } }
    public string LastName { get { return lastName; } set { lastName = value; } }

    public Person() { }

    public Person(string firstName, string lastName)
    {
        this.firstName = firstName;
        this.lastName = lastName;
    }

    public override string ToString()
    {
        return string.Format(firstName + lastName);
    } 
}
