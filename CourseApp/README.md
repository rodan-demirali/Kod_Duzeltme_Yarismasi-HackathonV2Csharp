# CourseApp - Hata Dokümantasyonu

Bu proje, geliştiricilerin hata bulma ve düzeltme yeteneklerini test etmek amacıyla kasıtlı olarak çeşitli seviyelerde hatalar içermektedir.

## 📊 Hata İstatistikleri

- **Toplam Hata Sayısı:** 75+
- **Kolay Seviye:** 20+ hata
- **Orta Seviye:** 40+ hata
- **Zor Seviye:** 15+ hata

---

## 🟢 KOLAY SEVİYE HATALAR (Build Hataları)

Bu hatalar, projenin derlenmesini (build) doğrudan engelleyen basit sentaks hatalarıdır. IDE veya derleyici yardımıyla kolayca fark edilebilir.

### 1. Noktalı Virgül Eksiklikleri

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 79:** `return BadRequest(result)` - Noktalı virgül eksik
- **Lokasyon:** `Create` metodu

#### `CourseApp.API/Controllers/CoursesController.cs`
- **Satır 68:** `return BadRequest(result)` - Noktalı virgül eksik
- **Lokasyon:** `Create` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamManager.cs`
- **Satır 63:** `return new ErrorResult(...)` - Noktalı virgül eksik
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 56:** `return new ErrorResult(...)` - Noktalı virgül eksik
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 59:** `return new ErrorResult(...)` - Noktalı virgül eksik
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamResultManager.cs`
- **Satır 59:** `return new ErrorResult(...)` - Noktalı virgül eksik
- **Lokasyon:** `CreateAsync` metodu

### 2. İsim Uyuşmazlığı (Typo) - Değişken Adları

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 86:** `updateStudntDto` - `updateStudentDto` yerine yanlış yazılmış
- **Lokasyon:** `Update` metodu

#### `CourseApp.API/Controllers/RegistrationsController.cs`
- **Satır 71:** `rsult` - `result` yerine yanlış yazılmış
- **Lokasyon:** `Create` metodu

### 3. Metod Adı Yanlış Yazımı

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 36:** `result.Succes` - `result.Success` yerine yanlış yazılmış
- **Lokasyon:** `GetAll` metodu

#### `CourseApp.API/Controllers/CoursesController.cs`
- **Satır 33:** `GetByIdAsnc` - `GetByIdAsync` yerine yanlış yazılmış
- **Lokasyon:** `GetById` metodu

#### `CourseApp.API/Controllers/LessonsController.cs`
- **Satır 72:** `CreatAsync` - `CreateAsync` yerine yanlış yazılmış
- **Lokasyon:** `Create` metodu

#### `CourseApp.API/Controllers/ExamResultsController.cs`
- **Satır 36:** `BadReqest` - `BadRequest` yerine yanlış yazılmış
- **Lokasyon:** `GetAll` metodu

#### `CourseApp.API/Program.cs`
- **Satır 26:** `AddScopd` - `AddScoped` yerine yanlış yazılmış
- **Lokasyon:** Service Configuration bölümü

- **Satır 29:** `ExamManagr` - `ExamManager` yerine yanlış yazılmış
- **Lokasyon:** Service Configuration bölümü

- **Satır 65:** `MapContrllers` - `MapControllers` yerine yanlış yazılmış
- **Lokasyon:** Pipeline configuration bölümü

### 4. Yanlış Tip Kullanımı

#### `CourseApp.ServiceLayer/Concrete/CourseManager.cs`
- **Satır 135:** `NonExistentType` - `GetAllCourseDetailDto` yerine var olmayan tip kullanılmış
- **Lokasyon:** `GetAllCourseDetail` metodu

---

## 🟡 ORTA SEVİYE HATALAR (Runtime ve Mantıksal Hatalar)

Bu hatalar projenin derlenmesine engel olmaz, ancak çalışma zamanında (runtime) beklenmedik davranışlara veya Exception fırlatılmasına neden olur.

