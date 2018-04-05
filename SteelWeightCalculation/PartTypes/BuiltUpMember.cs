using System;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation.PartTypes
{
    public class BuiltUpMember : SteelPart
    {
        public new const string NEEDS_TO_MATCH_THIS = "^.*";
        public override double weight { get; set ; }
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
            //Weights should be precalculated in the ACAD Drawing Reader for child parts.
            //Either way though built-up members are an accumulation of other steel parts.

            //Turnsout they can also have known shipping parts "Tacked onto them as well. See Jack Beams."
            string tackedOnStuffExpression = "(?<=(BUILT UP[\\)] )).*[\\)]";
            Regex reg = new Regex(tackedOnStuffExpression);
            string tackedOnItem = reg.Match(fullDescription).ToString().Replace("(", "").Replace(")", "");
            if (!string.IsNullOrEmpty(tackedOnItem))
            {
                string[] descriptionAndLength = tackedOnItem.Split('X');
                double tackedOnLength = 0;
                string lengthOfTackedOnItem = descriptionAndLength[1].Trim();
                if (!string.IsNullOrEmpty(lengthOfTackedOnItem))
                {
                    using (UnitConversion.FeetAndInches ucFaI = new UnitConversion.FeetAndInches())
                    {
                        tackedOnLength = ucFaI.InputNum(lengthOfTackedOnItem);
                    }
                }
                if(tackedOnLength > 0)
                {
                    DescriptionReader dR = new DescriptionReader();
                    double weightOfTackedOnItem = dR.CalculateWeightFor(descriptionAndLength[0].Trim(), tackedOnLength);
                    if (weightOfTackedOnItem > 0)
                    {
                        weight += weightOfTackedOnItem;
                    }
                }
            }
            
        }
    }
}
