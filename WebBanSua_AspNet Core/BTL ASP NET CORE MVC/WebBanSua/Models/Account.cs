using System;
using System.Collections.Generic;

#nullable disable

namespace WebBanSua.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string TaiKhoan { get; set; }
        public int RoleId { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual RoleAccount Role { get; set; }
    }
}
