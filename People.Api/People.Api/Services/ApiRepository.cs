using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using People.Api.Data;
using People.Api.Data.Entities;

namespace People.Api.Services
{
    public class ApiRepository: IApiRepository
    {
        private readonly PeopleContext _peopleContext;

        public ApiRepository(PeopleContext peopleContext)
        {
            _peopleContext = peopleContext;
        }

        public IEnumerable<Person> GetAllPeople()
        {
            var result = _peopleContext.People.ToList();
            result.ForEach(x=> x.NumberOfJobs = _peopleContext.Jobs.Count(j => j.PersonId ==x.PersonId));
            return result;
        }

        public Person GetPerson(int personId)
        {
            var result = _peopleContext.People.Include("Jobs.Period").FirstOrDefault(p => p.PersonId == personId);
            if (result != null)
            {
                result.NumberOfJobs = result.Jobs.Count;
            }

            return result;
        }

        public void AddPerson(Person person)
        {
            _peopleContext.Add(person);
        }

        public void UpdatePerson(Person person)
        {
            _peopleContext.Update(person);
        }

        public void DeletePerson(Person person)
        {
            _peopleContext.Jobs.Where(j => j.PersonId == person.PersonId).ToList().ForEach(DeleteJob);

            _peopleContext.Remove(person);
        }

        public bool PersonExists(int personId)
        {
            return _peopleContext.People.Any(p => p.PersonId == personId);
        }

        public IEnumerable<Job> GetPersonJobs(int personId, bool onlyPresent = false)
        {
            var result = _peopleContext.Jobs.Include(j => j.Period).Where(j => j.PersonId == personId);
            if (onlyPresent)
            {
                result = result.Where(j => !j.Period.End.HasValue);
            }

            return result.ToList();
        }

        public Job GetJobForPerson(int personId, int jobId)
        {
            return _peopleContext.Jobs.Include(j=>j.Period).FirstOrDefault(j => j.JobId == jobId && j.PersonId == personId);
        }

        public void AddJobForPerson(Job job)
        {
            _peopleContext.Add(job);
        }

        public void UpdateJobForPerson(Job job)
        {
            _peopleContext.Update(job);
        }

        public void DeleteJob(Job job)
        {
            _peopleContext.Remove(job);
            _peopleContext.Remove(job.Period);
        }

        public bool PersonJobExists(int personId, int jobId)
        {
            return _peopleContext.Jobs.Any(j => j.JobId == jobId && j.PersonId == personId);
        }

        public bool Save()
        {
            return _peopleContext.SaveChanges() >= 0;
        }
    }
}