### 1. Null Reference Exception Riskleri

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 29:** `_cachedStudents` null olabilir ama kontrol edilmeden kullanılıyor
- **Satır 52:** `result.Data.Name` - `result.Data` null olabilir
- **Satır 100:** `deleteStudentDto.Id` - `deleteStudentDto` null olabilir
- **Lokasyon:** `GetAll`, `GetById`, `Delete` metodları

#### `CourseApp.API/Controllers/CoursesController.cs`
- **Satır 57:** `createCourseDto.CourseName` - `createCourseDto` null olabilir
- **Lokasyon:** `Create` metodu

#### `CourseApp.API/Controllers/InstructorsController.cs`
- **Satır 44:** `createdInstructorDto.Name` - `createdInstructorDto` null olabilir
- **Lokasyon:** `Create` metodu

#### `CourseApp.ServiceLayer/Concrete/StudentManager.cs`
- **Satır 41:** `hasStudentMapping.Name` - `hasStudentMapping` null olabilir
- **Satır 54:** `createdStudent.Name` - `createdStudent` null olabilir
- **Lokasyon:** `GetByIdAsync`, `CreateAsync` metodları

#### `CourseApp.ServiceLayer/Concrete/CourseManager.cs`
- **Satır 56:** `hasCourse.CourseName` - `hasCourse` null olabilir (null check yok)
- **Lokasyon:** `GetByIdAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamManager.cs`
- **Satır 53:** `addedExamMapping.Name` - `addedExamMapping` null olabilir
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/InstructorManager.cs`
- **Satır 43:** `hasInstructorMapping.Name` - `hasInstructorMapping` null olabilir
- **Satır 77:** `updatedInstructor.Name` - `updatedInstructor` null olabilir
- **Lokasyon:** `GetByIdAsync`, `Update` metodları

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 45:** `createdRegistration.Price` - `createdRegistration` null olabilir
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 48:** `createdLesson.Name` - `createdLesson` null olabilir
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamResultManager.cs`
- **Satır 49:** `addedExamResultMapping.Score` - `addedExamResultMapping` null olabilir
- **Lokasyon:** `CreateAsync` metodu

### 2. Index Out of Range Exception

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 48:** `id[10]` - `id` 10 karakterden kısa olursa exception
- **Lokasyon:** `GetById` metodu

#### `CourseApp.API/Controllers/CoursesController.cs`
- **Satır 60:** `courseName[0]` - `courseName` boş/null ise exception
- **Lokasyon:** `Create` metodu

#### `CourseApp.API/Controllers/InstructorsController.cs`
- **Satır 47:** `instructorName[0]` - `instructorName` boş/null ise exception
- **Lokasyon:** `Create` metodu

#### `CourseApp.ServiceLayer/Concrete/StudentManager.cs`
- **Satır 85:** `entity.TC[0]` - `entity.TC` null/boş olabilir
- **Lokasyon:** `Update` metodu

#### `CourseApp.ServiceLayer/Concrete/CourseManager.cs`
- **Satır 42:** `result[0]` - `result` boş liste olabilir
- **Lokasyon:** `GetAllAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamManager.cs`
- **Satır 31:** `examtListMapping.ToList()[0]` - Liste boş olabilir
- **Lokasyon:** `GetAllAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/InstructorManager.cs`
- **Satır 37:** `id[5]` - `id` 5 karakterden kısa olabilir
- **Lokasyon:** `GetByIdAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 80:** `entity.Name[0]` - `entity.Name` null/boş olabilir
- **Lokasyon:** `Update` metodu

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 105:** `registrationDataMapping.ToList()[0]` - Liste boş olabilir
- **Lokasyon:** `GetAllRegistrationDetailAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 101:** `lessonsListMapping.First()` - Liste boş olabilir
- **Lokasyon:** `GetAllLessonDetailAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamResultManager.cs`
- **Satır 96:** `examResultListMapping.ToList()[0]` - Liste boş olabilir
- **Lokasyon:** `GetAllExamResultDetailAsync` metodu

### 3. Yanlış Tip Dönüşümü (Invalid Cast Exception)

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 65:** `(int)createStudentDto.Name` - String'i int'e direkt cast edilemez
- **Lokasyon:** `Create` metodu

