using System.ComponentModel.DataAnnotations;

namespace Blazor.WebAssembly.Services;

public class DisplayStringPropertyValueAttribute : StringLengthAttribute {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) 
        => !IsValid(value) ? new ValidationResult(value?.ToString(),
                new[] { validationContext.MemberName }!) : ValidationResult.Success;

    public DisplayStringPropertyValueAttribute() : base(0) {
    }
}