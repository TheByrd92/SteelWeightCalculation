using System.Text.RegularExpressions;

namespace SteelWeightCalculation
{
    public class DescriptionReader
    {
        public const double WEIGHT_OF_STEEL = 0.28356481481;

        private Regex rgx;
        private enum SteelParts
        {

        }

        private bool IsKnownPart()
        {
            return false;
        }
        /// <summary>
        /// Calculate a weight value for a part based on description.
        /// </summary>
        /// <param name="description"></param>
        public void CalculateWeightFor(string description)
        {

        }
    }
}
