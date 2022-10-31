using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetControllerTest
    {
        [Fact]
        public async void Should_add_new_pet_to_system_successfully()
        {
            // given
            // prepare HttpClient
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            // clear memory stored data
            await httpClient.DeleteAsync("/api/deleteAllPets");
            /*
             * Method: POST
             * URI: /api/addNewPet
             * Body:
             * {
             *    "name": "Kitty",
             *    "type": "cat",
             *    "color": "white",
             *    "price": 1000
             * }
             */
            // prepare post body
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            // serialize object to json string
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");

            // when
            // call API
            var response = await httpClient.PostAsync("/api/addNewPet", postBody);

            // then
            // check status code
            response.EnsureSuccessStatusCode();
            // read response body
            var responseBody = await response.Content.ReadAsStringAsync();
            // deserialize from json to object(Pet in this case)
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            // check response body
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pets_from_system_successfully()
        {
            // given
            // prepare HttpClient
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            // clear memory stored data
            await httpClient.DeleteAsync("/api/deleteAllPets");
            // prepare post request body: json format
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            // prepare data through API call
            await httpClient.PostAsync("/api/addNewPet", postBody);

            // when
            var response = await httpClient.GetAsync("/api/getAllPets?");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Single(allPets);
            Assert.Equal(pet, allPets[0]);
        }
    }
}