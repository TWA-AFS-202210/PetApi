using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Xunit;

namespace PetApiTest
{
    public class PetControllerTest
    {
        [Fact]
        public async void Should_add_new_pet_to_system_successfully()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            //when
            var pet = new Pet("kitty", "cat", "white", 1000);
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/addNewPet", postBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pet_to_system_successfully()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");
            //when
            var pet = new Pet("kitty", "cat", "white", 1000);
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            await client.PostAsync("/api/addNewPet", postBody);

            var response = await client.GetAsync("/api/getAllPets");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async void Should_get_one_pet_by_name()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");
            //when
            var pet = new Pet("kitty", "cat", "white", 1000);
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            await client.PostAsync("/api/addNewPet", postBody);

            var response = await client.GetAsync("/api/getOnePetByName?name=kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petFound = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.NotNull(petFound);
        }

        [Fact]
        public async void Should_remove_the_pet_when_sell()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            await client.PostAsync("/api/addNewPet", postBody);

            //when
            var response = await client.DeleteAsync("/api/deleteSelledPet?name=kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(0, allPets.Count);
        }

        [Fact]
        public async void Should_return_price_after_modify()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            await client.PostAsync("/api/addNewPet", postBody);

            //when
            pet.Price = 2000;
            var serialzeObject1 = JsonConvert.SerializeObject(pet);
            var postBody1 = new StringContent(serialzeObject1, Encoding.UTF8, "application/json");
            var response = await client.PatchAsync("/api/editPrice", postBody1);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petEdited = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(2000, petEdited.Price);
        }

        [Fact]
        public async void Should_return_all_same_type_pet_when_find_by_type()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");

            var pet = new Pet("kitty", "cat", "white", 1000);
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            await client.PostAsync("/api/addNewPet", postBody);

            //when
            var response = await client.GetAsync("/api/getByType?type=cat");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allCat = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(1, allCat.Count);
        }

        [Fact]
        public async void Should_return_all_pets_between_prices_range()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");

            var pet1 = new Pet("kitty", "cat", "white", 1000);
            var pet2 = new Pet("Bob", "cat", "white", 1500);
            var pet3 = new Pet("Ops", "cat", "white", 2000);
            var petList = new List<Pet> { pet1, pet2, pet3 };
            foreach (Pet pet in petList)
            {
                var serialzeObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/addNewPet", postBody);
            }

            //when
            var response = await client.GetAsync("/api/getPetsByPriceRange?min=500&&max=1700");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, allPets.Count);
        }

        [Fact]
        public async void Should_return_all_pets_with_color_searched()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var client = application.CreateClient();
            await client.DeleteAsync("/api/deleteAllPets");

            var pet1 = new Pet("kitty", "cat", "white", 1000);
            var pet2 = new Pet("Bob", "cat", "pink", 1500);
            var pet3 = new Pet("Ops", "dog", "white", 2000);
            var petList = new List<Pet> { pet1, pet2, pet3 };
            foreach (Pet pet in petList)
            {
                var serialzeObject = JsonConvert.SerializeObject(pet);
                var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
                await client.PostAsync("/api/addNewPet", postBody);
            }

            //when
            var response = await client.GetAsync("/api/getPetsByColor?color=white");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(2, allPets.Count);
        }
    }
}
