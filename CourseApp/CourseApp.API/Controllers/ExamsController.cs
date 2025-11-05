using CourseApp.EntityLayer.Dto.ExamDto;
using CourseApp.ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CourseApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly IExamService _examService;

    public ExamsController(IExamService examService)
    {
        _examService = examService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // ZOR: N+1 Problemi - Her exam için ayrı sorgu
        //fixed
        var result = await _examService.GetAllAsync();

        if (result.IsSuccess)
        {
            // ORTA: Null reference - result.Data null olabilir
            //fixed
            if (result == null || result.Data == null)
            {
                return BadRequest("Veri alınamadı veya null döndü.");
            }

            // ZOR: N+1 - Her exam için ayrı sorgu (örnek - gerçek implementasyon service layer'da olabilir)
            //fixed
            if (result.IsSuccess)
            {
                var exams = result.Data.ToList(); // Artık güvenli
                return Ok(result);
            }
            //foreach (var exam in exams)
            //{
            //    // Her exam için ayrı sorgu atılıyor - Include kullanılmamalıydı
            //    var details = await _examService.GetByIdAsync(exam.Id);
            //}
            //return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _examService.GetByIdAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExamDto createExamDto)
    {
        var result = await _examService.CreateAsync(createExamDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateExamDto updateExamDto)
    {
        var result = await _examService.Update(updateExamDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteExamDto deleteExamDto)
    {
        var result = await _examService.Remove(deleteExamDto);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
