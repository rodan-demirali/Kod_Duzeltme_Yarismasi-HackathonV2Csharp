using CourseApp.EntityLayer.Dto.LessonDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _lessonService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _lessonService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _lessonService.GetAllLessonDetailAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail/{id}")]
    public async Task<IActionResult> GetByIdDetail(string id)
    {
        var result = await _lessonService.GetByIdLessonDetailAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto createLessonDto)
    {
        // ORTA: Null check eksik - createLessonDto null olabilir
        //fixed
        if (createLessonDto == null)
            return BadRequest("Data girişi gerekli.");

        //Name -> Title'ye çevrildi. class tanımında Name diye bir değişken yok.
        //fixed
        if (string.IsNullOrWhiteSpace(createLessonDto.Title))
            return BadRequest("Bir isim giriniz.");

        var lessonName = createLessonDto.Title; // Null reference riski --> fixed
        
        // ORTA: Index out of range - lessonName boş/null ise
        var firstChar = lessonName[0]; // IndexOutOfRangeException riski
        
        // KOLAY: Metod adı yanlış yazımı - CreateAsync yerine CreatAsync
        //fixed
        var result = await _lessonService.CreateAsync(createLessonDto); // TYPO: Create yerine Creat
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        // KOLAY: Noktalı virgül eksikliği
        //fixed
        return BadRequest(result); // TYPO: ; eksik
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateLessonDto updateLessonDto)
    {
        var result = await _lessonService.Update(updateLessonDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteLessonDto deleteLessonDto)
    {
        var result = await _lessonService.Remove(deleteLessonDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