#### `CourseApp.API/Controllers/InstructorsController.cs`
- **Satır 50:** `(int)instructorName` - String'i int'e direkt cast edilemez
- **Lokasyon:** `Create` metodu

#### `CourseApp.API/Controllers/RegistrationsController.cs`
- **Satır 67:** `(int)createRegistrationDto.Price` - Decimal'i int'e direkt cast edilemez
- **Lokasyon:** `Create` metodu

#### `CourseApp.ServiceLayer/Concrete/StudentManager.cs`
- **Satır 50:** `(int)entity.TC` - String'i int'e direkt cast edilemez
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 77:** `(int)updatedRegistration.Price` - Decimal'i int'e direkt cast edilemez
- **Lokasyon:** `Update` metodu

### 4. Mantıksal Hatalar

#### `CourseApp.ServiceLayer/Concrete/StudentManager.cs`
- **Satır 92:** Başarılı durumda yanlış mesaj döndürülüyor (`StudentListSuccessMessage` yerine `StudentUpdateSuccessMessage` olmalı)
- **Satır 95:** Hata durumunda `SuccessResult` döndürülüyor (hatalı - `ErrorResult` olmalı)
- **Lokasyon:** `Update` metodu

#### `CourseApp.ServiceLayer/Concrete/InstructorManager.cs`
- **Satır 86:** Hata durumunda `SuccessResult` döndürülüyor (hatalı - `ErrorResult` olmalı)
- **Lokasyon:** `Update` metodu

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 86:** Hata durumunda `SuccessResult` döndürülüyor (hatalı - `ErrorResult` olmalı)
- **Lokasyon:** `Update` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 40:** Yanlış mesaj döndürülüyor (`InstructorGetByIdSuccessMessage` yerine `LessonGetByIdSuccessMessage` olmalı)
- **Satır 89:** Hata durumunda `SuccessResult` döndürülüyor (hatalı - `ErrorResult` olmalı)
- **Lokasyon:** `GetByIdAsync`, `Update` metodları

---

## 🔴 ZOR SEVİYE HATALAR (Mimari ve Performans Sorunları)

Bu hatalar, projenin temel mimarisinde, tasarım desenlerinde veya performansında ciddi sorunlar yaratır.

### 1. N+1 Problemi (Lazy Loading)

#### `CourseApp.ServiceLayer/Concrete/CourseManager.cs`
- **Satır 24-39:** `GetAllAsync` - Her course için Instructor ayrı sorgu ile çekiliyor (Include/ThenInclude kullanılmamış)
- **Satır 131-146:** `GetAllCourseDetail` - Her course için Instructor ayrı sorgu ile çekiliyor (`x.Instructor?.Name` lazy loading)
- **Lokasyon:** `GetAllAsync`, `GetAllCourseDetail` metodları

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 91-105:** `GetAllRegistrationDetailAsync` - Her registration için Course ve Student ayrı sorgu ile çekiliyor
- **Lokasyon:** `GetAllRegistrationDetailAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 94-101:** `GetAllLessonDetailAsync` - Her lesson için Course ayrı sorgu ile çekiliyor (`lesson.Course?.CourseName` lazy loading)
- **Lokasyon:** `GetAllLessonDetailAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamResultManager.cs`
- **Satır 82-96:** `GetAllExamResultDetailAsync` - Her examResult için Student ve Exam ayrı sorgu ile çekiliyor
- **Lokasyon:** `GetAllExamResultDetailAsync` metodu

#### `CourseApp.API/Controllers/ExamsController.cs`
- **Satır 21-32:** `GetAll` - Her exam için ayrı sorgu ile detay çekiliyor (foreach içinde `GetByIdAsync` çağrısı)
- **Lokasyon:** `GetAll` metodu

#### `CourseApp.API/Controllers/ExamResultsController.cs`
- **Satır 21-33:** `GetAll` - Her examResult için ayrı sorgu ile detay çekiliyor (foreach içinde `GetByIdExamResultDetailAsync` çağrısı)
- **Lokasyon:** `GetAll` metodu

### 2. Async/Await Anti-Pattern

#### `CourseApp.ServiceLayer/Concrete/StudentManager.cs`
- **Satır 58:** `.Result` kullanımı - Deadlock riski
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamManager.cs`
- **Satır 26:** Async metot içinde senkron `ToList()` kullanımı (`ToListAsync()` kullanılmalıydı)
- **Satır 56:** `.Wait()` kullanımı - Deadlock riski
- **Lokasyon:** `GetAllAsync`, `CreateAsync` metodları

