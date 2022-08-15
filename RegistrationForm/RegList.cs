using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationForm
{
    internal class RegList
    {
        public string QR { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string passportID { get; set; }
        public string age { get; set; }
        public string type { get; set; }
        public string district { get; set; }
        public string jamoat { get; set; }
        public string village { get; set; }
        public string phone { get; set; }
    }

    public class SubmitData
    {
        public string id { get; set; }
        public string qr { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string passport { get; set; }
        public string age { get; set; }
        public string type { get; set; }
        public string district { get; set; }
        public string jamoat { get; set; }
        public string village { get; set; }
        public string phone { get; set; }
        public string date { get; set; }
        public string trainer { get; set; }
        public string topic { get; set; }
        public string category { get; set; }
        public string eventJamoat { get; set; }
        public string eventDistrict { get; set; }
        public string project { get; set; }
        public string accommodation { get; set; }
        public string transport { get; set; }
        public string stationary { get; set; }
        public string lunch { get; set; }
        public string coffee { get; set; }
        public string status { get; set; }
    }
}
