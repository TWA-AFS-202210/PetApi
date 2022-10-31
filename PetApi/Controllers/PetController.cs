using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System.Collections.Generic;
using System.Xml.Linq;

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
            return pets.Find(x => x.Name == name);
        }

        [HttpGet("getByType")]
        public List<Pet> GetByType(string type)
        {
            return pets.FindAll(x => x.Type == type);
        }

        [HttpGet("getPetsByColor")]
        public List<Pet> GetPetsByColor(string color)
        {
            return pets.FindAll(x => x.Color == color);
        }

        [HttpGet("getPetsByPriceRange")]
        public List<Pet> GetPetsByPriceRange(int min, int max)
        {
            return pets.FindAll(x => x.Price >= min && x.Price <= max);
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpDelete("deleteSelledPet")]
        public List<Pet> DeleteSelledPet(string name)
        {
            foreach (var item in pets)
            {
                if (item.Name == name)
                {
                    pets.Remove(item);
                    break;
                }
            }

            return pets;
        }

        [HttpPatch("editPrice")]
        public Pet EditPrice(Pet pet)
        {
            foreach (var item in pets)
            {
                if (item.Name == pet.Name)
                {
                    item.Price = pet.Price;
                    return item;
                }
            }

            return null;
        }
    }
}
