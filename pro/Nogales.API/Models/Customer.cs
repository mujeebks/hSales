using System.Collections.Generic;

namespace Nogales.API.Models
{
    public class Customer
    {
        public string BusinessName { get; set; }
        public string EmailId { get; set; }
        public Address BillToAddress { get; set; }
        public Address ShipToAddress { get; set; }
        public PersonalDetail ContactAtLocation { get; set; }
        public PersonalDetail AccountPayableContact { get; set; }
        public List<Owner> Owners { get; set; }
        public string DeliveryTime { get; set; }
        public string Country { get; set; }
        public string BusinessType { get; set; }
        public string ResaleCertificate { get; set; }
        public string AnticipatedMonthlyPurchases { get; set; }
        public string SubsidaryOf { get; set; }
        public string YearInBusiness {get;set;}
        public CreditDepartment CreditDepartment { get; set; }
    }

    public class Address
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
    }

    public class PersonalDetail
    {
        public string PhoneAreaCode { get; set; }
        public string FaxAreaCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }

    public class Owner
    {
        public string DOB_Day { get; set; }
        public string DOB_Year { get; set; }
        public string DOB_Month { get; set; }
        public Address Address { get; set; }
        public string State { get; set; }
        public string SocialSecurityNumber { get; set; }
        public PersonalDetail OwnerName { get; set; }
        public string DrivingLicenseNumber { get; set; }
    }

    public class CreditDepartment
    {
        public string CustomerNumber { get; set; }
        public string OrderType { get; set; }
        public string SalesPerson { get; set; }
        public string RouteNumber { get; set; }
        public string Stop { get; set; }
        public string PriceLevel { get; set; }
        public string Terms { get; set; }
        public string Territory { get; set; }
    }
}