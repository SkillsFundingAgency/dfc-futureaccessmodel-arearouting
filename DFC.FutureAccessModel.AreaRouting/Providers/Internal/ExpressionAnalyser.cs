using DFC.FutureAccessModel.AreaRouting.Models;
using DFC.FutureAccessModel.AreaRouting.Validation;
using System.Text.RegularExpressions;

namespace DFC.FutureAccessModel.AreaRouting.Providers.Internal
{
    /// <summary>
    /// expression analyser
    /// </summary>
    internal sealed class ExpressionAnalyser :
        IAnalyseExpresssions
    {
        /// <summary>
        /// postcode reg
        /// </summary>
        private readonly Regex postcodeReg = new Regex(ValidationExpressions.Postcode);

        /// <summary>
        /// outward reg
        /// </summary>
        private readonly Regex outwardReg = new Regex(ValidationExpressions.OutwardCode);

        /// <summary>
        /// location reg
        /// </summary>
        private readonly Regex locationReg = new Regex(ValidationExpressions.TownOrRegion);

        /// <summary>
        /// get type of expression for...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the expression type</returns>
        public TypeOfExpression GetTypeOfExpressionFor(string theCandidate)
        {
            if (Matches(postcodeReg.Match(theCandidate), theCandidate))
            {
                return TypeOfExpression.Postcode;
            }

            if (Matches(outwardReg.Match(theCandidate), theCandidate))
            {
                return TypeOfExpression.Outward;
            }

            if (Matches(locationReg.Match(theCandidate), theCandidate))
            {
                return TypeOfExpression.Town;
            }

            return TypeOfExpression.Unknown;
        }

        /// <summary>
        /// matches...
        /// </summary>
        /// <param name="theMatch">the match</param>
        /// <param name="theCandidate">the candidate</param>
        /// <returns></returns>
        internal bool Matches(Match theMatch, string theCandidate) =>
            theMatch.Success
                && theMatch.Captures.Count == 1
                && theMatch.Length == theCandidate.Length;
    }
}
