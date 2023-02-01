using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using WalkDifficulty = NZWalks.API.Models.Domain.WalkDifficulty;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WalkDifficultyController : Controller
{
    private readonly IWalkDifficultyRepository _walkDifficultyRepository;
    private readonly IMapper _mapper;

    public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
    {
        this._walkDifficultyRepository = walkDifficultyRepository;
        this._mapper = mapper;
    }

    [HttpGet]
    [ActionName("GetAllWalkDifficultiesAsync")]
    public async Task<IActionResult> GetAllWalkDifficultiesAsync()
    {
        var walkDifficulties = await _walkDifficultyRepository.GetAllAsync();
        return Ok(walkDifficulties);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetWalkDifficultyAsync")]
    public async Task<IActionResult> GetWalkDifficultyAsync([FromRoute] Guid id)
    {
        var walkDifficulty = await _walkDifficultyRepository.GetAsync(id);
        if (walkDifficulty == null) return NotFound();
        var walkDifficultyResponse = _mapper.Map<WalkDifficultyResponseDto>(walkDifficulty);
        return Ok(walkDifficultyResponse);
    }

    [HttpPost]
    [ActionName("AddWalkDifficultyAsync")]
    public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] AddWalkDifficultyRequestDto request)
    {
        var valid = this.ValidateWalkDifficultyRequestDto(request);
        if (!valid)
        {
            return BadRequest(ModelState);
        }
        var walkDifficulty = new WalkDifficulty()
        {
            Code = request.Code
        };
        var savedWalkDifficulty = await _walkDifficultyRepository.AddAsync(walkDifficulty);
        return CreatedAtAction(
            nameof(GetWalkDifficultyAsync),
            new { id = savedWalkDifficulty.Id },
            _mapper.Map<WalkDifficultyResponseDto>(savedWalkDifficulty)
        );
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [ActionName("DeleteWalkDifficultyAsync")]
    public async Task<IActionResult> DeleteWalkDifficultyAsync([FromRoute] Guid id)
    {
        var walkDifficulty = await _walkDifficultyRepository.GetAsync(id);
        if (walkDifficulty == null) return NotFound();
        walkDifficulty = await _walkDifficultyRepository.DeleteAsync(walkDifficulty);
        return Ok(_mapper.Map<WalkDifficultyResponseDto>(walkDifficulty));
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ActionName("UpdateWalkDifficultyAsync")]
    public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id,
                                                               [FromBody] AddWalkDifficultyRequestDto request)
    {
        var valid = this.ValidateWalkDifficultyRequestDto(request);
        if (!valid)
        {
            return BadRequest(ModelState);
        }
        var existingWalkDifficulty = await _walkDifficultyRepository.GetAsync(id);
        if (existingWalkDifficulty == null) return NotFound();
        var updatedWalkDifficulty = new WalkDifficulty()
        {
            Code = request.Code
        };
        updatedWalkDifficulty =
            await _walkDifficultyRepository.UpdateAsync(existingWalkDifficulty, updatedWalkDifficulty);
        return Ok(_mapper.Map<WalkDifficultyResponseDto>(updatedWalkDifficulty));
    }

    #region Private methods

    private bool ValidateWalkDifficultyRequestDto(AddWalkDifficultyRequestDto request)
    {
        if (request == null)
        {
            ModelState.AddModelError(
                nameof(request),
                "Request body is null"
            );
            return false;
        }
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            ModelState.AddModelError(
                nameof(request.Code),
                $"{nameof(request.Code)} cannot be null or empty or whitespace"
            );
        }

        return ModelState.ErrorCount == 0;
    }

    #endregion
}