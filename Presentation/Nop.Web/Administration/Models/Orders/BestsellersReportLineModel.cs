using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.Orders
{
    public partial class BestsellersReportLineModel : BaseNopModel
    {
        public int ProductId { get; set; }
        [NopResourceDisplayName("Admin.SalesReport.Bestsellers.Fields.Name")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Bestsellers.Fields.TotalAmount")]
        public string TotalAmount { get; set; }

        [NopResourceDisplayName("Admin.SalesReport.Bestsellers.Fields.TotalQuantity")]
        public decimal TotalQuantity { get; set; }
    }

    public partial class BestViewProductReportLineModel : BaseNopModel
    {
        public int ProductId { get; set; }
        [NopResourceDisplayName("Admin.Product.BestViewProduct.Name")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.Product.BestViewProduct.TotalCount")]
        public int TotalViewCount { get; set; }
    }
}