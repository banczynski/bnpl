namespace Core.Models
{
    public static class DomainErrors
    {
        public static class Auth
        {
            public static readonly Error InvalidCredentials = new("Auth.InvalidCredentials", "Usuário ou senha inválidos.");
            public static readonly Error UserNotConfirmed = new("Auth.UserNotConfirmed", "O usuário não foi confirmado.");
            public static readonly Error AuthenticationFailed = new("Auth.AuthenticationFailed", "A autenticação falhou. Nenhum token foi retornado.");
            public static readonly Error InvalidRefreshToken = new("Auth.InvalidRefreshToken", "O token de atualização (refresh token) é inválido.");
            public static readonly Error ChallengeFailed = new("Auth.ChallengeFailed", "Ocorreu um erro ao completar o desafio de nova senha.");
        }

        public static class User
        {
            public static readonly Error NotFound = new("User.NotFound", "O usuário especificado não foi encontrado.");
            public static readonly Error AlreadyExists = new("User.AlreadyExists", "Um usuário com este e-mail já existe.");
            public static readonly Error GenericError = new("User.GenericError", "Ocorreu um erro ao processar a solicitação do usuário.");
        }

        public static class Group
        {
            public static readonly Error NotFound = new("Group.NotFound", "O grupo especificado não foi encontrado.");
        }

        public static class Partner
        {
            public static readonly Error NotFound = new("Partner.NotFound", "O parceiro especificado não foi encontrado.");
            public static readonly Error HasActiveAffiliates = new("Partner.HasActiveAffiliates", "Não é possível inativar um parceiro que possui afiliados ativos.");
            public static readonly Error HasActiveProposals = new("Partner.HasActiveProposals", "Não é possível inativar um parceiro com propostas ativas ou pendentes.");
        }

        public static class Affiliate
        {
            public static readonly Error NotFound = new("Affiliate.NotFound", "O afiliado especificado não foi encontrado.");
            public static readonly Error HasActiveProposals = new("Affiliate.HasActiveProposals", "Não é possível inativar um afiliado com propostas ativas.");
        }

        public static class Customer
        {
            public static readonly Error NotFound = new("Customer.NotFound", "O cliente especificado não foi encontrado.");
            public static readonly Error AlreadyExists = new("Customer.AlreadyExists", "Um cliente com o CPF fornecido já existe para este parceiro ou afiliado.");
            public static readonly Error HasActiveProposals = new("Customer.HasActiveProposals", "O cliente possui propostas ativas e não pode ser inativado.");
        }

        public static class Simulation
        {
            public static readonly Error NotFound = new("Simulation.NotFound", "A simulação especificada não foi encontrada.");
            public static readonly Error ExceedsLimit = new("Simulation.ExceedsLimit", "O valor solicitado excede o limite de crédito aprovado.");
            public static readonly Error NoInstallmentOption = new("Simulation.NoInstallmentOption", "Nenhuma opção de parcelamento se encaixa no limite de crédito aprovado.");
        }

        public static class Proposal
        {
            public static readonly Error NotFound = new("Proposal.NotFound", "A proposta especificada não foi encontrada.");
            public static readonly Error MustBeActive = new("Proposal.MustBeActive", "A proposta precisa estar no estado 'Ativa'.");
            public static readonly Error MustBeSigned = new("Proposal.MustBeSigned", "A proposta precisa estar assinada para esta operação.");
            public static readonly Error MustBeFormalized = new("Proposal.MustBeFormalized", "A proposta precisa estar formalizada para gerar o contrato.");
            public static readonly Error NotEligibleForSignature = new("Proposal.NotEligibleForSignature", "A proposta não está em um status elegível para assinatura.");
            public static readonly Error InvalidStateForCancellation = new("Proposal.InvalidStateForCancellation", "A proposta não pode ser cancelada no estado atual.");
            public static readonly Error InvalidStateForInactivation = new("Proposal.InvalidStateForInactivation", "A proposta não pode ser inativada no estado atual.");
            public static readonly Error InvalidStatusForApproval = new("Proposal.InvalidStatusForApproval", "A proposta precisa estar no estado 'Criada' para ser aprovada.");
            public static readonly Error HasUnpaidInstallments = new("Proposal.HasUnpaidInstallments", "A proposta ainda possui parcelas pendentes de pagamento.");
            public static readonly Error CancellationPeriodExpired = new("Proposal.CancellationPeriodExpired", "O período para cancelamento da proposta expirou.");
            public static readonly Error InvalidInstallmentOption = new("Proposal.InvalidInstallmentOption", "A opção de parcelamento selecionada é inválida para esta simulação.");
            public static readonly Error NotFoundForCriteria = new("Proposal.NotFoundForCriteria", "Nenhuma proposta foi encontrada para os critérios fornecidos.");
            public static readonly Error NotActive = new("Proposal.NotActive", "A operação não pode ser realizada em uma proposta inativa.");
            public static readonly Error NotEligibleForItems = new("Proposal.NotEligibleForItems", "A proposta não está em um estado que permita a adição de itens.");
            public static readonly Error NotEligibleForItemReturn = new("Proposal.NotEligibleForItemReturn", "A proposta não está em um estado que permita a devolução de itens.");
            public static readonly Error PaymentsNotAllowed = new("Proposal.PaymentsNotAllowed", "Pagamentos só são permitidos para propostas ativas.");
        }

        public static class ProposalItem
        {
            public static readonly Error NotEligibleForConfirmation = new("ProposalItem.NotEligibleForConfirmation", "O item não está elegível para confirmação de devolução.");
            public static readonly Error AlreadyConfirmed = new("ProposalItem.AlreadyConfirmed", "A devolução deste item já foi confirmada.");
            public static readonly Error ReturnPeriodExpired = new("ProposalItem.ReturnPeriodExpired", "O período para devolução expirou.");
            public static readonly Error NoActiveItemsRemaining = new("ProposalItem.NoActiveItemsRemaining", "A proposta não possui mais itens ativos após a devolução.");
            public static readonly Error InvalidStateForReturn = new("ProposalItem.InvalidStateForReturn", "O estado do item é inválido para devolução.");
        }

        public static class Payment
        {
            public static readonly Error AmountLessThanDue = new("Payment.AmountLessThanDue", "O valor pago é inferior ao total devido, incluindo encargos.");
        }

        public static class Installment
        {
            public static readonly Error NotFound = new("Installment.NotFound", "A parcela não foi encontrada.");
            public static readonly Error AlreadyGenerated = new("Installment.AlreadyGenerated", "As parcelas para esta proposta já foram geradas.");
            public static readonly Error NoOpenInstallments = new("Installment.NoOpenInstallments", "Não existem parcelas em aberto disponíveis.");
            public static readonly Error AlreadyPaid = new("Installment.AlreadyPaid", "A fatura contém uma ou mais parcelas que já foram pagas.");
        }

        public static class Invoice
        {
            public static readonly Error NotFound = new("Invoice.NotFound", "A fatura não foi encontrada.");
            public static readonly Error AlreadyPaid = new("Invoice.AlreadyPaid", "Esta fatura já foi paga.");
            public static readonly Error HasNoInstallments = new("Invoice.HasNoInstallments", "A fatura não possui parcelas associadas.");
        }

        public static class Kyc
        {
            public static readonly Error NotFound = new("Kyc.NotFound", "Os dados de KYC do cliente não foram encontrados.");
            public static readonly Error AlreadyExists = new("Kyc.AlreadyExists", "Os dados de KYC para este cliente já existem.");
            public static readonly Error NotCompleted = new("Kyc.NotCompleted", "O cliente não completou a validação de identidade (KYC).");
            public static readonly Error MissingImages = new("Kyc.MissingImages", "As imagens do documento e/ou selfie estão ausentes para a validação facial.");
            public static readonly Error FaceMatchFailed = new("Kyc.FaceMatchFailed", "A validação de correspondência facial falhou.");
            public static readonly Error NotFullyValidated = new("Kyc.NotFullyValidated", "O KYC não pôde ser validado pois a análise de OCR ou o Face Match não foram concluídos com sucesso.");
        }

        public static class General
        {
            public static readonly Error Unexpected = new("General.Unexpected", "Ocorreu um erro inesperado.");
        }

        public static class Billing
        {
            public static readonly Error NotFound = new("Billing.NotFound", "As preferências de faturamento não foram encontradas.");
            public static readonly Error InvalidDueDay = new("Billing.InvalidDueDay", "O dia de vencimento da fatura deve ser entre 1 e 28.");
        }

        public static class CreditLimit
        {
            public static readonly Error NotFound = new("CreditLimit.NotFound", "O limite de crédito do cliente não foi encontrado.");
            public static readonly Error Insufficient = new("CreditLimit.Insufficient", "O limite de crédito disponível é insuficiente.");
        }

        public static class CreditAnalysis
        {
            public static readonly Error CreditDenied = new("Credit.Denied", "Crédito negado com base na análise.");
            public static readonly Error ConfigNotFound = new("Credit.ConfigNotFound", "A configuração de análise de crédito não foi encontrada.");
        }

        public static class FinancialCharges
        {
            public static readonly Error ConfigNotFound = new("FinancialCharges.ConfigNotFound", "A configuração de encargos financeiros não foi encontrada.");
        }

        public static class Signature
        {
            public static readonly Error InvalidToken = new("Signature.InvalidToken", "O token de assinatura é inválido, expirou ou já foi utilizado.");
        }
    }
}
