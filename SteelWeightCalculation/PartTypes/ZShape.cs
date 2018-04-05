using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class ZShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "([zZ]\\d+[xX]\\d+[xX]\\d+\\/\\d+)|(\\d*[Zz].*[Xx].*[Gg][Aa])";
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
            string regExMatch = NEEDS_TO_MATCH_THIS;
            string matchedString = new Regex(regExMatch).Match(fullDescription).ToString();
            string regExSplitMatch = "[A-Za-z]";
            string[] dimensions = new Regex(regExSplitMatch).Split(matchedString);
            //Should get three different numbers only
            double width = 0.0;
            double leg = 0.0;
            double thickness = 0.0;
            using (UnitConversion.FeetAndInches uc = new UnitConversion.FeetAndInches())
            {
                width = uc.InputNum(dimensions[(int)DimensionIndices.Width] + "\"");
                leg = uc.InputNum(dimensions[(int)DimensionIndices.Legs] + "\"");
                if (leg == -1) double.TryParse(dimensions[(int)DimensionIndices.Legs], out leg);
                thickness = uc.GetGaugeInDecimal(dimensions[(int)DimensionIndices.Thickness]);
            }
            if (length <= 0 || width <= 0 || leg <= 0 || thickness <= 0)
            {
                weight = 0.0;
                return;
            }
            weight = System.Math.Round(((((leg * 2) + width) * thickness) * length) * DescriptionReader.WEIGHT_OF_STEEL, 2);
        }
    }
}
