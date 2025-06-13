namespace Core.Models
{
    public static class DomainErrors
    {
        public static class Auth
        {
            public static readonly Error InvalidCredentials = new("Auth.InvalidCredentials", "Invalid username or password.");
            public static readonly Error UserNotConfirmed = new("Auth.UserNotConfirmed", "The user has not been confirmed.");
            public static readonly Error AuthenticationFailed = new("Auth.AuthenticationFailed", "Authentication failed. No token was returned.");
            public static readonly Error InvalidRefreshToken = new("Auth.InvalidRefreshToken", "The refresh token is invalid.");
            public static readonly Error ChallengeFailed = new("Auth.ChallengeFailed", "An error occurred while completing the new password challenge.");
        }

        public static class User
        {
            public static readonly Error NotFound = new("User.NotFound", "The specified user was not found.");
            public static readonly Error AlreadyExists = new("User.AlreadyExists", "A user with this email already exists.");
            public static readonly Error GenericError = new("User.GenericError", "An error occurred while processing the user request.");
        }

        public static class Group
        {
            public static readonly Error NotFound = new("Group.NotFound", "The specified group was not found.");
        }

        public static class Partner
        {
            public static readonly Error NotFound = new("Partner.NotFound", "The specified partner was not found.");
            public static readonly Error HasActiveAffiliates = new("Partner.HasActiveAffiliates", "Cannot deactivate a partner with active affiliates.");
            public static readonly Error HasActiveProposals = new("Partner.HasActiveProposals", "Cannot deactivate a partner with active or pending proposals.");
        }

        public static class Affiliate
        {
            public static readonly Error NotFound = new("Affiliate.NotFound", "The specified affiliate was not found.");
            public static readonly Error HasActiveProposals = new("Affiliate.HasActiveProposals", "Cannot deactivate an affiliate with active proposals.");
        }

        public static class Customer
        {
            public static readonly Error NotFound = new("Customer.NotFound", "The specified customer was not found.");
            public static readonly Error AlreadyExists = new("Customer.AlreadyExists", "A customer with the provided CPF already exists for this partner or affiliate.");
            public static readonly Error HasActiveProposals = new("Customer.HasActiveProposals", "The customer has active proposals and cannot be deactivated.");
        }

        public static class Simulation
        {
            public static readonly Error NotFound = new("Simulation.NotFound", "The specified simulation was not found.");
            public static readonly Error ExceedsLimit = new("Simulation.ExceedsLimit", "The requested amount exceeds the approved credit limit.");
            public static readonly Error NoInstallmentOption = new("Simulation.NoInstallmentOption", "No installment option fits within the approved credit limit.");
        }

        public static class Proposal
        {
            public static readonly Error NotFound = new("Proposal.NotFound", "The specified proposal was not found.");
            public static readonly Error MustBeActive = new("Proposal.MustBeActive", "The proposal must be in the 'Active' state.");
            public static readonly Error MustBeSigned = new("Proposal.MustBeSigned", "The proposal must be signed for this operation.");
            public static readonly Error MustBeFormalized = new("Proposal.MustBeFormalized", "The proposal must be formalized to generate the contract.");
            public static readonly Error NotEligibleForSignature = new("Proposal.NotEligibleForSignature", "The proposal is not in an eligible status for signature.");
            public static readonly Error InvalidStateForCancellation = new("Proposal.InvalidStateForCancellation", "The proposal cannot be canceled in its current state.");
            public static readonly Error InvalidStateForInactivation = new("Proposal.InvalidStateForInactivation", "The proposal cannot be deactivated in its current state.");
            public static readonly Error InvalidStatusForApproval = new("Proposal.InvalidStatusForApproval", "The proposal must be in the 'Created' state to be approved.");
            public static readonly Error HasUnpaidInstallments = new("Proposal.HasUnpaidInstallments", "The proposal still has outstanding installments.");
            public static readonly Error CancellationPeriodExpired = new("Proposal.CancellationPeriodExpired", "The cancellation period for the proposal has expired.");
            public static readonly Error InvalidInstallmentOption = new("Proposal.InvalidInstallmentOption", "The selected installment option is invalid for this simulation.");
            public static readonly Error NotFoundForCriteria = new("Proposal.NotFoundForCriteria", "No proposal was found for the provided criteria.");
            public static readonly Error NotActive = new("Proposal.NotActive", "The operation cannot be performed on an inactive proposal.");
            public static readonly Error NotEligibleForItems = new("Proposal.NotEligibleForItems", "The proposal is not in a state that allows adding items.");
            public static readonly Error NotEligibleForItemReturn = new("Proposal.NotEligibleForItemReturn", "The proposal is not in a state that allows returning items.");
            public static readonly Error PaymentsNotAllowed = new("Proposal.PaymentsNotAllowed", "Payments are only allowed for active proposals.");
        }

        public static class ProposalItem
        {
            public static readonly Error NotEligibleForConfirmation = new("ProposalItem.NotEligibleForConfirmation", "The item is not eligible for return confirmation.");
            public static readonly Error AlreadyConfirmed = new("ProposalItem.AlreadyConfirmed", "The return of this item has already been confirmed.");
            public static readonly Error ReturnPeriodExpired = new("ProposalItem.ReturnPeriodExpired", "The return period has expired.");
            public static readonly Error NoActiveItemsRemaining = new("ProposalItem.NoActiveItemsRemaining", "The proposal has no active items remaining after the return.");
            public static readonly Error InvalidStateForReturn = new("ProposalItem.InvalidStateForReturn", "The item's state is invalid for return.");
        }

        public static class Payment
        {
            public static readonly Error AmountLessThanDue = new("Payment.AmountLessThanDue", "The paid amount is less than the total due, including charges.");
        }

        public static class Installment
        {
            public static readonly Error NotFound = new("Installment.NotFound", "The installment was not found.");
            public static readonly Error AlreadyGenerated = new("Installment.AlreadyGenerated", "The installments for this proposal have already been generated.");
            public static readonly Error NoOpenInstallments = new("Installment.NoOpenInstallments", "There are no open installments available.");
            public static readonly Error AlreadyPaid = new("Installment.AlreadyPaid", "The invoice contains one or more installments that have already been paid.");
        }

        public static class Invoice
        {
            public static readonly Error NotFound = new("Invoice.NotFound", "The invoice was not found.");
            public static readonly Error AlreadyPaid = new("Invoice.AlreadyPaid", "This invoice has already been paid.");
            public static readonly Error HasNoInstallments = new("Invoice.HasNoInstallments", "The invoice has no associated installments.");
        }

        public static class Kyc
        {
            public static readonly Error NotFound = new("Kyc.NotFound", "The customer's KYC data was not found.");
            public static readonly Error AlreadyExists = new("Kyc.AlreadyExists", "KYC data for this customer already exists.");
            public static readonly Error NotCompleted = new("Kyc.NotCompleted", "The customer has not completed the identity verification (KYC).");
            public static readonly Error MissingImages = new("Kyc.MissingImages", "Document and/or selfie images are missing for face validation.");
            public static readonly Error FaceMatchFailed = new("Kyc.FaceMatchFailed", "Face match validation failed.");
            public static readonly Error NotFullyValidated = new("Kyc.NotFullyValidated", "KYC could not be validated because the OCR analysis or Face Match did not complete successfully.");
        }

        public static class General
        {
            public static readonly Error Unexpected = new("General.Unexpected", "An unexpected error occurred.");
        }

        public static class Billing
        {
            public static readonly Error NotFound = new("Billing.NotFound", "Billing preferences were not found.");
            public static readonly Error InvalidDueDay = new("Billing.InvalidDueDay", "The invoice due day must be between 1 and 28.");
        }

        public static class CreditLimit
        {
            public static readonly Error NotFound = new("CreditLimit.NotFound", "The customer's credit limit was not found.");
            public static readonly Error Insufficient = new("CreditLimit.Insufficient", "The available credit limit is insufficient.");
        }

        public static class CreditAnalysis
        {
            public static readonly Error CreditDenied = new("Credit.Denied", "Credit denied based on analysis.");
            public static readonly Error ConfigNotFound = new("Credit.ConfigNotFound", "The credit analysis configuration was not found.");
        }

        public static class FinancialCharges
        {
            public static readonly Error ConfigNotFound = new("FinancialCharges.ConfigNotFound", "The financial charges configuration was not found.");
        }

        public static class Signature
        {
            public static readonly Error InvalidToken = new("Signature.InvalidToken", "The signature token is invalid, has expired, or has already been used.");
        }
    }
}