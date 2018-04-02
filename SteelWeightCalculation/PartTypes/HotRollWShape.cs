using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class HotRollWShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "[Ww]\\d+[Xx]\\d+";
        public override double weight { get { return weight; } set => weight = value; }
        public override double length { get { return length; } set => length = value; }
        public override string fullDescription { get { return fullDescription; } set => fullDescription = value; }

        private enum DimensionIndices
        {
            ApproxDepth,
            WeightPerFoot
        }

        public override void CalculateWeight()
        {
            string regExMatch = NEEDS_TO_MATCH_THIS;
            string matchedString = new Regex(regExMatch).Match(fullDescription).ToString();
            string regExSplitMatch = "[A-Za-z]";
            string[] dimensions = new Regex(regExSplitMatch).Split(matchedString.Replace("W", "").Replace("w", ""));
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
