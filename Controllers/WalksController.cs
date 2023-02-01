using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WalksController : Controller
{
    private readonly IWalkRepository _walkRepository;
    private readonly IMapper _mapper;
    private readonly IRegionRepository _regionRepository;
    private readonly IWalkDifficultyRepository _walkDifficultyRepository;
    
    public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
    {
        this._walkRepository = walkRepository;
        this._mapper = mapper;
        this._regionRepository = regionRepository;
        this._walkDifficultyRepository = walkDifficultyRepository;
    }

    [HttpGet]
    [ActionName("GetAllWalksAsync")]
    public async Task<IActionResult> GetAllWalksAsync()
    {
        var walks = await _walkRepository.GetAllAsync();
        return Ok(_mapper.Map<List<WalkResponseDto>>(walks));
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetWalkAsync")]
    public async Task<IActionResult> GetWalkAsync([FromRoute] Guid id)
    {
        var walk = await _walkRepository.GetAsync(id);
        if (walk == null) return NotFound();
        var walkResponse = _mapper.Map<WalkResponseDto>(walk);
        return Ok(walkResponse);
    }

    [HttpPost]
    [ActionName("AddWalkAsync")]
    public async Task<IActionResult> AddWalkAsync([FromBody] WalkRequestDto walkRequestDto)
    {
        var valid = await this.ValidateWalkRequestAsync(walkRequestDto);
        if (!valid)
        {
            return BadRequest(ModelState);
        }
        var walk = _mapper.Map<Walk>(walkRequestDto);
        var savedWalk = await _walkRepository.AddAsync(walk);
        return CreatedAtAction(nameof(GetWalkAsync), new { id = savedWalk.Id }, _mapper.Map<WalkResponseDto>(savedWalk));
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [ActionName("DeleteWalkAsync")]
    public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
    {
        var walk = await _walkRepository.GetAsync(id);
        if (walk == null) return NotFound();
        walk = await _walkRepository.DeleteAsync(walk);
        return Ok(_mapper.Map<WalkResponseDto>(walk));
    }

    [HttpPut]
    [Route("{id:guid}")]
    [ActionName("UpdateWalkAsync")]
    public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] WalkRequestDto walkRequestDto)
    {
        var valid = await this.ValidateWalkRequestAsync(walkRequestDto);
        if (!valid)
        {
            return BadRequest(ModelState);
        }
        var existingWalk = await _walkRepository.GetAsync(id);
        if (existingWalk == null) return NotFound();
        var updatedWalk = _mapper.Map<Walk>(walkRequestDto);
        updatedWalk = await _walkRepository.UpdateAsync(existingWalk, updatedWalk);
        return Ok(_mapper.Map<WalkResponseDto>(updatedWalk));
    }

    #region Private methods

    private async Task<bool> ValidateWalkRequestAsync(WalkRequestDto walkRequestDto)
    {
        if (walkRequestDto == null)
        {
            ModelState.AddModelError(
                nameof(walkRequestDto),
                "Add Walk Request cannot be null"
            );
            return false;
        }

        if (string.IsNullOrWhiteSpace(walkRequestDto.Name))
        {
            ModelState.AddModelError(
                nameof(walkRequestDto.Name), 
                $"{nameof(walkRequestDto.Name)} is required"
            );
        }

        if (walkRequestDto.Length <= 0)
        {
            ModelState.AddModelError(
                nameof(walkRequestDto.Length), 
                $"{nameof(walkRequestDto.Length)} must be greater than zero"
            );
        }

        var region = await _regionRepository.GetAsync(walkRequestDto.RegionId);
        if (region == null)
        {
            ModelState.AddModelError(
                nameof(walkRequestDto.RegionId),
                $"{nameof(walkRequestDto.RegionId)} is invalid"
            );
        }

        var walkDifficulty = await _walkDifficultyRepository.GetAsync(walkRequestDto.WalkDifficultyId);
        if (walkDifficulty == null)
        {
            ModelState.AddModelError(
                nameof(walkRequestDto.WalkDifficultyId),
                $"{nameof(walkRequestDto.WalkDifficultyId)} is invalid"
            );
        }

        return ModelState.ErrorCount == 0;
    }
    
    #endregion
}