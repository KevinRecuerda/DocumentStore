namespace Model
{
    public class User
    {
        public long   Id        { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public bool   Internal  { get; set; }
        public string UserName  { get; set; }
        public Gender Gender    { get; set; }

        public Address Address { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}";
        }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City    { get; set; }
        public string Street  { get; set; }
    }

    public enum Gender
    {
        Male   = 0,
        Female = 1
    }
}