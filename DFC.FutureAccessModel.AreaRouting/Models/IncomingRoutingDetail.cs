using System.ComponentModel.DataAnnotations;
using DFC.FutureAccessModel.AreaRouting.Storage;
using DFC.Swagger.Standard.Annotations;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    public sealed class IncomingRoutingDetail :
        IRoutingDetail
    {
        /// <summary>
        /// the region's unique identifier
        /// </summary>
        [Key]
        [Required]
        [JsonProperty("id")]
        [Display(Description = "The region's unique identifier")]
        [StringLength(10, MinimumLength = 10)]
        [Example(Description = "0000000101")]
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

        /// <summary>
        /// here to ensure cosmos db grouping is singular
        /// </summary>
        [PartitionKey]
        public string PartitionKey => "not_required";
    }
}