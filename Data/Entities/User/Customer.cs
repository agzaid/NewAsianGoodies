using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.User
{
    public class Customer : BaseEntity
    {
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }

        #region Navigation Properties
       // public virtual List<Order> Orders { get; set; } = new();
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        #endregion
    }

    public enum Gender
    {
        Male = 1,
        Female = 2
    }
}
