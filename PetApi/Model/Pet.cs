namespace PetApi.Model
{
    public class Pet
    {
        public Pet()
        {
        }

        public Pet(string name, string type, string color, int price)
        {
            this.Name = name;
            this.Type = type;
            this.Color = color; 
            this.Price = price;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public int Price { get; set; }

        public override bool Equals(object? obj)
        {
            var pet = obj as Pet;
            return pet != null &&
                Name.Equals(pet.Name) &&
                Type.Equals(pet.Type) &&
                Color.Equals(pet.Color) &&
                Price.Equals(pet.Price);
        }
    }
}
