namespace BNPL.Api.Server.src.Infrastructure.Services.Business
{
    public static class FinancialFormulas
    {
        public static decimal PMT(decimal monthlyRate, int periods, decimal presentValue)
        {
            if (monthlyRate == 0)
                return presentValue / periods;

            var rateFactor = (decimal)Math.Pow(1 + (double)monthlyRate, periods);
            return presentValue * monthlyRate * rateFactor / (rateFactor - 1);
        }
    }
}
