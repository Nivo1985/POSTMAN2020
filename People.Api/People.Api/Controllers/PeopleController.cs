using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using People.Api.Data.Entities;
using People.Api.Data.Models;
using People.Api.Services;
using People.Api.Utility;

namespace People.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly IApiRepository _repo;
        private readonly IMapper _mapper;

        public PeopleController(IApiRepository repo, IMapper mapper)
        {
            this._repo = repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonModel>> GetPeople()
        {
            var result = _repo.GetAllPeople();
            return Ok(_mapper.Map<IEnumerable<PersonModel>>(result));
        }

        [HttpGet("{personId}", Name = "GetPerson")]
        public ActionResult<Person> GetPerson(string personId)
        {
            if (!int.TryParse(personId, out var id))
            {
                return BadRequest("Id in wrong format. Int expected.");
            }

            if(!_repo.PersonExists(id))
            {
                return NotFound("Person with given id not found");
            }

            return Ok(_mapper.Map<PersonModel>(_repo.GetPerson(id)));
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest("No object provided for post");
            }

            if (person.PersonId != 0)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Person can't have Id provided");
            }

            if (person.DateOfDeath < person.DateOfBirth)
            {
                ModelState.AddModelError("Dates conflict","Death date priors birth date");
            }

            if (!ModelState.IsValid)
            {
                return new EntityValidationObjectResult(ModelState);
            }

            _repo.AddPerson(person);
            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during person creation");
            }

            return CreatedAtRoute("GetPerson", new {personId = person.PersonId}, _mapper.Map<PersonModel>(person));
        }

        [HttpPut("{personId}")]
        public IActionResult UpdatePerson(string personId, [FromBody] Person person)
        {
            if (!int.TryParse(personId, out var id))
            {
                return BadRequest("Id in wrong format. Int expected.");
            }

            if (!_repo.PersonExists(id))
            {
                return NotFound("Person with given id not found");
            }

            if (person == null)
            {
                return BadRequest("No object provided for post");
            }

            if (person.DateOfDeath < person.DateOfBirth)
            {
                ModelState.AddModelError("Dates conflict", "Death date priors birth date");
            }

            if (!ModelState.IsValid)
            {
                return new EntityValidationObjectResult(ModelState);
            }

            person.PersonId = id;

            _repo.UpdatePerson(person);
            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during person creation");
            }

            return CreatedAtRoute("GetPerson", new { personId = person.PersonId }, _mapper.Map<PersonModel>(person));
        }

        [HttpPatch("{personId}")]
        public IActionResult PatchPerson(string personId, [FromBody] JsonPatchDocument<Person> patchDoc)
        {
            if (!int.TryParse(personId, out var id))
            {
                return BadRequest("Id in wrong format. Int expected.");
            }

            var person = _repo.GetPerson(id);

            if (person == null)
            {
                return NotFound("Person with given id not found");
            }

            patchDoc.ApplyTo(person, ModelState);

            if (person.DateOfDeath < person.DateOfBirth)
            {
                ModelState.AddModelError("Dates conflict", "Death date priors birth date");
            }

            if (!ModelState.IsValid)
            {
                return new EntityValidationObjectResult(ModelState);
            }

            _repo.UpdatePerson(person);
            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during person creation");
            }

            return CreatedAtRoute("GetPerson", new { personId = person.PersonId }, _mapper.Map<PersonModel>(person));
        }

        [HttpDelete("{personId}")]
        public IActionResult DeletePerson(string personId)
        {
            if (!int.TryParse(personId, out var id))
            {
                return BadRequest("Id in wrong format. Int expected.");
            }

            var person = _repo.GetPerson(id);

            if (person == null)
            {
                return NotFound("Person with given id not found");
            }

            _repo.DeletePerson(person);
            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during person removal");
            }

            return NoContent();
        }
    }
}