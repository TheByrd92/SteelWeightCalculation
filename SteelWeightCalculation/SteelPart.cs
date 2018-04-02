using System.Collections.Generic;

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
        public List<SteelPart> steelPartChildren = new List<SteelPart>();
        public abstract void CalculateWeight();
        public abstract void AddChildrenToTotalWeight();
    }
}
