using Newtonsoft.Json;
using PersonalManagement.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Input;

namespace PersonalManagement.WPF.ViewModel
{
    public class MainViewModel : BindableBase
    {
        private static string token;
        private const string APP_PATH = "http://localhost:60007/";
        private HttpClient _client;
        private IEnumerable<Person> _personList;
        private Person _selectedPerson;

        public MainViewModel()
        {
            LoadCommands();
            Dictionary<string, string> tokenDictionary = GetTokenDictionary("Max@gmail.com", "1234");
            token = tokenDictionary["access_token"];
            ShowPersons();
        }
        static HttpClient CreateClient(string accessToken = "")
        {
            var client = new HttpClient();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return client;
        }
        static Dictionary<string, string> GetTokenDictionary(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ),
                    new KeyValuePair<string, string>( "username", userName ),
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsync(APP_PATH + "/Token", content).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                // Десериализация полученного JSON-объекта
                Dictionary<string, string> tokenDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                return tokenDictionary;
            }
        }

        private async void ShowPersons()
        {
            _client = CreateClient(token);
                HttpResponseMessage resp = await _client.GetAsync(APP_PATH + "api/person");

                if (resp.IsSuccessStatusCode)
                {
                    PersonList = JsonConvert.DeserializeObject<IEnumerable<Person>>(resp.Content.ReadAsStringAsync().Result);
                }
        }

        private void LoadCommands()
        {
            AddPersonCommand = new DelegateCommand(AddPerson);
            DeletePersonCommand = new DelegateCommand(DeletePerson);
        }

        private void AddPerson()
        {
            var jsonString = JsonConvert.SerializeObject(SelectedPerson);
            var stringContent = new StringContent(jsonString);
            var result = _client.PutAsync(APP_PATH + "api/person", stringContent).Result;
            Console.WriteLine(result.ReasonPhrase);
            ShowPersons();
        }

        private void DeletePerson()
        {
             var result = _client.DeleteAsync(APP_PATH + "api/person/" + SelectedPerson.Id).Result;
             ShowPersons();
        }

        public ICommand AddPersonCommand { get; set; }

        public ICommand DeletePersonCommand { get; set; }

        public IEnumerable<Person> PersonList
        {
            get { return _personList; }
            set
            {
                SetProperty(ref _personList, value);
                OnPropertyChanged(() => PersonList);
            }
        }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                SetProperty(ref _selectedPerson, value);
                OnPropertyChanged(() => SelectedPerson);
            }
        }
    }
}
