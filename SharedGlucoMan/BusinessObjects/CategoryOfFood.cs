namespace GlucoMan
{
    internal class CategoryOfFood
    {
        public int IdCategory { get; set; }
        public string Name { get; set; }
        public CategoryOfFood(string name)
        {
            Name = name;
        }
        public CategoryOfFood()
        {
            
        }
        public override string ToString()
        {
            return Name;
        }
    }
}