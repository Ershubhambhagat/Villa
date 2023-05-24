using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Data;
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

        public villaController(ILogger<villaController> logger,ILogging logging)//
        {
            _logger = logger;
            _logging = logging;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {

            _logging.Log("error","Get All Villa");

            _logger.LogInformation("Get All Villa");
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{Id:int}", Name = "GetVill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVillasById(int Id)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == Id);
            if (villa == null)
            {
                return NotFound(Id + " Not found");
            }

            return Ok(villa);
        }

        [HttpPost]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTOs)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTOs.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "villa Already Exist!");
                return BadRequest(ModelState);
            }
            if (villaDTOs == null)
            {
                return BadRequest(villaDTOs);
            }
            if (villaDTOs.Id > 0)
            {
                return BadRequest("Id Should be Grater Then 0");
            }
            villaDTOs.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTOs);
            return CreatedAtRoute("GetVill", new { id = villaDTOs.Id }, villaDTOs);
        }

        [HttpDelete("{id:int}", Name = "Deletevilla")]
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                _logging.Log( " Id is not available", "error");
                _logger.LogError(id, " Id is not available");
                return BadRequest("your Id is 0");

            }
            var Villa = VillaStore.villaList.FirstOrDefault(m => m.Id == id);
            if (Villa == null)
            {
                return BadRequest("Villa is not available in database");
            }
            VillaStore.villaList.Remove(Villa);
            return NoContent();
        }
        [HttpPut("{Id:int}", Name = "Update Record")]
        public IActionResult UpdateVilla(int Id, [FromBody] VillaDTO villaDTO)
        {
            if (Id == null || Id != villaDTO.Id)
            {
                _logging.Log("error", "Id is not available");

                _logger.LogError(Id, "Id is not available");

                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(m => m.Id == Id);
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.SqFt = villaDTO.SqFt;
            return NoContent();
        }

        [HttpPatch("{Id:int}", Name = "UpdateVialla")]
        public ActionResult UpdatePartilaVilla(int Id, [FromBody] JsonPatchDocument<VillaDTO> patchDto)
        {
            if (patchDto == null || Id == 0)
                
            {
                _logging.Log("error", "Id is not available");
                _logger.LogError(Id, "is not available","error");

                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(m => m.Id == Id);
            if (villa == null)
            {
                return BadRequest();

            }
            patchDto.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }
    }


}
