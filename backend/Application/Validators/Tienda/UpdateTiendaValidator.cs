using Application.DTOs.Tienda;
using FluentValidation;

namespace Application.Validators.Tienda;

public class UpdateTiendaValidator : AbstractValidator<UpdateTiendaDto>
{
    public UpdateTiendaValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID de la tienda debe ser válido");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(200).WithMessage("La dirección no puede exceder 200 caracteres");
    }
}
