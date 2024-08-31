using GraphQLDemo.API.Models;
using HotChocolate.Data.Sorting;

namespace GraphQLDemo.API.Schemas.Sorters
{
    public class CourseSortType:SortInputType<CourseType>
    {
        protected override void Configure(ISortInputTypeDescriptor<CourseType> descriptor)
        {
            descriptor.Ignore(x => x.Id);
            descriptor.Ignore(x => x.InstructorId);
            descriptor.Field(c=>c.Name).Name("CourseName");

            base.Configure(descriptor);
        }
    }
}
