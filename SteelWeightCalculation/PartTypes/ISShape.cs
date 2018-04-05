using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class ISShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "(\\d+( |-)\\d+\\/\\d+\")( [Ii][.][Ss][.] [X] )(\\d+( |-)\\d+\\/\\d+\")( [X] \\d+[Gg][Aa])";
        public override double weight { get; set; }
        public override double length { get; set; }
        public override string fullDescription { get; set; }

        public override void AddChildrenToTotalWeight()
        {
            foreach (var item in steelPartChildren)
            {
                this.weight += item.weight;
            }
        }

        public override void CalculateWeight()
        {
            string matchedString = new Regex(NEEDS_TO_MATCH_THIS).Match(fullDescription).ToString();
            string removePatterns = "([Ii][.][Ss][.])|([Gg][Aa])";
            string cleanMatchedString = new Regex(removePatterns).Replace(matchedString, "").Trim();
            string regExSplitMatch = "[A-Za-z]";
            string[] dimensions = new Regex(regExSplitMatch).Split(cleanMatchedString);
            //Should get three different numbers only
            double width = 0.0;
            double leg = 0.0;
            double thickness = 0.0;
            using (UnitConversion.FeetAndInches uc = new UnitConversion.FeetAndInches())
            {
                width = uc.InputNum(dimensions[(int)DimensionIndices.Width].Trim());
                double.TryParse(dimensions[(int)DimensionIndices.Legs].Trim(), out leg);
                if (leg <= 0)
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
