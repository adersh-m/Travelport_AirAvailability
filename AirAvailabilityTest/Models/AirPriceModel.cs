using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace AirAvailabilityTest.Models
{
    public class AirPriceModel
    {
        [XmlRoot(ElementName = "BillingPointOfSaleInfo", Namespace = "http://www.travelport.com/schema/common_v42_0")]
        public class BillingPointOfSaleInfo
        {
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "OriginApplication")]
            public string OriginApplication { get; set; }
        }
        [XmlRoot(ElementName = "AirSegment", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class AirSegment
        {
            [XmlAttribute(AttributeName = "Key")]
            public string Key { get; set; }
            [XmlAttribute(AttributeName = "AvailabilitySource")]
            public string AvailabilitySource { get; set; }
            [XmlAttribute(AttributeName = "Equipment")]
            public string Equipment { get; set; }
            [XmlAttribute(AttributeName = "AvailabilityDisplayType")]
            public string AvailabilityDisplayType { get; set; }
            [XmlAttribute(AttributeName = "Group")]
            public string Group { get; set; }
            [XmlAttribute(AttributeName = "Carrier")]
            public string Carrier { get; set; }
            [XmlAttribute(AttributeName = "FlightNumber")]
            public string FlightNumber { get; set; }
            [XmlAttribute(AttributeName = "Origin")]
            public string Origin { get; set; }
            [XmlAttribute(AttributeName = "Destination")]
            public string Destination { get; set; }
            [XmlAttribute(AttributeName = "DepartureTime")]
            public string DepartureTime { get; set; }
            [XmlAttribute(AttributeName = "ArrivalTime")]
            public string ArrivalTime { get; set; }
            [XmlAttribute(AttributeName = "FlightTime")]
            public string FlightTime { get; set; }
            [XmlAttribute(AttributeName = "Distance")]
            public string Distance { get; set; }
            [XmlAttribute(AttributeName = "ProviderCode")]
            public string ProviderCode { get; set; }
            [XmlAttribute(AttributeName = "ClassOfService")]
            public string ClassOfService { get; set; }
        }
        [XmlRoot(ElementName = "AirItinerary", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class AirItinerary
        {
            [XmlElement(ElementName = "AirSegment", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public AirSegment AirSegment { get; set; }
        }
        [XmlRoot(ElementName = "BrandModifiers", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class BrandModifiers
        {
            [XmlAttribute(AttributeName = "ModifierType")]
            public string ModifierType { get; set; }
        }
        [XmlRoot(ElementName = "AirPricingModifiers", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class AirPricingModifiers
        {
            [XmlElement(ElementName = "BrandModifiers", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public BrandModifiers BrandModifiers { get; set; }
            [XmlAttribute(AttributeName = "InventoryRequestType")]
            public string InventoryRequestType { get; set; }
        }
        [XmlRoot(ElementName = "SearchPassenger", Namespace = "http://www.travelport.com/schema/common_v42_0")]
        public class SearchPassenger
        {
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "Code")]
            public string Code { get; set; }
            [XmlAttribute(AttributeName = "BookingTravelerRef")]
            public string BookingTravelerRef { get; set; }
            [XmlAttribute(AttributeName = "Key")]
            public string Key { get; set; }
        }
        [XmlRoot(ElementName = "BookingCode", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class BookingCode
        {
            [XmlAttribute(AttributeName = "Code")]
            public string Code { get; set; }
        }
        [XmlRoot(ElementName = "PermittedBookingCodes", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class PermittedBookingCodes
        {
            [XmlElement(ElementName = "BookingCode", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public BookingCode BookingCode { get; set; }
        }
        [XmlRoot(ElementName = "AirSegmentPricingModifiers", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class AirSegmentPricingModifiers
        {
            [XmlElement(ElementName = "PermittedBookingCodes", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public PermittedBookingCodes PermittedBookingCodes { get; set; }
            [XmlAttribute(AttributeName = "AirSegmentRef")]
            public string AirSegmentRef { get; set; }
            [XmlAttribute(AttributeName = "FareBasisCode")]
            public string FareBasisCode { get; set; }
        }
        [XmlRoot(ElementName = "AirPricingCommand", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class AirPricingCommand
        {
            [XmlElement(ElementName = "AirSegmentPricingModifiers", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public AirSegmentPricingModifiers AirSegmentPricingModifiers { get; set; }
        }
        [XmlRoot(ElementName = "FormOfPayment", Namespace = "http://www.travelport.com/schema/common_v42_0")]
        public class FormOfPayment
        {
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "Type")]
            public string Type { get; set; }
        }
        [XmlRoot(ElementName = "AirPriceReq", Namespace = "http://www.travelport.com/schema/air_v42_0")]
        public class AirPriceReq
        {
            [XmlElement(ElementName = "BillingPointOfSaleInfo", Namespace = "http://www.travelport.com/schema/common_v42_0")]
            public BillingPointOfSaleInfo BillingPointOfSaleInfo { get; set; }
            [XmlElement(ElementName = "AirItinerary", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public AirItinerary AirItinerary { get; set; }
            [XmlElement(ElementName = "AirPricingModifiers", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public AirPricingModifiers AirPricingModifiers { get; set; }
            [XmlElement(ElementName = "SearchPassenger", Namespace = "http://www.travelport.com/schema/common_v42_0")]
            public SearchPassenger SearchPassenger { get; set; }
            [XmlElement(ElementName = "AirPricingCommand", Namespace = "http://www.travelport.com/schema/air_v42_0")]
            public AirPricingCommand AirPricingCommand { get; set; }
            [XmlElement(ElementName = "FormOfPayment", Namespace = "http://www.travelport.com/schema/common_v42_0")]
            public FormOfPayment FormOfPayment { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "TraceId")]
            public string TraceId { get; set; }
            [XmlAttribute(AttributeName = "AuthorizedBy")]
            public string AuthorizedBy { get; set; }
            [XmlAttribute(AttributeName = "TargetBranch")]
            public string TargetBranch { get; set; }
        }
    }
}
