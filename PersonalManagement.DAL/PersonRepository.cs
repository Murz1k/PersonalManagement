using MongoDB.Bson;
using MongoDB.Driver;
using PersonalManagement.Model;
using System.Collections.Generic;

namespace PersonalManagement.DAL
{
    public interface IPersonRepository
    {
        void AddPerson(Person person);
        void DeletePerson(string id);
        List<Person> GetAllPerson();
    }
    public class PersonRepository : IPersonRepository
    {
        private IMongoCollection<BsonDocument> _collection;
        private List<Person> _personList;

        public PersonRepository()
        {
            var _client = new MongoClient();
            var _database = _client.GetDatabase("test");
            _collection = _database.GetCollection<BsonDocument>("Persons");
        }
        public void AddPerson(Person person)
        {
            person.Id = ObjectId.GenerateNewId().ToString();
            var bsonPerson = person.ToBsonDocument();
            _collection.InsertOne(bsonPerson);
        }

        public void DeletePerson(string id)
        {
            _collection.FindOneAndDelete(item => item["_id"] == id);
        }

        public List<Person> GetAllPerson()
        {
            _personList = new List<Person>();
            using (var cursor = _collection.FindSync(new BsonDocument()))
            {
                if (cursor.MoveNext())
                {
                    var personCollection = cursor.Current;
                    foreach (var person in personCollection)
                    {
                        _personList.Add(new Person
                        {
                            Id = person["_id"].AsString,
                            FirstName = person["FirstName"].AsString,
                            SecondName = person["SecondName"].AsString,
                            PhoneNumber = person["PhoneNumber"].AsString
                        });
                    }
                }
            }
            return _personList;
        }
    }
}