using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using People.Api.Data.Entities;

namespace People.Api.Services
{
    public interface IApiRepository
    {
        IEnumerable<Person> GetAllPeople();
        Person GetPerson(int personId);
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void DeletePerson(Person person);
        bool PersonExists(int personId);
        IEnumerable<Job> GetPersonJobs(int personId, bool onlyPresent = false);
        Job GetJobForPerson(int personId, int jobId);
        void AddJobForPerson(Job job);
        void UpdateJobForPerson(Job job);
        void DeleteJob(Job job);
        bool PersonJobExists(int personId, int jobId);
        bool Save();
    }
}
