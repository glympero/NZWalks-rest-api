using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetWalks(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize
        )
        {
            var walks = await walkRepository.GetWalksAsync(
                filterOn,
                filterQuery,
                sortBy,
                isAscending ?? true,
                pageNumber ?? 1,
                pageSize ?? 10
            );

            throw new Exception("Something went wrong very badly");
            return Ok(mapper.Map<IEnumerable<WalkDto>>(walks));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWalk(Guid id)
        {
            var walk = await walkRepository.GetWalkByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walk));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomain = mapper.Map<Walk>(addWalkRequestDto);

            walkDomain = await walkRepository.CreateWalkAsync(walkDomain);

            return CreatedAtAction(
                nameof(GetWalk),
                new { id = walkDomain.Id },
                mapper.Map<WalkDto>(walkDomain)
            );
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk(
            Guid id,
            [FromBody] UpdateWalkRequestDto updateWalkRequestDto
        )
        {
            var walk = await walkRepository.GetWalkByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDomain = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomain = await walkRepository.UpdateWalkAsync(id, walkDomain);
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walk = await walkRepository.GetWalkByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            walk = await walkRepository.DeleteWalkAsync(id);
            return Ok(mapper.Map<WalkDto>(walk));
        }
    }
}
