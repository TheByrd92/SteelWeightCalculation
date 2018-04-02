using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class EndwallRafter : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "(ENDWALL JAMB|endwall jamb)";
        public override double weight { get { return weight; } set => weight = value; }
        public override double length { get { return length; } set => length = value; }
        public override string fullDescription { get { return fullDescription; } set => fullDescription = value; }

        public override void CalculateWeight()
        {
            string regExMatch = "\\(\\d+\\s*[Cc]\\s*\\d\\s+\\d+\\/\\d+\\s*[xX]\\s*\\d+[Gg][Aa]\\)\\s*[Xx]\\s*\\d+\'\\-\\d+\\s*\\d+\\/\\d+\\\"";
            string matchedString = new Regex(regExMatch).Match(fullDescription).ToString();
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
                leg = uc.InputNum(dimensions[(int)DimensionIndices.Legs].Trim() + "\"");
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
