using System;
using System.Globalization;
using System.Xml.Serialization;

namespace SetComparer.Common
{
    [Serializable]
    public class ReportItem
    {
        const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        [XmlIgnore]
        public DateTime Timestamp { get; set; }
        
        // "helper" property for Timestamp(DateTime) serialization
        [XmlElement("Timestamp")]
        public string TimestampString
        {
            get { return Timestamp.ToString(DATE_FORMAT, CultureInfo.InvariantCulture); }
            set { Timestamp = DateTime.ParseExact(value, DATE_FORMAT, CultureInfo.InvariantCulture); }
        }

        public string Set { get; set; }


        public ReportItem(string set, DateTime timestamp)
        {
            Set = set;
            Timestamp = timestamp;
        }
        public ReportItem() { }
    }
}
