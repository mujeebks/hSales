namespace Nogales.BusinessModel
{
    public class JournalFilterBM
    {
        //used for IC, AP & AR
        public string StartDate { get; set; }

        //used for IC, AP & AR
        public string EndDate { get; set; }

        //used for IC, AP & AR
        public string AccountNumberStart { get; set; }

        //used for IC, AP & AR
        public string AccountNumberEnd { get; set; }

        //used for AP   
        public string VendorNumber { get; set; }
        
        //used for AP   
        public string InvoiceNumber { get; set; }

        //used for AP & AR
        public string StartSession { get; set; }

        //used for AP & AR
        public string EndSession { get; set; }

        //used for AP & AR
        public string StartBatch { get; set; }

        //used for AP & AR
        public string EndBatch { get; set; }

        //used for AR
        public string TransactionNumber { get; set; }

        //used for AR
        public string ARType { get; set; }

        //used for AR
        public string CustomerNumber { get; set; }

        public string JournalType { get; set; }

        public string GLAccounTtype { get; set; }

        public string SecondStartSession { get; set; }

        public string SecondEndSession { get; set; }

    }
}
