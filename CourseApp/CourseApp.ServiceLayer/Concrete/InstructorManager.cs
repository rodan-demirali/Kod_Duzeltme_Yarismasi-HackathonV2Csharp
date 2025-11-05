using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.EntityLayer.Dto.InstructorDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class InstructorManager : IInstructorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public InstructorManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllInstructorDto>>> GetAllAsync(bool track = true)
    {
        var instructorList = await _unitOfWork.Instructors.GetAll(false).ToListAsync();
        var instructorListMapping = _mapper.Map<IEnumerable<GetAllInstructorDto>>(instructorList);
        if (!instructorList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllInstructorDto>>(null, ConstantsMessages.InstructorListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllInstructorDto>>(instructorListMapping, ConstantsMessages.InstructorListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdInstructorDto>> GetByIdAsync(string id, bool track = true)
    {
        // ORTA: Null check eksik - id null/empty olabilir
        //fixed
        // ORTA: Index out of range - id çok kısa olabilir
        //fixed hali aşağıda
        if (string.IsNullOrWhiteSpace(id) || id.Length <= 5)
        {
            //InstructorGetByIdFailedMessage
            //return new ErrorDataResult<GetByIdInstructorDto>(null, ConstantsMessages.InstructorGetByIdFailedMessage);
            //daha doğrusu. aşağıda
            return new ErrorDataResult<GetByIdInstructorDto>(new GetByIdInstructorDto(), ConstantsMessages.InstructorGetByIdFailedMessage);

        }
        var idPrefix = id[5]; // IndexOutOfRangeException riski
        
        var hasInstructor = await _unitOfWork.Instructors.GetByIdAsync(id, false);
        // ORTA: Null reference - hasInstructor null olabilir ama kontrol edilmiyor
        //fixed
        if (hasInstructor == null)
        {
            return new ErrorDataResult<GetByIdInstructorDto>(new GetByIdInstructorDto(), ConstantsMessages.InstructorGetByIdFailedMessage);
        }
        var hasInstructorMapping = _mapper.Map<GetByIdInstructorDto>(hasInstructor);
        // ORTA: Null reference - hasInstructorMapping null olabilir
        //fixed
        if (hasInstructorMapping == null)
        {
            return new ErrorDataResult<GetByIdInstructorDto>(new GetByIdInstructorDto(), ConstantsMessages.InstructorGetByIdFailedMessage);
        }
        var name = hasInstructorMapping.Name; // Null reference riski
        return new SuccessDataResult<GetByIdInstructorDto>(hasInstructorMapping, ConstantsMessages.InstructorGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreatedInstructorDto entity)
    {
        var createdInstructor = _mapper.Map<Instructor>(entity);
        await _unitOfWork.Instructors.CreateAsync(createdInstructor);
        var result = await _unitOfWork.CommitAsync();
        if(createdInstructor == null) return new ErrorResult("Null");
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorCreateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.InstructorCreateFailedMessage);
    }

    public async Task<IResult> Remove(DeletedInstructorDto entity)
    {
        var deletedInstructor = _mapper.Map<Instructor>(entity);
        _unitOfWork.Instructors.Remove(deletedInstructor);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.InstructorDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.InstructorDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdatedInstructorDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        //fixed
        if (entity == null)
        {
            return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
        }
        var updatedInstructor = _mapper.Map<Instructor>(entity);

        // ORTA: Null reference - updatedInstructor null olabilir
        //fixed
        if (updatedInstructor == null)
        {
            return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
        }
        //var instructorName = updatedInstructor.Name; // Null reference riski
        var instructorName = updatedInstructor.Name ?? "Bilinmiyor"; //fixed


        try
        {
            _unitOfWork.Instructors.Update(updatedInstructor);
            var result = await _unitOfWork.CommitAsync();

            if (result > 0)
            {
                return new SuccessResult(ConstantsMessages.InstructorUpdateSuccessMessage);
            }

            return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
        }
        catch (Exception)
        {
            return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage);
        }
        //_unitOfWork.Instructors.Update(updatedInstructor);
        //var result = await _unitOfWork.CommitAsync();
        //if (result > 0)
        //{
        //    return new SuccessResult(ConstantsMessages.InstructorUpdateSuccessMessage);
        //}
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor
        return new ErrorResult(ConstantsMessages.InstructorUpdateFailedMessage); // HATA: ErrorResult olmalıydı
    }

    //private void UseNonExistentNamespace()
    //{
    //    var x = NonExistentNamespace.NonExistentClass.Create();
    //}
}
