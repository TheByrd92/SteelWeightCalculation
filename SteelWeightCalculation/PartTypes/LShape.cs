using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class LShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "([lL]\\d+[xX]\\d+[xX]\\d+\\/\\d+)|([lL]\\d+[xX]\\d+[xX].*[Gg][Aa])";
        public override double weight { get { return weight; } set => weight = value; }
        public override double length { get { return length; } set => length = value; }
        public override string fullDescription { get { return fullDescription; } set => fullDescription = value; }

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
            string[] dimensions = new Regex(regExSplitMatch).Split(matchedString.Replace("L", "").Replace("l", ""));
            //Should get three different numbers only
            double width = 0.0;
            double leg = 0.0;
            double thickness = 0.0;
            using (UnitConversion.FeetAndInches uc = new UnitConversion.FeetAndInches())
            {
                width = uc.InputNum(dimensions[(int)DimensionIndices.Width] + "\"");
                leg = uc.InputNum(dimensions[(int)DimensionIndices.Legs] + "\"");
                thickness = uc.GetGaugeInDecimal(dimensions[(int)DimensionIndices.Thickness].Trim());
            }
            if (length <= 0 || width <= 0 || leg <= 0 || thickness <= 0)
            {
                weight = 0.0;
                return;
            }
            weight = Math.Round((((leg + width) * thickness) * length) * DescriptionReader.WEIGHT_OF_STEEL, 2);
        }
    }
}
