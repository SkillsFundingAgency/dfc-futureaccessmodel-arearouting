﻿using System.ComponentModel.DataAnnotations;
using DFC.Swagger.Standard.Annotations;
using Newtonsoft.Json;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    /// <summary>
    /// the local authority (intermediary class)
    /// </summary>
    public class LocalAuthority :
        ILocalAuthority
    {
        /// <summary>
        /// the local admin district code
        /// </summary>
        [Key]
        [Required]
        [JsonProperty("id")]
        [Display(Description = "The authority's unique identifier")]
        [StringLength(10, MinimumLength = 10)]
        [Example(Description = "E09000002")]
        public string LADCode { get; set; }

        /// <summary>
        /// the (authorities) touchpoint
        /// </summary>
        [Required]
        [Display(Description = "The authority's touchpoint")]
        [StringLength(10, MinimumLength = 10)]
        [Example(Description = "0000000101")]
        public string TouchpointID { get; set; }

        /// <summary>
        /// the (authority) name
        /// </summary>
        [Required]
        [StringLength(50)]
        [RegularExpression(ValidationExpressions.StandardText)]
        [Display(Description = "The name of the authority")]
        [Example(Description = "Barking and Dagenham")]
        public string Name { get; set; }
    }
}
