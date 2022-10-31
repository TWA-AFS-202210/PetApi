using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetApi.Model;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
    {
        private static List<Pet> pets = new List<Pet>();

        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("findPetByName")]
        public Pet FindPetByName([FromQuery] string name)
        {
            return pets.First(pet => pet.Name.Equals(name));
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }
    }
}