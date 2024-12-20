namespace DFC.FutureAccessModel.AreaRouting.Models
{
    public class ConfigurationSettings
    {
        public required string DocumentStoreAccountKey { get; set; }
        public required string DocumentStoreEndpointAddress { get; set; }
        public required string DocumentStoreID { get; set; }
        public required string LocalAuthorityCollectionID { get; set; }
        public required string RoutingDetailCollectionID { get; set; }
        public required string WEBSITE_RUN_FROM_PACKAGE { get; set; }
        public required string MSDEPLOY_RENAME_LOCKED_FILES { get; set; }
    }
}
