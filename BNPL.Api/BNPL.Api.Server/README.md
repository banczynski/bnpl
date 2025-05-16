# BNPL - Fluxo Detalhado da Jornada

Este documento descreve o **fluxo completo da jornada BNPL (Buy Now, Pay Later)** de forma **conceitual e funcional**, com destaque para os pontos de integração, eventos importantes, validações de negócio e os caminhos dos UseCases relacionados em cada etapa.

---

## 🔄 Visão Geral do Fluxo

1. **Simulação de crédito**  
2. **Cadastro do cliente**  
3. **KYC (OCR + Face Match)**  
4. **Criação da proposta + itens**  
5. **Assinatura eletrônica**  
6. **Formalização e liberação**  
7. **Parcelas e cobrança**  
8. **Pagamento e finalização**  
9. **Devolução e renegociação (quando aplicável)**

Cada etapa aciona diferentes serviços externos ou processos internos, conforme descrito abaixo.

---

## 1. Simulação de Crédito

📆 **Quando:** logo após o cliente informar CPF e valor desejado  
🛠️ **O que faz:**
- Consulta serviço externo de análise de crédito  
- Retorna valor aprovado, parcelas, juros, CET  
- **Atualiza ou cria o limite de crédito do cliente** com o valor aprovado

🔐 **Controle de limite:**  
- Entidade: `CustomerCreditLimit`  
- Campos: `ApprovedLimit`, `UsedLimit`, `AvailableLimit`  
- O limite é **criado ou atualizado** aqui via `UpsertCustomerCreditLimitUseCase`

**UseCases:**  
- `CreateSimulationUseCase`  
- `UpsertCustomerCreditLimitUseCase`  
**Rota:** `POST /api/simulation`

---

## 2. Cadastro do Cliente

📆 **Quando:** após simulação aprovada  
🛠️ **O que faz:** cria registro de cliente com dados pessoais e flag `SkipKyc` se KYC já realizado pela Amazon

**UseCase:** `CreateCustomerUseCase`  
**Rota:** `POST /api/customer`

---

## 3. Validação de Identidade (KYC)

📆 **Quando:** obrigatório para clientes que não vieram com `SkipKyc`  
🛠️ **O que faz:**
- Realiza OCR da imagem do documento  
- Valida a selfie com o documento (face match)  
- Marca como validado se ambos forem positivos

**UseCases:**  
- `AnalyzeCustomerDocumentUseCase`  
- `ValidateFaceMatchUseCase`  
- `ValidateKycStatusUseCase`

---

## 4. Criação da Proposta + Itens

📆 **Quando:** após KYC validado  
🛠️ **O que faz:**
- Cria proposta com valor, parcelas, cliente e simulação vinculada  
- Cria itens separados (um para cada produto/afiliado)  
- Verifica se a soma dos itens bate com o valor aprovado

**UseCases:**  
- `CreateProposalUseCase` → `POST /api/proposal`  
- `CreateProposalItemUseCase` → `POST /api/proposal/{id}/item`  
- `CreateProposalItemsUseCase` → `POST /api/proposal/{id}/items`  
- `MarkProposalAsSignedUseCase`

> 🧮 Ao assinar a proposta, o valor aprovado é **comprometido no limite de crédito** (`UsedLimit += valor`) via `AdjustCustomerCreditLimitUseCase`. Caso o saldo seja insuficiente, a assinatura é bloqueada.

---

## 5. Assinatura Eletrônica

📆 **Quando:** proposta pronta para assinatura  
🛠️ **O que faz:**
- Gera link externo (GovBR, Clicksign etc.)  
- Aguarda callback do serviço de assinatura  
- Atualiza status da proposta e cria parcelas

**UseCases:**  
- `GenerateSignatureLinkUseCase`  
- `ProcessSignatureCallbackUseCase`

---

## 6. Formalização e Disbursement

📆 **Quando:** após assinatura  
🛠️ **O que faz:**
- Gera contrato final PDF com hash e QR code (serviço externo)  
- Formaliza proposta e libera valor

**UseCases:**  
- `GenerateFinalContractUseCase`  
- `FormalizeProposalUseCase`  
- `MarkProposalAsDisbursedUseCase`

---

## 7. Parcelamento e Fatura

📆 **Quando:** logo após assinatura  
🛠️ **O que faz:**
- Gera parcelas com datas futuras  
- Agrupa parcelas em faturas (manual ou em lote)  
- Gera link de pagamento com juros/multa se aplicável

**UseCases:**  
- `MarkProposalAsSignedUseCase` (gera parcelas)  
- `GenerateInvoiceBatchUseCase` ou `CreateInvoiceUseCase`  
- `GenerateInvoicePaymentLinkUseCase`

---

## 8. Pagamento e Finalização

📆 **Quando:** cliente paga via Pix, boleto ou link  
🛠️ **O que faz:**
- Atualiza status da fatura  
- Atualiza parcelas vinculadas  
- Se todas parcelas forem quitadas, finaliza proposta  
- Libera o valor no limite de crédito novamente

> 💳 Ao quitar a fatura, o valor é **liberado novamente no limite** (`UsedLimit -= valor`). Isso permite reutilização do saldo para novas compras.

**UseCase:** `ProcessPaymentCallbackUseCase`

---

## 9. Devolução e Renegociação

📆 **Quando:** cliente devolve um ou mais produtos  
🛠️ **O que faz:**
- Marca item como retornado  
- Cria nova estrutura de parcelas e fatura  
- Cancela as anteriores

**UseCases:**  
- `MarkProposalItemAsReturnedUseCase`  
- `CreateRenegotiationFromReturnedItemUseCase`  
- `ConfirmRenegotiationUseCase`

---

## ✅ Funcionalidades Implementadas

- ✅ Jornada completa até formalização e disbursement
- ✅ Inserção de itens únicos ou múltiplos via `ProposalItem`
- ✅ Validações de valor aprovado vs itens
- ✅ Geração automática de parcelas
- ✅ Geração e atualização de fatura
- ✅ Finalização automática após quitação
- ✅ Renegociação por devolução
- ✅ Controle de limite de crédito (criação, débito e liberação)