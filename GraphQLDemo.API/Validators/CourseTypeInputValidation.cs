using FluentValidation;
using GraphQLDemo.API.Models;

namespace GraphQLDemo.API.Validators
{
    public class CourseTypeInputValidation : AbstractValidator<CourseTypeInput>
    {
        public CourseTypeInputValidation()
        {
            RuleFor(c=>c.Name)
                .MinimumLength(3)
                .MaximumLength(50)
                .WithMessage("Course name must be between 3 and 50 characters.")
                .WithErrorCode("422");
        }
    }
}
