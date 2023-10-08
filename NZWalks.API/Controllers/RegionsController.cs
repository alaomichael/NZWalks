using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksDbContext dbContext, 
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //GET ALL REGIONS
        // GET: https://localhost:portnumber/api/v1/regions
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllV1() 
        {
            // try {
            // throw custom exception
            //throw new Exception("This is a custom exception.");
            // Get Data From Database - Domain models
            var regionsDomain = await _regionRepository.GetAllAsync();

            // Log response data
            _logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}"); // Convert object to json data
                               
                //Map Domain Models to DTOs and Return DTOs
                return Ok(_mapper.Map<List<RegionDtoV1>>(regionsDomain));
            //}
            //catch (Exception ex)
            //{
            //    //_logger.LogInformation("GetAllRegions Action Method was invoked");
            //    //_logger.LogWarning("This is a warning log");
            //    _logger.LogError(ex,ex.Message);
            //    throw;
            //}
            
        }

        //GET ALL REGIONS
        // GET: https://localhost:portnumber/api/v2/regions
        [MapToApiVersion("2.0")]
        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllV2()
        {
            var regionsDomain = await _regionRepository.GetAllAsync();

            // Log response data
            _logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}"); // Convert object to json data

            //Map Domain Models to DTOs and Return DTOs
            return Ok(_mapper.Map<List<RegionDtoV2>>(regionsDomain));
            
        }


        //GET SINGLE REGION ( Get Regions By ID)
        // GET: https://localhost:portnumber/api/v1/regions/{id}
        [MapToApiVersion("1.0")]
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetByIdV1([FromRoute] Guid id)
        {
            // Get Data From Database - Domain Model
            // Get Region Domain Model From Database

            //var region = _dbContext.Regions.Find(id);
            //var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id); // Linq method

            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map/Convert Region Domain Model to Region DTO
            
            return Ok(_mapper.Map<List<RegionDtoV1>>(regionDomain));
        }

        //GET SINGLE REGION ( Get Regions By ID)
        // GET: https://localhost:portnumber/api/v2/regions/{id}
        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetByIdV2([FromRoute] Guid id)
        {
            // Get Data From Database - Domain Model
            // Get Region Domain Model From Database

            //var region = _dbContext.Regions.Find(id);
            //var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id); // Linq method

            var regionDomain = await _regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }
            // Map/Convert Region Domain Model to Region DTO

            return Ok(_mapper.Map<List<RegionDtoV2>>(regionDomain));
        }


        //POST To Create New Region
        // POST: https://localhost:portnumber/api/v1/regions
        [MapToApiVersion("1.0")]
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateV1([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = _mapper.Map<RegionDtoV1>(regionDomainModel);

            return CreatedAtAction(nameof(GetByIdV1), new {id = regionDto.Id }, regionDto);

        }


        //POST To Create New Region
        // POST: https://localhost:portnumber/api/v2/regions
        [MapToApiVersion("2.0")]
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateV2([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = _mapper.Map<RegionDtoV2>(regionDomainModel);

            return CreatedAtAction(nameof(GetByIdV2), new { id = regionDto.Id }, regionDto);

        }


        // Update Region
        // PUT: https://localhost:portnumber/api/v1/regions/{id}
        [MapToApiVersion("1.0")]
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateV1([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

            // Check if region exists
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel == null)
            {
                return NotFound();
            } 

            // Convert Domain Model to DTO
            return Ok(_mapper.Map<RegionDtoV1>(regionDomainModel));
        }

        // Update Region
        // PUT: https://localhost:portnumber/api/v2/regions/{id}
        [MapToApiVersion("2.0")]
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateV2([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // Map DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDto);

            // Check if region exists
            regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            return Ok(_mapper.Map<RegionDtoV2>(regionDomainModel));
        }


        // Delete Region
        // DELETE : https://localhost:portnumber/api/v1/regions/{id}
        [MapToApiVersion("1.0")]
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteV1([FromRoute] Guid id)
        {
           var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Return deleted Region back
            // Map Domain Model to DTO
            return Ok(_mapper.Map<RegionDtoV1>(regionDomainModel));

        }

        // Delete Region
        // DELETE : https://localhost:portnumber/api/v2/regions/{id}
        [MapToApiVersion("2.0")]
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteV2([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Return deleted Region back
            // Map Domain Model to DTO
            return Ok(_mapper.Map<RegionDtoV2>(regionDomainModel));

        }

    }
}
