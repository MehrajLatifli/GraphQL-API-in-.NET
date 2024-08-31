namespace GraphQLDemo.API.Models
{
    public class InstructorType: IInterfaceResultSearchType, IUnionResultSearchType
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double Salary { get; set; }
    }
}
