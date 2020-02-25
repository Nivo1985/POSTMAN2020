using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using People.Api.Data.Entities;
using People.Api.Data.Models;
using People.Api.Services;

namespace People.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IApiRepository _repo;
        private readonly IMapper _mapper;

        public JobController(IApiRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{personId}", Name = "GetPersonJobs")]
        public ActionResult<IEnumerable<JobModel>> GetPersonJobs(string personId, bool onlyPresent = false)
        {
            if (!int.TryParse(personId, out var id))
            {
                return BadRequest("Id in wrong format. Int expected.");
            }

            if (!_repo.PersonExists(id))
            {
                return NotFound("Person with given id not found");
            }

            return Ok(_mapper.Map<IEnumerable<JobModel>>(_repo.GetPersonJobs(id, onlyPresent)));
        }

        [HttpGet("{personId}/{jobId}", Name = "GetPersonJob")]
        public ActionResult<JobModel> GetPersonJob(int personId, int jobId)
        {
            var job = _repo.GetJobForPerson(personId, jobId);

            if (job == null)
            {
                return NotFound("Job with given id not found for given person Id");
            }

            return Ok(_mapper.Map<JobModel>(job));
        }

        [HttpPost("{personId}")]
        public IActionResult AddJobToPerson(int personId, [FromBody] Job job)
        {
            if (!_repo.PersonExists(personId))
            {
                return NotFound("Person with given id not found");
            }

            job.PersonId = personId;

            _repo.AddJobForPerson(job);
            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during job creation");
            }

            return CreatedAtRoute("GetPersonJob", new { personId = job.PersonId, jobId=job.JobId }, _mapper.Map<JobModel>(job));
        }

        [HttpPut("{personId}/{jobId}")]
        public IActionResult UpdateJobOfPerson(int personId, int jobId, [FromBody] Job job)
        {
            if (!_repo.PersonExists(personId))
            {
                return NotFound("Person with given id not found");
            }

            if (!_repo.PersonJobExists(personId, jobId))
            {
                return NotFound("Job with given id not found for given person Id");
            }

            job.JobId = jobId;
            job.PersonId = personId;

            _repo.UpdateJobForPerson(job);
            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during job updating");
            }

            return CreatedAtRoute("GetPersonJob", new { personId = job.PersonId, jobId = job.JobId }, _mapper.Map<JobModel>(job));
        }

        [HttpDelete("{personId}/{jobId}")]
        public IActionResult DeleteJobForPerson(int personId, int jobId)
        {
            if (!_repo.PersonExists(personId))
            {
                return NotFound("Person with given id not found");
            }

            if (!_repo.PersonJobExists(personId, jobId))
            {
                return NotFound("Job with given id not found for given person Id");
            }

            _repo.DeleteJob(_repo.GetJobForPerson(personId, jobId));

            if (!_repo.Save())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal error during job deleting");
            }

            return NoContent();
        }
    }
}