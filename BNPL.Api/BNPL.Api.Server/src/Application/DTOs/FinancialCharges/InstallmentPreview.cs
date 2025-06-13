namespace BNPL.Api.Server.src.Application.DTOs.FinancialCharges
{
    public class InstallmentPreview
    {
        public int Sequence { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
    }
}
