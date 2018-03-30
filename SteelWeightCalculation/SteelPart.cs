namespace SteelWeightCalculation
{
    public abstract class SteelPart
    {
        public abstract string descriptionExpression { get; set; }
        public abstract double weight { get; set; }
        public abstract double length { get; set; }
        public abstract string fullDescription { get; set; }
        public abstract void CalculateWeight();
    }
}
