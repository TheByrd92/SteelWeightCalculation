using SteelWeightCalculation.PartTypes;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SteelWeightCalculation
{
    public class DescriptionReader
    {
        public const double WEIGHT_OF_STEEL = 0.28356481481;
        
        private enum SteelParts
        {
            LShape,
            ZShape,
            CShape,
            EndwallRafter,
            EndwallCornerColumn,
            EndwallJAMB,
            Header,
            HotRollWShape,
            BuiltUpMember,
            HotRollCShape,
            ISShape
        }
        /// <summary>
        ///     Calculate a length value from passed in strings.
        /// <para >
        ///     Uses the <see cref="UnitConversion.FeetAndInches.InputNum(object)"/> for calculating the weight.
        /// </para>
        /// </summary>
        /// <param name="possibleLengths">Will return a length. Otherwise -1.</param>
        /// <returns></returns>
        public double GetLengthFromArchitectualLength(params string[] possibleLengths)
        {
            double toReturn = -1;
            using (UnitConversion.FeetAndInches ucFaI = new UnitConversion.FeetAndInches())
            {
                foreach (string item in possibleLengths)
                {
                    toReturn = ucFaI.InputNum(item);
                    if(toReturn > 0)
                    {
                        break;
                    }
                }
            }
            return toReturn;
        }

        /// <summary>
        /// Calculate a weight value for a part based on description.
        /// </summary>
        /// <param name="description">The full description of your steel part.</param>
        /// <param name="knownLength">Needs a valid length or weight may not be calculated.</param>
        /// <param name="possibleChildren">This steel part may have children.</param>
        public double CalculateWeightFor(string description, double knownLength, List<SteelPart> possibleChildren = null)
        {
            double toReturn = -1;
            try
            {
                SteelPart toCalculate = null;
                foreach (SteelParts steelParts in Enum.GetValues(typeof(SteelParts)))
                {
                    if (description.Contains("BUILT UP") ||
                        description.Contains("built up") ||
                        description.Contains("BUILT-UP") ||
                        description.Contains("built-up"))
                        break;
                    Regex regex;
                    switch (steelParts)
                    #region SteelPartMatchingCasting
                    {
                        #region AlreadyCreated (Last Added: ISShape)
                        case SteelParts.LShape:
                            LShape lShape;
                            regex = new Regex(LShape.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                lShape = new LShape();
                                toCalculate = lShape;
                                break;
                            }
                            continue;
                        case SteelParts.ZShape:
                            ZShape zShape;
                            regex = new Regex(ZShape.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                zShape = new ZShape();
                                toCalculate = zShape;
                                break;
                            }
                            continue;
                        case SteelParts.CShape:
                            CShape cShape;
                            regex = new Regex(CShape.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                cShape = new CShape();
                                toCalculate = cShape;
                                break;
                            }
                            continue;
                        case SteelParts.EndwallRafter:
                            EndwallRafter endwallRafter;
                            regex = new Regex(EndwallRafter.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                endwallRafter = new EndwallRafter();
                                toCalculate = endwallRafter;
                                break;
                            }
                            continue;
                        case SteelParts.EndwallCornerColumn:
                            EndwallCornerColumn endwallCornerColumn;
                            regex = new Regex(EndwallCornerColumn.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                endwallCornerColumn = new EndwallCornerColumn();
                                toCalculate = endwallCornerColumn;
                                break;
                            }
                            continue;
                        case SteelParts.EndwallJAMB:
                            EndwallJAMB endwallJAMB;
                            regex = new Regex(EndwallJAMB.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                endwallJAMB = new EndwallJAMB();
                                toCalculate = endwallJAMB;
                                break;
                            }
                            continue;
                        case SteelParts.Header:
                            Header header;
                            regex = new Regex(Header.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                header = new Header();
                                toCalculate = header;
                                break;
                            }
                            continue;
                        case SteelParts.HotRollWShape:
                            HotRollWShape hotRollWShape;
                            regex = new Regex(HotRollWShape.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                hotRollWShape = new HotRollWShape();
                                toCalculate = hotRollWShape;
                                break;
                            }
                            continue;
                        case SteelParts.HotRollCShape:
                            HotRollCShape hotRollCShape;
                            regex = new Regex(HotRollCShape.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                hotRollCShape = new HotRollCShape();
                                toCalculate = hotRollCShape;
                                break;
                            }
                            continue;
                        case SteelParts.ISShape:
                            ISShape isShape;
                            regex = new Regex(ISShape.NEEDS_TO_MATCH_THIS);
                            if (regex.IsMatch(description))
                            {
                                isShape = new ISShape();
                                toCalculate = isShape;
                                break;
                            }
                            continue;
                            #endregion
                    }
                    #endregion
                }
                if (toCalculate != null)
                {
                    toReturn = FinishCalculation(toCalculate, description, knownLength, possibleChildren);
                }
                else
                {
                    //For built-up members or anything that isn't matched with a known part it will just add what it found in the flange layer/cut layer.
                    toCalculate = new BuiltUpMember();
                    toReturn = FinishCalculation(toCalculate, description, knownLength, possibleChildren);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return toReturn;
        }

        private double FinishCalculation(SteelPart toCalculate, string description, double knownLength, List<SteelPart> possibleChildren)
        {
            if (possibleChildren != null)
                AddInChildren(toCalculate, possibleChildren);
            toCalculate.fullDescription = description;
            toCalculate.length = knownLength;
            toCalculate.CalculateWeight();
            toCalculate.AddChildrenToTotalWeight();
            return toCalculate.weight;
        }

        private void AddInChildren(SteelPart steelPartParent, List<SteelPart> steelPartChildren)
        {
            foreach (var item in steelPartChildren)
            {
                steelPartParent.steelPartChildren.Add(item);
            }
        }
    }
}
