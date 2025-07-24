using Application.DTOs.Tienda;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class TiendaService : ITiendaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTiendaDto> _createValidator;
    private readonly IValidator<UpdateTiendaDto> _updateValidator;

    public TiendaService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateTiendaDto> createValidator,
        IValidator<UpdateTiendaDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<TiendaDto?> GetByIdAsync(int id)
    {
        var tienda = await _unitOfWork.TiendaRepository.GetByIdAsync(id);
        return tienda != null ? _mapper.Map<TiendaDto>(tienda) : null;
    }

    public async Task<IEnumerable<TiendaDto>> GetAllAsync()
    {
        var tiendas = await _unitOfWork.TiendaRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TiendaDto>>(tiendas);
    }

    public async Task<IEnumerable<TiendaDto>> GetActivasAsync()
    {
        var tiendas = await _unitOfWork.TiendaRepository.GetTiendasActivasAsync();
        return _mapper.Map<IEnumerable<TiendaDto>>(tiendas);
    }

    public async Task<TiendaDto> CreateAsync(CreateTiendaDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Verificar que el nombre no existe
        if (await _unitOfWork.TiendaRepository.ExistsByNameAsync(createDto.Nombre))
            throw new ArgumentException("Ya existe una tienda con este nombre");

        var tienda = _mapper.Map<Tienda>(createDto);
        var createdTienda = await _unitOfWork.TiendaRepository.AddAsync(tienda);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TiendaDto>(createdTienda);
    }

    public async Task<TiendaDto> UpdateAsync(UpdateTiendaDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingTienda = await _unitOfWork.TiendaRepository.GetByIdAsync(updateDto.Id);
        if (existingTienda == null)
            throw new ArgumentException("La tienda no existe");

        // Verificar que el nombre no existe en otra tienda
        if (await _unitOfWork.TiendaRepository.ExistsByNameAsync(updateDto.Nombre, updateDto.Id))
            throw new ArgumentException("Ya existe otra tienda con este nombre");

        _mapper.Map(updateDto, existingTienda);
        var updatedTienda = await _unitOfWork.TiendaRepository.UpdateAsync(existingTienda);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TiendaDto>(updatedTienda);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tienda = await _unitOfWork.TiendaRepository.GetByIdAsync(id);
        if (tienda == null)
            return false;

        // Soft delete - cambiar estado a inactivo
        tienda.Estado = false;
        await _unitOfWork.TiendaRepository.UpdateAsync(tienda);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _unitOfWork.TiendaRepository.ExistsAsync(id);
    }
}
