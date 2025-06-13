namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public class InstallmentOption
    {
        public int Term { get; set; }
        public decimal Value { get; set; }
        public decimal Total { get; set; }
    }
}
