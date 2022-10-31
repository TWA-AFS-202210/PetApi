using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System.Collections.Generic;

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

        [HttpGet("getOnePetByName")]
        public Pet GetOnePetByName(string name)
        {
            foreach (var item in pets)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }

            return null;
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpDelete("deleteSelledPet")]
        public List<Pet> DeleteSelledPet(Pet pet)
        {
            if (pets.Contains(pet))
            {
                pets.Remove(pet);
            }

            return pets;
        }
    }
}
