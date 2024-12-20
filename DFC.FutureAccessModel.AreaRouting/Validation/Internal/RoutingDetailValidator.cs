using DFC.FutureAccessModel.AreaRouting.Factories;
using DFC.FutureAccessModel.AreaRouting.Faults;
using DFC.FutureAccessModel.AreaRouting.Helpers;
using DFC.FutureAccessModel.AreaRouting.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.FutureAccessModel.AreaRouting.Validation.Internal
{
    /// <summary>
    /// the routing detail validator
    /// </summary>
    internal sealed class RoutingDetailValidator :
        IValidateRoutingDetails
    {
        /// <summary>
        /// the message (factory)
        /// </summary>
        public ICreateValidationMessageContent Message { get; }

        /// <summary>
        /// initialsies an instance of <see cref="RoutingDetailValidator"/>
        /// </summary>
        /// <param name="message">the message (factory)</param>
        public RoutingDetailValidator(ICreateValidationMessageContent message)
        {
            It.IsNull(message)
                .AsGuard<ArgumentNullException>(nameof(message));

            Message = message;
        }

        /// <summary>
        /// validate...
        /// </summary>
        /// <param name="theCandidate">the candidate</param>
        /// <returns>the currently running task</returns>
        public async Task Validate(IRoutingDetail theCandidate) =>
            await Task.Run(() =>
            {
                It.IsNull(theCandidate)
                    .AsGuard<ArgumentNullException>(nameof(theCandidate));

                var context = new ValidationContext(theCandidate, null, null);
                var results = Collection.Empty<ValidationResult>();

                Validator.TryValidateObject(theCandidate, context, results, true);

                results.Any()
                    .AsGuard<UnprocessableEntityException>(Message.Create(results.AsSafeReadOnlyList()));
            });
    }
}
