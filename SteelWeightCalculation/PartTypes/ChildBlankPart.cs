using System;

namespace SteelWeightCalculation.PartTypes
{
    public class ChildBlankPart : SteelPart
    {
        public override double weight { get { return weight; } set => weight = value; }
        public override double length { get { return length; } set => length = value; }
        public override string fullDescription { get { return fullDescription; } set => fullDescription = value; }

        public override void AddChildrenToTotalWeight()
        {
            throw new NotImplementedException();
        }

        public override void CalculateWeight()
        {
            throw new NotImplementedException();
        }
    }
}
