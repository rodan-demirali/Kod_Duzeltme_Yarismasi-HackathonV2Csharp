using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.RegistrationDto;
using CourseApp.EntityLayer.Dto.StudentDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class StudentManager : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public StudentManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllStudentDto>>> GetAllAsync(bool track = true)
    {
        var studentList = await _unitOfWork.Students.GetAll(track).ToListAsync();
        var studentListMapping = _mapper.Map<IEnumerable<GetAllStudentDto>>(studentList);
        if (!studentList.Any())
        {
            return new ErrorDataResult<IEnumerable<GetAllStudentDto>>(null, ConstantsMessages.StudentListFailedMessage);
        }
        return new SuccessDataResult<IEnumerable<GetAllStudentDto>>(studentListMapping, ConstantsMessages.StudentListSuccessMessage);
    }

    public async Task<IDataResult<GetByIdStudentDto>> GetByIdAsync(string id, bool track = true)
    {
        // ORTA: Null check eksik - id null/empty olabilir
        //fixed
        if (string.IsNullOrWhiteSpace(id))
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);

        // ORTA: Null reference exception - hasStudent null olabilir ama kontrol edilmiyor
        //fixed
        var hasStudent = await _unitOfWork.Students.GetByIdAsync(id, false);
        if (hasStudent == null)
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);

        var hasStudentMapping = _mapper.Map<GetByIdStudentDto>(hasStudent);
        // ORTA: Null reference - hasStudentMapping null olabilir ama kullanılıyor
        //fixed
        if (hasStudentMapping == null)
            return new ErrorDataResult<GetByIdStudentDto>(null, ConstantsMessages.StudentGetByIdFailedMessage);

        var name = hasStudentMapping.Name; // Null reference riski --> fixed
        return new SuccessDataResult<GetByIdStudentDto>(hasStudentMapping, ConstantsMessages.StudentGetByIdSuccessMessage);
    }

    public async Task<IResult> CreateAsync(CreateStudentDto entity)
    {
        if(entity == null) return new ErrorResult("Geçersiz öğrenci bilgisi gönderildi.");

        // ORTA: Tip dönüşüm hatası - string'i int'e direkt cast
        //fixed
        //var invalidConversion = (int)entity.TC; // ORTA: InvalidCastException - string int'e dönüştürülemez
        if (!long.TryParse(entity.TC, out long parsedTC))
            return new ErrorResult("TC kimlik numarası geçersiz formatta.");
        
        var createdStudent = _mapper.Map<Student>(entity);
        // ORTA: Null reference - createdStudent null olabilir
        //fixed
        if (createdStudent == null)
            return new ErrorResult("Öğrenci verisi oluşturulamadı (mapping hatası).");
        var studentName = createdStudent.Name; // Null check yok

        await _unitOfWork.Students.CreateAsync(createdStudent);
        // ZOR: Async/await anti-pattern - .Result kullanımı deadlock'a sebep olabilir
        //fixed
        //var result = _unitOfWork.CommitAsync().Result; // ZOR: Anti-pattern
        var result = await _unitOfWork.CommitAsync();

        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.StudentCreateSuccessMessage);
        }

        return new ErrorResult(ConstantsMessages.StudentCreateFailedMessage);
    }

    public async Task<IResult> Remove(DeleteStudentDto entity)
    {
        var deletedStudent = _mapper.Map<Student>(entity);
        _unitOfWork.Students.Remove(deletedStudent);
        var result = _unitOfWork.CommitAsync().GetAwaiter().GetResult();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.StudentDeleteSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.StudentDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateStudentDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        //fixed
        if (entity == null)
            return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);
        var updatedStudent = _mapper.Map<Student>(entity);
        if (updatedStudent == null)
            return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);


        // ORTA: Index out of range - entity.TC null/boş olabilir
        //fixed
        if (string.IsNullOrWhiteSpace(entity.TC))
            return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);
        var tcFirstDigit = entity.TC[0]; // IndexOutOfRangeException riski

        try
        {
            var tcFirstDigit2 = entity.TC[0];
            if (!char.IsDigit(tcFirstDigit2) || tcFirstDigit == '0')
                return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);

            _unitOfWork.Students.Update(updatedStudent);
            var result2 = await _unitOfWork.CommitAsync();

            if (result2 > 0)
            {
                return new SuccessResult(ConstantsMessages.StudentUpdateSuccessMessage);
            }

            return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);
        }
        catch (Exception)
        {
            return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage);
        }

        _unitOfWork.Students.Update(updatedStudent);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            // ORTA: Mantıksal hata - başarılı durumda yanlış mesaj döndürülüyor
            //fixed yukarıdaki yeni kalıp ile.
            return new SuccessResult(ConstantsMessages.StudentUpdateSuccessMessage); // HATA: UpdateSuccessMessage olmalıydı
        }
        // ORTA: Mantıksal hata - hata durumunda SuccessResult döndürülüyor
        return new ErrorResult(ConstantsMessages.StudentUpdateFailedMessage); // HATA: ErrorResult olmalıydı
    }

    //fixed
    //public void MissingImplementation()
    //{
    //    var x = UnknownClass.StaticMethod();
    //}
}
