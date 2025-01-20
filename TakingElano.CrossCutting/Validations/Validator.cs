using System.ComponentModel.DataAnnotations;

namespace TakingElano.CrossCutting.Validations;

public static class Validator
{
    public static void Validate(object obj)
    {
        var context = new ValidationContext(obj, null, null);
        var results = new List<ValidationResult>();
        if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(obj, context, results, true))
        {
            var errors = string.Join(", ", results.Select(r => r.ErrorMessage));
            throw new ValidationException($"Validation failed: {errors}");
        }
    }
}
