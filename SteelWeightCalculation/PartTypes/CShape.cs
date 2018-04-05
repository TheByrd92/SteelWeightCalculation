using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class CShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "((\\d+[cC]\\d+.*[xX]\\d+[Gg][Aa])|(\\d+[Xx]\\d+[.]{1}\\d+[Cc]\\d+))";
        public override double weight { get; set; }
        public override double length { get; set; }
        public override string fullDescription { get; set; }

        public override void AddChildrenToTotalWeight()
        {
            foreach (var item in steelPartChildren)
            {
                this.weight += item.weight;
            }
            this.weight = Math.Round(this.weight, 2);
        }

        public override void CalculateWeight()
        {
            string matchedString = new Regex(NEEDS_TO_MATCH_THIS).Match(fullDescription).ToString();
            string regExMatchForLength = "[Xx]\\s*\\d+\'\\-\\d+\\s*\\d+\\/\\d+\\\"";
            string architectLength = new Regex(regExMatchForLength).Match(matchedString).ToString().Replace("X", "").Replace("x", "").Trim();
            matchedString = new Regex(regExMatchForLength).Replace(matchedString, "").Trim();
            string regExSplitMatch = "[A-Za-z]";
            string[] dimensions = new Regex(regExSplitMatch).Split(matchedString.Replace("(", "").Replace(")", ""));
            //Should get three different numbers only
            double width = 0.0;
            double leg = 0.0;
            double thickness = 0.0;
            using (UnitConversion.FeetAndInches uc = new UnitConversion.FeetAndInches())
            {
                width = uc.InputNum(dimensions[(int)DimensionIndices.Width].Trim() + "\"");
                double.TryParse(dimensions[(int)DimensionIndices.Legs].Trim(), out leg);
                if(leg <= 0)
                    leg = uc.InputNum(dimensions[(int)DimensionIndices.Legs].Trim());
                thickness = uc.GetGaugeInDecimal(dimensions[(int)DimensionIndices.Thickness].Trim());
            }
            if (length <= 0 || width <= 0 || leg <= 0 || thickness <= 0)
            {
                weight = 0.0;
                return;
            }
            weight = Math.Round(((((leg * 2) + width) * thickness) * length) * DescriptionReader.WEIGHT_OF_STEEL, 2);
        }
    }
}
