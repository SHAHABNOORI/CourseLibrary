using System.ComponentModel.DataAnnotations;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForCreationDto)validationContext.ObjectInstance;
            return course.Title == course.Description ?
                new ValidationResult(ErrorMessage, new[] { nameof(CourseForCreationDto) })
                : ValidationResult.Success;

            //return new ValidationResult(
            //   "The provided description should be different from the title.",
            //   new[] { nameof(CourseForCreationDto) });
        }
    }
}