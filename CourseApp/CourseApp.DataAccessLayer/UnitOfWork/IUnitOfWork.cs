using CourseApp.DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourseApp.DataAccessLayer.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IStudentRepository Students { get; }
        ILessonRepository Lessons { get; }
        ICourseRepository Courses { get; }
        IExamRepository Exams { get; }
        IExamResultRepository ExamResults { get; }
        IInstructorRepository Instructors { get; }
        IRegistrationRepository Registrations { get; }

        Task<int> CommitAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
