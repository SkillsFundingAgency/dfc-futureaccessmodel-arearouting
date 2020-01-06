using System.ComponentModel.DataAnnotations;
using DFC.Swagger.Standard.Annotations;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    internal sealed class RoutingDetail :
        IRoutingDetail
    {
        /// <summary>
        /// the region's unique identifier
        /// </summary>
        [Required]
        [Display(Description = "The region's unique identifier")]
        [StringLength(10, MinimumLength = 10)]
        [Example(Description = TouchpointArea.EastMidlandsAndNorthamptonshire)]
        public string TouchpointID { get; set; }

        /// <summary>
        /// the name of the region
        /// </summary>
        [Required]
        [StringLength(50)]
        [RegularExpression(ValidationExpressions.StandardText)]
        [Display(Description = "The name of the region")]
        [Example(Description = "East of England and Buckinghamshire")]
        public string Area { get; set; }

        /// <summary>
        /// contractor's region specific contact phone number
        /// </summary>
        [Required]
        [StringLength(12)]
        [RegularExpression(ValidationExpressions.PhoneNumber)]
        [Display(Description = "The contractor's region specific contact phone number")]
        [Example(Description = "01234 456789")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// contractor's region specific contact 'text' number
        /// </summary>
        [Required]
        [StringLength(12)]
        [RegularExpression(ValidationExpressions.PhoneNumber)]
        [Display(Description = "The contractor's region specific contact 'text' number")]
        [Example(Description = "01234 456789")]
        public string SMSNumber { get; set; }

        /// <summary>
        /// contractor's region specific contact email address
        /// </summary>
        [Required]
        [StringLength(100)]
        [RegularExpression(ValidationExpressions.EmailAddress)]
        [Display(Description = "The contractor's region specific contact email address")]
        [Example(Description = "abc@regionprovider.co.uk")]
        public string EmailAddress { get; set; }
    }
}