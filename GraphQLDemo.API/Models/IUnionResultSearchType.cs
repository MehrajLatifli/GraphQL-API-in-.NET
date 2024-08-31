namespace GraphQLDemo.API.Models
{
    [UnionType("UnionSearchResult")]
    public interface IUnionResultSearchType
    {
        public string Id { get; set; }
    }
}
