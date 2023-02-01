using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using Region = NZWalks.API.Models.Domain.Region;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
            var regions = await _regionRepository.GetAllAsync();
            return Ok(_mapper.Map<List<RegionResponseDto>>(regions));
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetRegionAsync([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetAsync(id);
            if (region == null) return NotFound();
            var regionResponseDto = _mapper.Map<RegionResponseDto>(region);
            return Ok(regionResponseDto);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegionAsync([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // var valid = ValidateAddRegionAsync(addRegionRequestDto);
            // if (!valid)
            // {
            //     return BadRequest(ModelState);
            // }
            var region = _mapper.Map<Region>(addRegionRequestDto);
            var savedRegion = await _regionRepository.AddAsync(region);
            var regionResponseDto = _mapper.Map<RegionResponseDto>(savedRegion);
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionResponseDto.Id }, regionResponseDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ActionName("DeleteRegionAsync")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetAsync(id);
            if (region == null) return NotFound();
            await _regionRepository.DeleteAsync(region);
            return Ok(_mapper.Map<RegionResponseDto>(region));
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ActionName("UpdateRegionAsync")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var valid = ValidateUpdateRegionAsync(updateRegionRequestDto);
            if (!valid)
            {
                return BadRequest(ModelState);
            }
            var existingRegion = await _regionRepository.GetAsync(id);
            if (existingRegion == null) return NotFound();
            var updatedRegion = _mapper.Map<Region>(updateRegionRequestDto);
            updatedRegion = await _regionRepository.UpdateAsync(existingRegion, updatedRegion);
            var responseDto = _mapper.Map<RegionResponseDto>(updatedRegion);
            return Ok(responseDto);
        }

        #region Private methods

        private bool ValidateAddRegionAsync(AddRegionRequestDto addRegionRequestDto)
        {
            if (addRegionRequestDto == null)
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto), 
                    "Add Region Data is required"
                );
            }
            
            if (string.IsNullOrWhiteSpace(addRegionRequestDto.Code))
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto.Code), 
                    $"{nameof(addRegionRequestDto.Code)} cannot be null or empty or whitespace"
                );
            }

            if (string.IsNullOrWhiteSpace(addRegionRequestDto.Name))
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto.Name), 
                    $"{nameof(addRegionRequestDto.Name)} cannot be null or empty or whitespace"
                );
            }

            if (addRegionRequestDto.Area <= 0)
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto.Area), 
                    $"{nameof(addRegionRequestDto.Area)} must be greater than zero"
                );
            }
            
            if (addRegionRequestDto.Lat <= 0)
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto.Lat), 
                    $"{nameof(addRegionRequestDto.Lat)} must be greater than zero"
                );
            }
            
            if (addRegionRequestDto.Long <= 0)
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto.Long), 
                    $"{nameof(addRegionRequestDto.Long)} must be greater than zero"
                );
            }
            
            if (addRegionRequestDto.Population < 0)
            {
                ModelState.AddModelError(
                    nameof(addRegionRequestDto.Population), 
                    $"{nameof(addRegionRequestDto.Population)} cannot be less than zero"
                );
            }

            return ModelState.ErrorCount == 0;
        }

        private bool ValidateUpdateRegionAsync(UpdateRegionRequestDto updateRegionRequestDto)
        { 
            if (updateRegionRequestDto == null)
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto), 
                    "Add Region Data is required"
                );
            }
            
            if (string.IsNullOrWhiteSpace(updateRegionRequestDto.Code))
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto.Code), 
                    $"{nameof(updateRegionRequestDto.Code)} cannot be null or empty or whitespace"
                );
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequestDto.Name))
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto.Name), 
                    $"{nameof(updateRegionRequestDto.Name)} cannot be null or empty or whitespace"
                );
            }

            if (updateRegionRequestDto.Area <= 0)
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto.Area), 
                    $"{nameof(updateRegionRequestDto.Area)} must be greater than zero"
                );
            }
            
            if (updateRegionRequestDto.Lat <= 0)
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto.Lat), 
                    $"{nameof(updateRegionRequestDto.Lat)} must be greater than zero"
                );
            }
            
            if (updateRegionRequestDto.Long <= 0)
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto.Long), 
                    $"{nameof(updateRegionRequestDto.Long)} must be greater than zero"
                );
            }
            
            if (updateRegionRequestDto.Population < 0)
            {
                ModelState.AddModelError(
                    nameof(updateRegionRequestDto.Population), 
                    $"{nameof(updateRegionRequestDto.Population)} cannot be less than zero"
                );
            }

            return ModelState.ErrorCount == 0;
        }
        
        #endregion
    }
}
