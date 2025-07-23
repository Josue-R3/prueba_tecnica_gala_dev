using Application.DTOs.Store;
using FluentValidation;

namespace Application.Validators.Store;

public class UpdateStoreDtoValidator : AbstractValidator<UpdateStoreDto>
{
    public UpdateStoreDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("El ID debe ser mayor a 0");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre de la tienda es requerido")
            .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(200).WithMessage("La dirección no puede tener más de 200 caracteres");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado no es válido");
    }
}
