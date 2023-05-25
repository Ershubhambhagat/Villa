using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Villa.Data;
using Villa.Logging;
using Villa.Models.Dto;
namespace Villa.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class villaController : ControllerBase
    {
        private readonly ILogger<villaController> _logger;
        private readonly ILogging _logging;
        private readonly ApplicationDbContext _db;
        public villaController(ILogger<villaController> logger, ILogging logging, ApplicationDbContext db)//
        {
            _logger = logger;
            _logging = logging;
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Models.Dto.VillaDTO>>> GetVillas()
        {
            _logging.Log("error", "Get All Villa");
            _logger.LogInformation("Get All Villa");
            return Ok(await _db.Villas.ToListAsync());
            //return Ok(VillaStore.villaList);//for local store 
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
            //var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);//for local store 
            var villa = await _db.Villas.FirstOrDefaultAsync(d => d.Id == Id);
            if (villa == null)
            {
                return NotFound(Id + " Not found");
            }
            return Ok(villa);
        }
        [HttpPost]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDTO villaDTOs)
        {
            if (await _db.Villas.FirstOrDefaultAsync(n => n.Name.ToLower() == villaDTOs.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "villa Already Exist!");
                return BadRequest(ModelState);
            }
            if (villaDTOs == null)
            {
                return BadRequest(villaDTOs);
            }
            return CreatedAtRoute("GetVill", new { id = villaDTOs.Id }, villaDTOs);
            Models.Villa model = new()
            {
                Name = villaDTOs.Name,
                Amenity = villaDTOs.Amenity,
                Details = villaDTOs.Details,
                Rate = villaDTOs.Rate,
                ImageURL = villaDTOs.ImageURL,
                Occupancy = villaDTOs.Occupancy,
                SqFt = villaDTOs.SqFt,
            };
            await _db.Villas.AddAsync(model);
            await _db.SaveChangesAsync();
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
            var Villa = VillaStore.villaList.FirstOrDefault(m => m.Id == id);//for local store 
            var Villa = await _db.Villas.FirstOrDefaultAsync(d => d.Id == id);
            if (Villa == null)
            {
                return BadRequest("Villa is not available in database");
            }
            VillaStore.villaList.Remove(Villa);//for local store 
            _db.Remove(Villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{Id:int}", Name = "Update Record")]
        public async Task<IActionResult> UpdateVilla(int Id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (Id == null && Id > 0)
            {
                _logging.Log("error", "Id is not available");

                _logger.LogError(Id, "Id is not available");

                return BadRequest();
            }
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(d => d.Id == Id);
            if (villa == null)
            {
                _logger.LogError(Id, "Id is not available in database");
                return BadRequest("villa not Present");
            }
            Models.Villa model = new()
            {
                Name = villaDTO.Name,
                Id = villaDTO.Id,
                Amenity = villaDTO.Amenity,
                Rate = villaDTO.Rate,
                SqFt = villaDTO.SqFt,
                ImageURL = villaDTO.ImageURL,
                Occupancy = villaDTO.Occupancy,
                Details = villaDTO.Details,
            };
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return Ok(model);
        }
        [HttpPatch("{Id:int}", Name = "UpdateVialla")]
        public async Task<IActionResult> UpdatePartilaVilla(int Id, [FromBody] JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if (patchDto == null || Id == 0)
            {
                _logging.Log("error", "Id is not available");
                _logger.LogError(Id, "is not available", "error");
                return BadRequest();
            }
            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(d => d.Id == Id);
            if (villa == null)
            {
                return BadRequest();
            }
            VillaUpdateDTO villaDTOmodel = new()
            {
                Name = villa.Name,
                Details = villa.Details,
                Rate = villa.Rate,
                SqFt = villa.SqFt,
                ImageURL = villa.ImageURL,
                Occupancy = villa.Occupancy,
                Amenity = villa.Amenity,
            };
            patchDto.ApplyTo(villaDTOmodel, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            Models.Villa model = new()
            {
                Name = villaDTOmodel.Name,
                Details = villaDTOmodel.Details,
                Rate = villaDTOmodel.Rate,
                SqFt = villaDTOmodel.SqFt,
                ImageURL = villaDTOmodel.ImageURL,
                Occupancy = villaDTOmodel.Occupancy,
                Amenity = villaDTOmodel.Amenity,
            };
            _db.Villas.Update(model);
            await _db.SaveChangesAsync();
            return Ok(model);
        }
    }
}
