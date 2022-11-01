using System;

namespace PetApi.Models;

public class Pet
{
    public Pet()
    {
    }

    public Pet(string name, string type, int age, int price)
    {
        Name = name;
        PetType = type;
        Age = age;
        Price = price;
    }
    public string PetType { get; set; }

    public int Price { get; set; }

    public int Age { get; set; }

    public string Name { get; set; }
    public override bool Equals(object? obj)
    {
        var pet = obj as Pet;
        return pet != null &&
               pet.Name.Equals(Name) &&
               pet.PetType.Equals(PetType) &&
               pet.Age.Equals(Age) &&
               pet.Price == Price;
    }
}