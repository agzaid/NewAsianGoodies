using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Shop
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        #region Navigation Properties
        //public OrderDetails OrderDetails { get; set; }
        //public int? OrderDetailsId { get; set; }
        #endregion
    }
}
