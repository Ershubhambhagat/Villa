using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villa.Data;
using Villa.Logging;
using Villa.Models.Dto;
using Villa.Repository.IRepository;

namespace Villa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class villaController : ControllerBase
    {
        private readonly ILogger<villaController> _logger;
        private readonly ILogging _logging;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;

        public villaController(ILogger<villaController> logger, ILogging logging, IMapper mapper,IVillaRepository villaRepository)//
        {
            _logger = logger;
            _logging = logging;
            _mapper = mapper;
            _dbVilla = villaRepository;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Dto.VillaDTO>>> GetVillas()
        {
            _logging.Log("error", "Get All Villa");
            _logger.LogInformation("Get All Villa");
            IEnumerable<Models.Villa> villaList=await _dbVilla.GetAllVillaAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }
        [HttpGet("{Id:int}", Name = "GetVill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillasById(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetVillaAsync(d => d.Id == Id);
            if (villa == null)
            {
                return NotFound(Id + " Not found");
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        [HttpPost]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            if (await _dbVilla.GetVillaAsync(n => n.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "villa Already Exist!");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            Models.Villa model = _mapper.Map<Models.Villa>(createDTO);
            //Models.Villa model = new()
            //{
            //    Name = createDTO.Name,
            //    Amenity = createDTO.Amenity,
            //    Details = createDTO.Details,
            //    Rate = createDTO.Rate,
            //    ImageURL = createDTO.ImageURL,
            //    Occupancy = createDTO.Occupancy,
            //    SqFt = createDTO.SqFt,
            //};

            await _dbVilla.CreateAsync(model);
            return Ok(model);
        }
        [HttpDelete("{id:int}", Name = "Deletevilla")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logging.Log(" Id is not available", "error");//Created One
                _logger.LogError(id, " Id is not available");
                return BadRequest("your Id is 0");

            }
            var Villa = await _dbVilla.GetVillaAsync(d => d.Id == id);
            if (Villa == null)
            {
                return BadRequest("Villa is not available in database");
            }
            _dbVilla.RemoveAsync(Villa);
            return NoContent();
        }
        [HttpPut("{Id:int}", Name = "Update Record")]
        public async Task<IActionResult> UpdateVilla(int Id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null && updateDTO.Id > 0)
            {
                _logging.Log("error", "Id is not available");

                _logger.LogError(Id, "Id is not available");

                return BadRequest();
            }
            var villa = await _dbVilla.GetVillaAsync(d => d.Id == Id,tracked:false);
            if (villa == null)
            {
                _logger.LogError(Id, "Id is not available in database");
                return BadRequest("villa not Present");
            }
            Models.Villa model=_mapper.Map<Models.Villa>(updateDTO);
            //Models.Villa model = new()
            //{
            //    Name = updateDTO.Name,
            //    Id = updateDTO.Id,
            //    Amenity = updateDTO.Amenity,
            //    Rate = updateDTO.Rate,
            //    SqFt = updateDTO.SqFt,
            //    ImageURL = updateDTO.ImageURL,
            //    Occupancy = updateDTO.Occupancy,
            //    Details = updateDTO.Details,
            //};
            _dbVilla.UpdateAsync(model);
            return Ok(model);
        }
        [HttpPatch("{Id:int}", Name = "UpdateVialla")]
        public async Task<IActionResult> UpdatePartilaVilla(int Id, [FromBody] JsonPatchDocument<VillaUpdateDTO> patchDto )
        {
            if (patchDto == null || Id == 0)
            {
                _logging.Log("error", "Id is not available");
                _logger.LogError(Id, "is not available", "error");
                return BadRequest();
            }
            var villa = await _dbVilla.GetVillaAsync(d => d.Id == Id);
            if (villa == null)
            {
                return BadRequest();
            }
            VillaUpdateDTO villaUpdate = _mapper.Map<VillaUpdateDTO>(villa);
            //VillaUpdateDTO villaUpdate = new()
            //{
            //    Name = villa.Name,
            //    Details = villa.Details,
            //    Rate = villa.Rate,
            //    SqFt = villa.SqFt,
            //    ImageURL = villa.ImageURL,
            //    Occupancy = villa.Occupancy,
            //    Amenity = villa.Amenity,
            //};
            patchDto.ApplyTo(villaUpdate, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Models.Villa model = _mapper.Map<Models.Villa>(villaUpdate);
            //Models.Villa model = new()
            //{
            //    Name = villaUpdate.Name,
            //    Details = villaUpdate.Details,
            //    Rate = villaUpdate.Rate,
            //    SqFt = villaUpdate.SqFt,
            //    ImageURL = villaUpdate.ImageURL,
            //    Occupancy = villaUpdate.Occupancy,
            //    Amenity = villaUpdate.Amenity,
            //};
           _dbVilla.UpdateAsync(model);
            return Ok(model);
        }
    }
}
