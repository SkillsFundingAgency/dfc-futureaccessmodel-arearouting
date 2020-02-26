using System.ComponentModel.DataAnnotations;
using DFC.FutureAccessModel.AreaRouting.Validation;
using DFC.Swagger.Standard.Annotations;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    public  class RoutingDetail :
        IRoutingDetail
    {
        /// <summary>
        /// the fallback id
        /// </summary>
        public const string FallbackID = "0000000999";

        /// <summary>
        /// the region's unique identifier
        /// </summary>
        [Key]
        [Required]
        [JsonProperty("id")]
        [RegularExpression(ValidationExpressions.TouchpointID)]
        [Display(Description = "The region's unique identifier")]
        [StringLength(10, MinimumLength = 10)]
        [Example(Description = "0000000101")]
        public string TouchpointID { get; set; }

        /// <summary>
        /// the name of the region
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 5)]
        [RegularExpression(ValidationExpressions.TownOrRegion)]
        [Display(Description = "The name of the region")]
        [Example(Description = "East of England and Buckinghamshire")]
        public string Area { get; set; }

        /// <summary>
        /// contractor's region specific contact phone number
        /// </summary>
        [Required]
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression(ValidationExpressions.PhoneNumber)]
        [Display(Description = "The contractor's region specific contact phone number")]
        [Example(Description = "01234 456789")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// contractor's region specific contact 'text' number
        /// </summary>
        [StringLength(12)]
        [RegularExpression(ValidationExpressions.MobileNumber)]
        [Display(Description = "The contractor's region specific contact 'text' number")]
        [Example(Description = "07123 456789")]
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