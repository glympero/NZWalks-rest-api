using System.Text.Json;
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
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger
        )
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegions()
        {
            //var regionsDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain)
            //{
            //    regionsDto.Add(new RegionDto
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImgUrl = regionDomain.RegionImgUrl
            //    });
            //}
            var regionsDomain = await regionRepository.GetRegionsAsync();

            logger.LogInformation("GetRegions called"); //this will not be visible if log level is warning (on program.cs)

            var regionsDto = mapper.Map<IEnumerable<RegionDto>>(regionsDomain);

            logger.LogInformation(
                $"Finished GetRegions request with : {JsonSerializer.Serialize(regionsDomain)}"
            );

            return Ok(regionsDto);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetRegion(Guid id)
        {
            // this can be used to search also for x.Code == "AUK" etc
            // var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            // find only works with primary key
            var regionDomain = await regionRepository.GetRegionByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImgUrl = regionDomain.RegionImgUrl
            //};
            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion(
            [FromBody] AddRegionRequestDto addRegionRequestDto
        )
        {
            //map or convert DTO to Domain Model
            //var regionDomain = new Region
            //{
            //    Code = addRegionRequestDto.Code,
            //    Name = addRegionRequestDto.Name,
            //    RegionImgUrl = addRegionRequestDto.RegionImgUrl
            //};

            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            regionDomain = await regionRepository.CreateRegionAsync(regionDomain);

            // map domain model back to dto because we only sending dto back to client

            var regionDto = mapper.Map<RegionDto>(regionDomain);
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImgUrl = regionDomain.RegionImgUrl
            //};
            return CreatedAtAction(nameof(GetRegion), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion(
            Guid id,
            [FromBody] UpdateRegionRequestDto updateRegionRequestDto
        )
        {
            //var regionDomain = new Region
            //{
            //    Id = id,
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImgUrl = updateRegionRequestDto.RegionImgUrl
            //};
            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);

            regionDomain = await regionRepository.UpdateRegionAsync(id, regionDomain);
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImgUrl = regionDomain.RegionImgUrl
            //};
            return Ok(regionDto);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            var regionDomain = await regionRepository.DeleteRegionAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImgUrl = regionDomain.RegionImgUrl
            //};
            return Ok(regionDto); //not necessary to return back the deleted object
        }
    }
}
