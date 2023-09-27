using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(NZWalksDbContext dbContext, 
            IRegionRepository regionRepository,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        //GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
          var regionsDomain = await _regionRepository.GetAllAsync();

            // Map Domain Models to DTOs
           // var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);
            //Return DTOs
           // return Ok(regionsDto);

            // OR

            //Map Domain Models to DTOs and Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regionsDomain));
        }

        //GET SINGLE REGION ( Get Regions By ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
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
            
            return Ok(_mapper.Map<List<RegionDto>>(regionDomain));
        }

        //POST To Create New Region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            // Map or Convert DTO to Domain Model
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);

            regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

            // Map Domain model back to DTO
            var regionDto = _mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new {id = regionDto.Id }, regionDto);

        }

        // Update Region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
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
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));
        }

        // Delete Region
        // DELETE : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var regionDomainModel = await _regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Return deleted Region back
            // Map Domain Model to DTO
            return Ok(_mapper.Map<RegionDto>(regionDomainModel));

        }

    }
}
