﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Controllers;
using PetApi.Models;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetController
{
    [Fact]
    public async Task Should_return_app_pet_success_when_add_pet()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        var pet = new Pet(name: "dog", type: "cat", age: 10, price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        //when
        var response = await httpClient.PostAsync("api/addNewPet", stringContent);
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(readAsStringAsync);
        //then
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async Task Should_return_app_pet_success_when_get_pet()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("api/deleteAllPets");
        var pet = new Pet(name: "dog", type: "cat", age: 10, price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", stringContent);
        //when
        var response = await httpClient.GetAsync("api/getAllPets");
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<List<Pet>>(readAsStringAsync);
        //then
        Assert.Equal(pet, savedPet[0]);
    }

    [Fact]
    public async Task Should_return_selected_pet_when_get_pet_by_name()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("api/deleteAllPets");
        var pet = new Pet(name: "dog", type: "cat", age: 10, price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", stringContent);
        //when
        var response = await httpClient.GetAsync($"api/getPetByName?name={pet.Name}");
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(readAsStringAsync);
        //then
        Assert.Equal(pet, savedPet);
    }
    [Fact]
    public async Task Should_return_deleted_pet_when_buy_pet_by_name()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("api/deleteAllPets");
        var pet = new Pet(name: "dog", type: "cat", age: 10, price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var stringContent = new StringContent(serializeObject, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("api/addNewPet", stringContent);
        //when
        var response = await httpClient.DeleteAsync($"api/deleteByName?name={pet.Name}");
        response.EnsureSuccessStatusCode();
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        var responseResult = JsonConvert.DeserializeObject<Pet>(readAsStringAsync);
        
        //then
        Assert.Equal(pet, responseResult);
    }
}