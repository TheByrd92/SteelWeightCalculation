using System;

namespace SteelWeightCalculation.PartTypes
{
    public class ChildBlankPart : SteelPart
    {
        public override double weight { get; set; }
        public override double length { get; set; }
        public override string fullDescription { get; set; }

        public override void AddChildrenToTotalWeight()
        {
            
        }

        public override void CalculateWeight()
        {
            
        }
    }
}
