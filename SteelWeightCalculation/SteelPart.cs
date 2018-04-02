namespace SteelWeightCalculation
{
    public enum DimensionIndices
    {
        Width,
        Legs,
        Thickness
    }
    
    public abstract class SteelPart
    {
        public enum SteelPart_Types
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

        public const string NEEDS_TO_MATCH_THIS = "";
        public abstract double weight { get; set; }
        public abstract double length { get; set; }
        public abstract string fullDescription { get; set; }
        public abstract void CalculateWeight();
    }
}
