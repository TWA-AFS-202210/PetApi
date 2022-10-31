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
    }
}