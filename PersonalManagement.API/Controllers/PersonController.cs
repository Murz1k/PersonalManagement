using PersonalManagement.DAL;
using PersonalManagement.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace PersonalManagement.API.Controllers
{
    [Authorize]
    public class PersonController : ApiController
    {
        private PersonRepository _personRepository;

        public PersonController()
        {
            _personRepository = new PersonRepository();
        }

        // GET api/values
        public List<Person> Get()
        {
            return _personRepository.GetAllPerson();
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            _personRepository.DeletePerson(id);
        }

        // PUT api/values
        public void Put(Person person)
        {
            _personRepository.AddPerson(person);
        }
    }
}