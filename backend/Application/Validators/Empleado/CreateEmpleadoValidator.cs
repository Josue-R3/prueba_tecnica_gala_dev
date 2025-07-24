using Application.DTOs.Empleado;
using FluentValidation;

namespace Application.Validators.Empleado;

public class CreateEmpleadoValidator : AbstractValidator<CreateEmpleadoDto>
{
    public CreateEmpleadoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(50).WithMessage("El apellido no puede exceder 50 caracteres");

        RuleFor(x => x.Correo)
            .NotEmpty().WithMessage("El correo es requerido")
            .EmailAddress().WithMessage("El formato del correo no es válido")
            .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres");

        RuleFor(x => x.Cargo)
            .NotEmpty().WithMessage("El cargo es requerido")
            .MaximumLength(50).WithMessage("El cargo no puede exceder 50 caracteres");

        RuleFor(x => x.FechaIngreso)
            .NotEmpty().WithMessage("La fecha de ingreso es requerida")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("La fecha de ingreso no puede ser futura");

        RuleFor(x => x.TiendaId)
            .GreaterThan(0).WithMessage("Debe seleccionar una tienda válida");
    }
}
