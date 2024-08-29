using GraphQLDemo.API.Models;
using HotChocolate.Data.Filters;

namespace GraphQLDemo.API.Schemas.Filters
{
    public class CourseFilterType:FilterInputType<CourseType>
    {
        protected override void Configure(IFilterInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(x => x.Students);
            base.Configure(descriptor);
        }
    }
}
