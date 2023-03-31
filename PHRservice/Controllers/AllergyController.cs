using BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllergyController : ControllerBase
    {
        ILogic _logic;
        public AllergyController(ILogic logic)
        {
            _logic = logic;
        }

        [HttpPost("AddAllergyRecords")]
        public ActionResult Add([FromBody] Models.Patient_Allergy r)
        {
            try
            {
                r.Id = Guid.NewGuid();
                var add = _logic.AddAllergyReport(r);
                return Ok(add);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut("modifyAllergy/{Id}")]
        public ActionResult UpdateAllergyRecord([FromRoute] string Id, [FromBody] Models.Patient_Allergy r)
        {
            try
            {
                if (Id != null)
                {
                    _logic.UpdatePA(Id, r);
                    return Ok(r);
                }
                else
                    return BadRequest($"something wrong with {Id}, please try again!");
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
