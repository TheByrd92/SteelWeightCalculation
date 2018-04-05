using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class HotRollCShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "[Cc]\\d+[Xx]\\d+[.]?\\d+";
        public override double weight { get; set; }
        public override double length { get; set; }
        public override string fullDescription { get; set; }

        private enum DimensionIndices
        {
            ApproxDepth,
            WeightPerFoot
        }

        public override void AddChildrenToTotalWeight()
        {
            foreach (var item in steelPartChildren)
            {
                this.weight += item.weight;
            }
        }

        public override void CalculateWeight()
        {
            string regExMatch = NEEDS_TO_MATCH_THIS;
            string matchedString = new Regex(regExMatch).Match(fullDescription).ToString();
            string regExSplitMatch = "[A-Za-z]";
            string[] dimensions = new Regex(regExSplitMatch).Split(matchedString.Replace("C", "").Replace("c", ""));
            //Should get three different numbers only
            double depth = 0.0;
            double perFoot = 0.0;
            double.TryParse(dimensions[(int)DimensionIndices.ApproxDepth], out depth);
            double.TryParse(dimensions[(int)DimensionIndices.WeightPerFoot], out perFoot);
            if (depth <= 0 || perFoot <= 0 || length <= 0)
            {
                weight = 0.0;
                return;
            }
            weight = Math.Round((length / 12) * perFoot, 2);
        }
    }
}
