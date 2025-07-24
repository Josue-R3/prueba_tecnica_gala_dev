using Application.DTOs.Common;
using Application.DTOs.Empleado;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;

namespace Application.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateEmpleadoDto> _createValidator;
    private readonly IValidator<UpdateEmpleadoDto> _updateValidator;

    public EmpleadoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateEmpleadoDto> createValidator,
        IValidator<UpdateEmpleadoDto> updateValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<EmpleadoDto?> GetByIdAsync(int id)
    {
        var empleado = await _unitOfWork.EmpleadoRepository.GetByIdAsync(id);
        return empleado != null ? _mapper.Map<EmpleadoDto>(empleado) : null;
    }

    public async Task<IEnumerable<EmpleadoDto>> GetAllAsync()
    {
        var empleados = await _unitOfWork.EmpleadoRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<EmpleadoDto>>(empleados);
    }

    public async Task<PagedResultDto<EmpleadoDto>> GetPaginatedAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var (empleados, totalCount) = await _unitOfWork.EmpleadoRepository.GetEmpleadosPaginatedAsync(pageNumber, pageSize, searchTerm);

        return new PagedResultDto<EmpleadoDto>
        {
            Data = _mapper.Map<IEnumerable<EmpleadoDto>>(empleados),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<EmpleadoDto>> SearchAsync(string searchTerm)
    {
        var empleados = await _unitOfWork.EmpleadoRepository.SearchEmpleadosAsync(searchTerm);
        return _mapper.Map<IEnumerable<EmpleadoDto>>(empleados);
    }

    public async Task<EmpleadoDto> CreateAsync(CreateEmpleadoDto createDto)
    {
        var validationResult = await _createValidator.ValidateAsync(createDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        // Verificar que la tienda existe
        if (!await _unitOfWork.TiendaRepository.ExistsAsync(createDto.TiendaId))
            throw new ArgumentException("La tienda especificada no existe");

        // Verificar que el correo no existe
        if (await _unitOfWork.EmpleadoRepository.ExistsByCorreoAsync(createDto.Correo))
            throw new ArgumentException("Ya existe un empleado con este correo");

        var empleado = _mapper.Map<Empleado>(createDto);
        var createdEmpleado = await _unitOfWork.EmpleadoRepository.AddAsync(empleado);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EmpleadoDto>(createdEmpleado);
    }

    public async Task<EmpleadoDto> UpdateAsync(UpdateEmpleadoDto updateDto)
    {
        var validationResult = await _updateValidator.ValidateAsync(updateDto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingEmpleado = await _unitOfWork.EmpleadoRepository.GetByIdAsync(updateDto.Id);
        if (existingEmpleado == null)
            throw new ArgumentException("El empleado no existe");

        // Verificar que la tienda existe
        if (!await _unitOfWork.TiendaRepository.ExistsAsync(updateDto.TiendaId))
            throw new ArgumentException("La tienda especificada no existe");

        // Verificar que el correo no existe en otro empleado
        if (await _unitOfWork.EmpleadoRepository.ExistsByCorreoAsync(updateDto.Correo, updateDto.Id))
            throw new ArgumentException("Ya existe otro empleado con este correo");

        _mapper.Map(updateDto, existingEmpleado);
        var updatedEmpleado = await _unitOfWork.EmpleadoRepository.UpdateAsync(existingEmpleado);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<EmpleadoDto>(updatedEmpleado);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var empleado = await _unitOfWork.EmpleadoRepository.GetByIdAsync(id);
        if (empleado == null)
            return false;

        // Soft delete - cambiar estado a inactivo
        empleado.Estado = false;
        await _unitOfWork.EmpleadoRepository.UpdateAsync(empleado);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _unitOfWork.EmpleadoRepository.ExistsAsync(id);
    }
}
