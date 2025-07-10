using FluentValidation;
using workstation_back_end.Users.Domain.Models.Commands;

namespace workstation_back_end.Users.Domain.Models.Validadors;

/// <summary>
/// Validador para la creación de un user base en TripMatch.
/// </summary>
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.FirstName)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(50).WithMessage("Máximo 50 caracteres.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(50).WithMessage("Máximo 50 caracteres.");

        RuleFor(u => u.Number)
            .NotEmpty().WithMessage("El número de teléfono es obligatorio.")
            .Matches(@"^\d{7,15}$").WithMessage("Debe contener entre 7 y 15 dígitos.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("Formato de correo inválido.")
            .MaximumLength(100);

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("Mínimo 6 caracteres.")
            .MaximumLength(100).WithMessage("Máximo 100 caracteres.");
    }
}