using System.ComponentModel;

namespace DFC.FutureAccessModel.AreaRouting.Models
{
    // TODO: remove me, as i restrict change...
    /// <summary>
    /// the touch point areas
    /// </summary>
    public static class TouchpointArea
    {
        /// <summary>
        /// the east of england and buckinghamshire
        /// </summary>
        [Description("Touchpoint Area: 0000000101, East of England and Buckinghamshire")]
        public const string EastOfEnglandAndBuckinghamshire = "0000000101";

        /// <summary>
        /// the east midlands and northamptonshire
        /// </summary>
        [Description("Touchpoint Area: 0000000102, East Midlands and Northamptonshire")]
        public const string EastMidlandsAndNorthamptonshire = "0000000102";

        /// <summary>
        /// london
        /// </summary>
        [Description("Touchpoint Area: 0000000103, London")]
        public const string London = "0000000103";

        /// <summary>
        /// the west midlands
        /// </summary>
        [Description("Touchpoint Area: 0000000104, West Midlands")]
        public const string WestMidlands = "0000000104";

        /// <summary>
        /// the north west
        /// </summary>
        [Description("Touchpoint Area: 0000000105, North West")]
        public const string NorthWest = "0000000105";

        /// <summary>
        /// the north east and cumbria
        /// </summary>
        [Description("Touchpoint Area: 0000000106, North East and Cumbria")]
        public const string NorthEastAndCumbria = "0000000106";

        /// <summary>
        /// the south east
        /// </summary>
        [Description("Touchpoint Area: 0000000107, South East")]
        public const string SouthEast = "0000000107";

        /// <summary>
        /// the south west
        /// </summary>
        [Description("Touchpoint Area: 0000000108, South West")]
        public const string SouthWest = "0000000108";

        /// <summary>
        /// yorkshire and the humber
        /// </summary>
        [Description("Touchpoint Area: 0000000109, Yorkshire and Humber")]
        public const string YorkshireAndHumber = "0000000109";

        /// <summary>
        /// the national careers helpline
        /// </summary>
        [Description("Touchpoint Area: 0000000999, National Careers Helpline")]
        public const string NationalCareersHelpline = "0000000999";

        /// <summary>
        /// digital (?)
        /// </summary>
        [Description("Touchpoint Area: 1000000000, Digital")]
        public const string Digital = "1000000000";

        /// <summary>
        /// a set of all of the currently available areas
        /// </summary>
        [Description("Touchpoint Area, Available Areas contains all of the currently available areas")]
        public static readonly string[] AvailableAreas = {
            EastOfEnglandAndBuckinghamshire,
            EastMidlandsAndNorthamptonshire,
            London,
            WestMidlands,
            NorthWest,
            NorthEastAndCumbria,
            SouthEast,
            SouthWest,
            YorkshireAndHumber,
            NationalCareersHelpline,
            Digital
        };
    }
}
