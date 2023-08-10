using System.ComponentModel.DataAnnotations.Schema;

namespace MarketManager.Domain.Entities;

    public class Package : BaseAuditableEntity
    {
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
        public double IncomingCount { get; set; }
        public double ExistCount { get; set; }
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public double IncomingPrice { get; set; }


        /// <summary>
        /// This is additional percent for incoming
        /// </summary>
        /// 
        private double markup { get; set; }

        [NotMapped]
        public double Markup
        {
            get
            {
                return markup;
            }
            set
            {
                markup = (Product.SalePrice - IncomingPrice) / 100;
            }
        }

        public DateTime IncomingDate { get; set; }

        public virtual ICollection<ExpiredProduct> ExpiredProducts { get; set; }

    }
