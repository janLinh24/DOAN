using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanSua.Models
{
    public partial class TinTuc
    {
        public int MaTt { get; set; }
        public string TenTt { get; set; }
        public string AnhTt { get; set; }
        public string Motangan { get; set; }
        public string Motadai { get; set; }
        public string Tacgia { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? LoaiTin { get; set; }
    }
}
