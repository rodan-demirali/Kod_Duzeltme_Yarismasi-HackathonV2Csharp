using AutoMapper;
using CourseApp.DataAccessLayer.UnitOfWork;
using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.EntityLayer.Entity;
using CourseApp.ServiceLayer.Abstract;
using CourseApp.ServiceLayer.Utilities.Constants;
using CourseApp.ServiceLayer.Utilities.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseApp.ServiceLayer.Concrete;

public class ExamManager : IExamService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExamManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<IEnumerable<GetAllExamDto>>> GetAllAsync(bool track = true)
    {
        // ZOR: Async/await anti-pattern - async metot içinde senkron ToList kullanımı
        //fixed
        //var examList = _unitOfWork.Exams.GetAll(false).ToList(); // ZOR: ToListAsync kullanılmalıydı
        var examList = await _unitOfWork.Exams.GetAll(false).ToListAsync();

        // KOLAY: Değişken adı typo - examtListMapping yerine examListMapping
        //fixed
        var examtListMapping = _mapper.Map<IEnumerable<GetAllExamDto>>(examList); // TYPO

        // ORTA: Index out of range - examtListMapping boş olabilir
        //fixed
        var examListAsList = examtListMapping?.ToList() ?? new List<GetAllExamDto>();
        if (examListAsList.Count == 0)
        {
            return new ErrorDataResult<IEnumerable<GetAllExamDto>>(
                examListAsList,
                "Hiç sınav bulunamadı."
            );
        }

        var firstExam = examListAsList.First();
        //var firstExam = examtListMapping.ToList()[0]; // IndexOutOfRangeException riski

        //return new SuccessDataResult<IEnumerable<GetAllExamDto>>(examtListMapping, ConstantsMessages.ExamListSuccessMessage);
        return new SuccessDataResult<IEnumerable<GetAllExamDto>>(examListAsList,ConstantsMessages.ExamListSuccessMessage);
    }

    //fixed
    //public void NonExistentMethod()
    //{
    //    var x = new MissingType();
    //}

    public async Task<IDataResult<GetByIdExamDto>> GetByIdAsync(string id, bool track = true)
    {
        var hasExam = await _unitOfWork.Exams.GetByIdAsync(id, false);
        var examResultMapping = _mapper.Map<GetByIdExamDto>(hasExam);
        return new SuccessDataResult<GetByIdExamDto>(examResultMapping, ConstantsMessages.ExamGetByIdSuccessMessage);
    }
    public async Task<IResult> CreateAsync(CreateExamDto entity)
    {
        // ORTA: Null check eksik - entity null olabilir
        //fixed 
        if (entity == null)
            return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage);

        if (string.IsNullOrWhiteSpace(entity.Name))
            return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage);
        var addedExamMapping = _mapper.Map<Exam>(entity);

        // ORTA: Null reference - addedExamMapping null olabilir
        //fixed ?
        if (addedExamMapping == null)
            return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage);

        if (string.IsNullOrWhiteSpace(addedExamMapping.Name))
            return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage);

        var examName = addedExamMapping.Name; // Null reference riski

        // ZOR: Async/await anti-pattern - async metot içinde .Wait() kullanımı deadlock'a sebep olabilir
        //fixed
        //_unitOfWork.Exams.CreateAsync(addedExamMapping).Wait(); // ZOR: Anti-pattern - await kullanılmalıydı
        await _unitOfWork.Exams.CreateAsync(addedExamMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamCreateSuccessMessage);
        }
        // KOLAY: Noktalı virgül eksikliği
        //fixed
        return new ErrorResult(ConstantsMessages.ExamCreateFailedMessage); // TYPO: ; eksik
    }

    public async Task<IResult> Remove(DeleteExamDto entity)
    {
        if (entity == null || string.IsNullOrWhiteSpace(entity.Id))
        {
            return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
        }
        //fixed
        var deletedExamMapping = _mapper.Map<Exam>(entity); // Fixed -> ORTA SEVİYE: ID kontrolü eksik - entity ID'si null/empty olabilir
        if (deletedExamMapping == null)
        {
            return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
        }

        //fixed - zor hatay gidermek icin BeginTransactionAsync yöntemi eklendi.
        try
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync();

            _unitOfWork.Exams.Remove(deletedExamMapping);
            var result = await _unitOfWork.CommitAsync();

            if (result > 0)
            {
                await transaction.CommitAsync();
                return new SuccessResult(ConstantsMessages.ExamDeleteSuccessMessage);
            }

            await transaction.RollbackAsync();
            return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
        }
        catch (Exception)
        {
            return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
        }

        //_unitOfWork.Exams.Remove(deletedExamMapping);
        //var result = await _unitOfWork.CommitAsync(); Fixed -> // ZOR SEVİYE: Transaction yok - başka işlemler varsa rollback olmaz 
        //if (result > 0)
        //{
        //    return new SuccessResult(ConstantsMessages.ExamDeleteSuccessMessage);
        //}
        //return new ErrorResult(ConstantsMessages.ExamDeleteFailedMessage);
    }

    public async Task<IResult> Update(UpdateExamDto entity)
    {
        var updatedExamMapping = _mapper.Map<Exam>(entity);
        _unitOfWork.Exams.Update(updatedExamMapping);
        var result = await _unitOfWork.CommitAsync();
        if (result > 0)
        {
            return new SuccessResult(ConstantsMessages.ExamUpdateSuccessMessage);
        }
        return new ErrorResult(ConstantsMessages.ExamUpdateFailedMessage);
    }
}
