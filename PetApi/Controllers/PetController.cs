using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using PetApi.Models;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : Controller
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

        [HttpGet("getPetByName")]
        public Pet FindPetByName([FromQuery] string name)
        {
            return pets.Find(item=> item.Name == name);
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpDelete("deleteByName")]
        public Pet DeleteByName([FromQuery] string name)
        {
            Pet pet = new Pet();
            pet = pets.Find(item => item.Name == name);
            pets.Remove(pets.Find(item => item.Name == name));
            return pet;
        }

        [HttpPut("modifyByName")]
        public Pet ModifyByName(Pet pet)
        {
            pets.Find(item => item.Equals(pet)).Price = pet.Price;
            return pets.Find(item => item.Name == pet.Name);
        }

        [HttpGet("getPetByType")]
        public List<Pet> GetByType([FromQuery] string type)
        {
            return pets.FindAll(item => item.PetType.Equals(type));
        }

        [HttpGet("getPetByRange")]
        public List<Pet> GetByType([FromQuery] int upper, [FromQuery] int lower)
        {
            return pets.FindAll(item => item.Price > lower && item.Price < upper);
        }

        [HttpGet("getPetByAge")]
        public List<Pet> GetByType([FromQuery] int age)
        {
            return pets.FindAll(item => item.Age == age);
        }
    }
}
