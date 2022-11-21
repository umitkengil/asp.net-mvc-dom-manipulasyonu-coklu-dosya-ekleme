using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bes.Models
{
    public class MasrafRapor
    {
        public string magazaAdi { get; set; }
        public string masrafKodu{ get; set;}
        public string masrafAciklama { get; set; }
        public decimal masraf { get; set; }
    }
}