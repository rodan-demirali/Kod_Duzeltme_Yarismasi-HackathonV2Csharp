using CourseApp.EntityLayer.Dto.CourseDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _courseService.GetAllAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("ID is required.");

        var result = await _courseService.GetByIdAsync(id); // TYPO: Async yerine Asnc
        //fixed

        // ORTA: Null reference - result null olabilir
        //fixed
        if (result == null)
            return NotFound("Course not found.");

        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetAllDetail()
    {
        var result = await _courseService.GetAllCourseDetail();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto createCourseDto)
    {
        // ORTA: Null check eksik - createCourseDto null olabilir
        //fixed
        if (createCourseDto == null)
            return BadRequest("Data is required.");

        if (string.IsNullOrEmpty(createCourseDto.CourseName))
            return BadRequest("Course name is required.");

        var courseName = createCourseDto.CourseName; // Null reference riski

        // ORTA: Array index out of range - courseName boş/null ise
        //fixed
        if (string.IsNullOrWhiteSpace(courseName))
            return BadRequest("Name is required.");
        var firstChar = courseName.Trim()[0]; // IndexOutOfRangeException riski
        
        var result = await _courseService.CreateAsync(createCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        // KOLAY: Noktalı virgül eksikliği
        //fixed
        return BadRequest(result); // TYPO: ; eksik
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCourseDto updateCourseDto)
    {
        var result = await _courseService.Update(updateCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCourseDto deleteCourseDto)
    {
        var result = await _courseService.Remove(deleteCourseDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
