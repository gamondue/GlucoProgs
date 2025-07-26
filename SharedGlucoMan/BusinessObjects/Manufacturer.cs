namespace GlucoMan
{
    internal class Manufacturer
    {
        public int IdManufacturer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Manufacturer(string name)
        {
            Name = name;
        }
        public Manufacturer()
        {
        }
        public override string ToString()
        {
            return Name;
        }
    }
}