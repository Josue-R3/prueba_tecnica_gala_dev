using Application.DTOs.Tienda;
using FluentValidation;

namespace Application.Validators.Tienda;

public class CreateTiendaValidator : AbstractValidator<CreateTiendaDto>
{
    public CreateTiendaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres");
    }
}
