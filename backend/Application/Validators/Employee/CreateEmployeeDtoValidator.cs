using Application.DTOs.Employee;
using FluentValidation;

namespace Application.Validators.Employee;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede tener más de 50 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(50).WithMessage("El apellido no puede tener más de 50 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido")
            .MaximumLength(100).WithMessage("El correo electrónico no puede tener más de 100 caracteres");

        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("El cargo es requerido")
            .MaximumLength(100).WithMessage("El cargo no puede tener más de 100 caracteres");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("La fecha de ingreso es requerida")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("La fecha de ingreso no puede ser futura");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado no es válido");
    }
}
