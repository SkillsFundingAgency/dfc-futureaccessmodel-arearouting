using System.Text.RegularExpressions;
using DFC.FutureAccessModel.AreaRouting.Models;

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
        private readonly Regex locationReg = new Regex(ValidationExpressions.StandardText);

        /// <summary>
        /// get type of expression for...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the expression type</returns>
        public TypeOfExpression GetTypeOfExpressionFor(string theCandidate)
        {
            if (postcodeReg.Match(theCandidate).Success)
            {
                return TypeOfExpression.Postcode;
            }

            if (outwardReg.Match(theCandidate).Success)
            {
                return TypeOfExpression.Outward;
            }

            if (locationReg.Match(theCandidate).Success)
            {
                return TypeOfExpression.Town;
            }

            return TypeOfExpression.Unknown;
        }
    }
}
