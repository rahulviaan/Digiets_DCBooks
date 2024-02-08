using System;

namespace Database
{
    public partial class BookDetails
    {

        public string BookName { get; set; }

        public string Board { get; set; }

        public string Catagory { get; set; }

        public string Class { get; set; }

        public string Subject { get; set; }

        public string Series { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public Nullable<byte> status { get; set; }

        public string IPaddress { get; set; }

        public Nullable<decimal> EbookPrice { get; set; }

        public Nullable<decimal> PrintPrice { get; set; }

        public Nullable<decimal> discount { get; set; }

    }
}