#### `CourseApp.ServiceLayer/Concrete/RegistrationManager.cs`
- **Satır 48:** `GetAwaiter().GetResult()` kullanımı - Deadlock riski
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/LessonsManager.cs`
- **Satır 51:** `GetAwaiter().GetResult()` kullanımı - Deadlock riski
- **Lokasyon:** `CreateAsync` metodu

#### `CourseApp.ServiceLayer/Concrete/ExamResultManager.cs`
- **Satır 53:** `GetAwaiter().GetResult()` kullanımı - Deadlock riski
- **Lokasyon:** `CreateAsync` metodu

### 3. Katman İhlali (Architecture Violation)

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 5:** `using CourseApp.DataAccessLayer.Concrete;` - Presentation katmanından direkt DataAccessLayer'a erişim
- **Satır 15:** `AppDbContext _dbContext` - Controller'da DbContext kullanımı (Business Logic bypass ediliyor)
- **Satır 68-71:** Controller'dan direkt DbContext'e erişim - `_dbContext.Students.Add(...)`
- **Lokasyon:** Sınıf tanımı ve `Create` metodu

### 4. Memory Leak (Bellek Sızıntısı)

#### `CourseApp.API/Controllers/StudentsController.cs`
- **Satır 103-104:** `AppDbContext` Dispose edilmeden kullanılıyor
- **Lokasyon:** `Delete` metodu

---

## 📍 Hata Lokasyon Haritası

### Controllers
- `StudentsController.cs` - 10+ hata
- `CoursesController.cs` - 4 hata
- `ExamsController.cs` - 3 hata
- `ExamResultsController.cs` - 3 hata
- `InstructorsController.cs` - 4 hata
- `LessonsController.cs` - 4 hata
- `RegistrationsController.cs` - 3 hata

### Service Layer
- `StudentManager.cs` - 8 hata
- `CourseManager.cs` - 7 hata
- `ExamManager.cs` - 6 hata
- `InstructorManager.cs` - 5 hata
- `RegistrationManager.cs` - 7 hata
- `LessonsManager.cs` - 7 hata
- `ExamResultManager.cs` - 5 hata

### Configuration
- `Program.cs` - 3 hata

---

## 🎯 Hata Kategorileri

### Build Hataları (Derleme Zamanı)
- Noktalı virgül eksiklikleri
- Typo'lar (değişken, metod, tip adları)
- Yanlış tip kullanımı

### Runtime Hataları (Çalışma Zamanı)
- Null Reference Exception
- Index Out of Range Exception
- Invalid Cast Exception
- Mantıksal hatalar

### Performans Sorunları
- N+1 Query Problem
- Async/Await anti-pattern'ler
- Memory leak'ler

### Mimari Sorunlar
- Katman ihlalleri
- Business Logic bypass'ları

---

## 🔍 Hata Bulma İpuçları

1. **Build hataları:** IDE veya compiler çıktısını kontrol edin
2. **Runtime hataları:** Unit test yazın veya uygulamayı çalıştırıp exception loglarını inceleyin
3. **Performans sorunları:** Profiling araçları kullanın (SQL Server Profiler, Application Insights)
4. **Mimari sorunlar:** Kod review yapın, katman bağımlılıklarını kontrol edin

---

## ⚠️ Not

Bu hatalar kasıtlı olarak eklenmiştir. Tüm hatalar kod içinde yorum satırları ile işaretlenmiştir. Her hatanın yanında hangi seviyede olduğu ve ne tür bir hata olduğu belirtilmiştir.

---

**Son Güncelleme:** 2025-02-11
**Toplam Hata Sayısı:** 75+

