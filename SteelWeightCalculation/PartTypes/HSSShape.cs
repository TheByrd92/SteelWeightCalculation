using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class HSSShape : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "([Hh][Ss][Ss][ ]*\\d+\"[ ]*[Xx][ ]*\\d\"[ ]*[Xx][ ]*\\d+\\/\\d+\")";
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
            string removePatterns = "([Hh][Ss][Ss])";
            string cleanMatchedString = new Regex(removePatterns).Replace(matchedString, "").Trim();
            string regExSplitMatch = "[A-Za-z]";
            string[] dimensions = new Regex(regExSplitMatch).Split(cleanMatchedString);
            //Should get three different numbers only
            double width = 0.0;
            double leg = 0.0;
            double thickness = 0.0;
            double weightPerFoot = 0.0;
            using (UnitConversion.FeetAndInches uc = new UnitConversion.FeetAndInches())
            {
                width = uc.InputNum(dimensions[(int)DimensionIndices.Width].Trim());
                double.TryParse(dimensions[(int)DimensionIndices.Legs].Trim(), out leg);
                if (leg <= 0)
                    leg = uc.InputNum(dimensions[(int)DimensionIndices.Legs].Trim());
                thickness = uc.GetGaugeInDecimal(dimensions[(int)DimensionIndices.Thickness].Trim());
            }
            
            switch (width)
            {
                case 5:
                    weightPerFoot = LbsPerFootForNominalSize5(thickness);
                    break;
                case 6:
                    weightPerFoot = LbsPerFootForNominalSize6(thickness);
                    break;
                case 8:
                    weightPerFoot = LbsPerFootForNominalSize8(thickness);
                    break;
                case 10:
                    weightPerFoot = LbsPerFootForNominalSize10(thickness);
                    break;
                case 12:
                    weightPerFoot = LbsPerFootForNominalSize12(thickness);
                    break;
            }

            if (length <= 0 || width <= 0 || leg <= 0 || thickness <= 0 || weightPerFoot <=0)
            {
                weight = 0.0;
                return;
            }
            weight = Math.Round((length/12) * weightPerFoot, 2);
        }

        private double LbsPerFootForNominalSize5(double thickness)
        {
            switch (thickness)
            {
                //  3/8
                case 0.375:
                    return 22.370;
                //  1/2
                case 0.500:
                    return 28.430;
            }
            return 0.0;
        }

        private double LbsPerFootForNominalSize6(double thickness)
        {
            switch (thickness)
            {
                //  3/16
                case 0.1875:
                    return 14.530;
                //  1/4
                case 0.250:
                    return 19.020;
                //  5/16
                case 0.3125:
                    return 23.340;
                //  3/8
                case 0.375:
                    return 22.370;
                //  1/2
                case 0.500:
                    return 28.430;
                //  5/8
                case 0.625:
                    return 42.260;
            }
            return 0.0;
        }

        private double LbsPerFootForNominalSize7(double thickness)
        {
            switch (thickness)
            {
                //  3/16
                case 0.1875:
                    return 17.080;
                //  1/4
                case 0.250:
                    return 22.420;
                //  5/16
                case 0.3125:
                    return 27.590;
                //  3/8
                case 0.375:
                    return 32.580;
                //  1/2
                case 0.500:
                    return 42.050;
            }
            return 0.0;
        }

        private double LbsPerFootForNominalSize8(double thickness)
        {
            switch (thickness)
            {
                //  3/16
                case 0.1875:
                    return 19.630;
                //  1/4
                case 0.250:
                    return 25.820;
                //  5/16
                case 0.3125:
                    return 31.840;
                //  3/8
                case 0.375:
                    return 37.690;
                //  1/2
                case 0.500:
                    return 48.850;
                //  5/8
                case 0.625:
                    return 59.320;
            }
            return 0.0;
        }

        private double LbsPerFootForNominalSize10(double thickness)
        {
            switch (thickness)
            {
                //  3/16
                case 0.1875:
                    return 24.730;
                //  1/4
                case 0.250:
                    return 32.630;
                //  5/16
                case 0.3125:
                    return 40.350;
                //  3/8
                case 0.375:
                    return 47.900;
                //  1/2
                case 0.500:
                    return 62.460;
                //  5/8
                case 0.625:
                    return 76.330;
            }
            return 0.0;
        }

        private double LbsPerFootForNominalSize12(double thickness)
        {
            switch (thickness)
            {
                //  1/4
                case 0.250:
                    return 39.430;
                //  5/16
                case 0.3125:
                    return 48.860;
                //  3/8
                case 0.375:
                    return 58.100;
                //  1/2
                case 0.500:
                    return 76.070;
                //  5/8
                case 0.625:
                    return 93.250;
            }
            return 0.0;
        }
    }
}
