﻿using Microsoft.AspNetCore.Mvc.Testing;
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
            AddOnePet(client, pet);
            var pet1 = new Pet("Bob", "dog", "white", 2000);
            AddOnePet(client, pet1);

            var response = await client.GetAsync("/api/getOnePetByName?name=kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var petFound = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.NotNull(petFound);
        }

        public void AddOnePet(HttpClient client, Pet pet)
        {
            var serialzeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serialzeObject, Encoding.UTF8, "application/json");
            client.PostAsync("/api/addNewPet", postBody);
        }
    }
}
