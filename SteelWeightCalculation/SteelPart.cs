namespace SteelWeightCalculation
{
    public enum DimensionIndices
    {
        Width,
        Legs,
        Thickness
    }

    public enum  SteelPart_Types
    {
        LShape,
        ZShape,
        CShape,
        EndwallRafter,
        EndwallCornerColumn,
        EndwallJAMB,
        Header,
        HotRollWShape
    }

    public abstract class SteelPart
    {
        public abstract string descriptionExpression { get; set; }
        public abstract double weight { get; set; }
        public abstract double length { get; set; }
        public abstract string fullDescription { get; set; }
        public abstract void CalculateWeight();
    }
}
