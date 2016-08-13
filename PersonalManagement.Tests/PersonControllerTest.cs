using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PersonalManagement.Model;
using PersonalManagement.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PersonalManagement.Tests
{
    [TestClass]
    public class PersonControllerTest
    {
        private MockRepository _mockRepository;
        private HttpClient _client;
        private List<Person> _personList;
        private Person _selectedPerson;

        [TestInitialize]
        public void Init()
        {
            _mockRepository = new MockRepository();
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:60007/");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TestMethod]
        public void GetPersons()
        {
            //Arrange
            var persons =  _mockRepository.GetAllPerson();

            //act
            var result =  _client.GetAsync("api/person").Result;
            if (result.IsSuccessStatusCode)
            {
                _personList = JsonConvert.DeserializeObject<List<Person>>(result.Content.ReadAsStringAsync().Result);
            }

            //accert
            CollectionAssert.AreEqual(persons, _personList);
        }
    }
}
