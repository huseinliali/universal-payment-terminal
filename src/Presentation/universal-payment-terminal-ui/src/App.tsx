
import { useEffect, useState } from "react";
import {
  paymentApi,
  type PaymentMethod,
  type ServiceCategory,
  type ServiceValidationResult,
  type PayResult,
} from "./api/paymentApi";

import ServiceSelector from "./components/ServiceSelector";
import PaymentMethodSelector from "./components/PaymentMethodSelector";
import StepIndicator from "./components/StepIndicator";

type CheckoutStep = 1 | 2 | 3;

const categoryLabels: Record<string, string> = {
  Education: "Education",
  Utility: "Utilities",
  Bank: "Bank loan",
  Fine: "Fines",
};

const identifierLabels: Record<string, string> = {
  Education: "Student code",
  Utility: "Subscriber code",
  Bank: "Loan number",
  Fine: "Fine number",
};

const identifierPlaceholders: Record<string, string> = {
  Education: "e.g. STU123",
  Utility: "e.g. UTIL123",
  Bank: "e.g. LOAN123",
  Fine: "e.g. FINE123",
};

function App() {
  const [categories, setCategories] = useState<ServiceCategory[]>([]);
  const [methods, setMethods] = useState<PaymentMethod[]>([]);

  const [selectedCategory, setSelectedCategory] =
    useState<ServiceCategory | null>(null);

  const [customerIdentifier, setCustomerIdentifier] = useState("");
  const [validation, setValidation] =
    useState<ServiceValidationResult | null>(null);

  const [selectedMethod, setSelectedMethod] =
    useState<PaymentMethod | "">("");

  const [currentStep, setCurrentStep] = useState<CheckoutStep>(1);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const [paymentReference, setPaymentReference] = useState("");
  const [paymentResult, setPaymentResult] =
  useState<PayResult | null>(null);
  useEffect(() => {
    async function loadCheckoutOptions() {
      try {
        const [categoryData, methodData] = await Promise.all([
          paymentApi.getCategories(),
          paymentApi.getMethods(),
        ]);

        setCategories(categoryData);
        setMethods(methodData);
      } catch {
        setError(
          "Unable to load checkout options. Check that the API is running and the URL is correct."
        );
      }
    }

    void loadCheckoutOptions();
  }, []);

  function handleCategorySelect(category: ServiceCategory) {
    setSelectedCategory(category);
    setCustomerIdentifier("");
    setValidation(null);
    setSelectedMethod("");
    setCurrentStep(1);
    setError("");
    setPaymentReference("");
  }

  async function handleValidate() {
    if (!selectedCategory) {
      setError("Please select a service.");
      return;
    }

    if (!customerIdentifier.trim()) {
      setError("Please enter your account identifier.");
      return;
    }

    setIsLoading(true);
    setError("");
    setValidation(null);

    try {
      const result = await paymentApi.validateService(
        selectedCategory.providerId,
        customerIdentifier.trim()
      );

      if (!result.isValid) {
        setError("Account not found. Please check your identifier.");
        return;
      }

      setValidation(result);
      setCurrentStep(2);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Unable to validate this account."
      );
    } finally {
      setIsLoading(false);
    }
  }

  
async function handlePayment() {
  if (!selectedCategory || !validation || !selectedMethod) {
    setError("Please complete all payment details.");
    return;
  }

  setIsLoading(true);
  setError("");

  try {
    const result = await paymentApi.pay({
      providerId: selectedCategory.providerId,
      paymentMethod: selectedMethod,
      amount: validation.outstandingAmount,
      currency: validation.currency,
    });

    setPaymentResult(result);

    setPaymentReference(
      `DEMO-${Date.now().toString().slice(-8)}`
    );

    setCurrentStep(3);
  } catch (err) {
    setError(
      err instanceof Error
        ? err.message
        : "Unable to process this payment."
    );
  } finally {
    setIsLoading(false);
  }
}


  function startNewPayment() {
    setSelectedCategory(null);
    setCustomerIdentifier("");
    setValidation(null);
    setSelectedMethod("");
    setCurrentStep(1);
    setError("");
    setPaymentReference("");
  }

  const categoryName = selectedCategory
    ? categoryLabels[selectedCategory.category] ??
      selectedCategory.category
    : "Select a service";

  return (
    <main className="app-shell">
      <header className="topbar">
        <a
          className="brand"
          href="/"
          onClick={(event) => event.preventDefault()}
        >
          <span className="brand-mark">N</span>
          <span>NexaPay</span>
        </a>

        <div className="secure-label">
          <span className="secure-dot" />
          Secure checkout demo
        </div>
      </header>

      <section className="checkout-layout">
        <div className="checkout-main">
          <div className="page-heading">
            <p className="eyebrow">UNIVERSAL PAYMENT TERMINAL</p>
            <h1>Payments, made simple.</h1>
            <p className="page-subtitle">
              Pay your bills in a few simple steps.
            </p>
          </div>

          <StepIndicator currentStep={currentStep} />

          {error && (
            <div className="error-message" role="alert">
              <span aria-hidden="true">!</span>
              {error}
            </div>
          )}

          {currentStep === 1 && (
            <section className="checkout-section">
              <div className="section-heading">
                <div>
                  <span className="section-kicker">STEP 01</span>
                  <h2>Choose your service</h2>
                  <p>Select the type of payment you want to make.</p>
                </div>
              </div>

              {categories.length === 0 ? (
                <div className="loading-panel">
                  Loading services...
                </div>
              ) : (
                <ServiceSelector
                  categories={categories}
                  selectedProviderId={
                    selectedCategory?.providerId ?? ""
                  }
                  onSelect={handleCategorySelect}
                />
              )}

              {selectedCategory && (
                <div className="account-form">
                  <label htmlFor="customerIdentifier">
                    {identifierLabels[selectedCategory.category] ??
                      "Account identifier"}
                  </label>

                  <input
                    id="customerIdentifier"
                    type="text"
                    value={customerIdentifier}
                    onChange={(event) => {
                      setCustomerIdentifier(event.target.value);
                      setError("");
                    }}
                    placeholder={
                      identifierPlaceholders[
                        selectedCategory.category
                      ] ?? "Enter your identifier"
                    }
                    autoComplete="off"
                  />

                  <button
                    className="primary-button"
                    type="button"
                    onClick={handleValidate}
                    disabled={isLoading}
                  >
                    {isLoading ? "Checking account..." : "Continue"}
                    {!isLoading && <span aria-hidden="true">→</span>}
                  </button>
                </div>
              )}
            </section>
          )}

          {currentStep === 2 && validation && (
            <section className="checkout-section">
              <div className="section-heading">
                <div>
                  <span className="section-kicker">STEP 02</span>
                  <h2>Choose payment method</h2>
                  <p>
                    Select how you want to complete your demo payment.
                  </p>
                </div>
              </div>

              <div className="account-summary">
                <div className="summary-avatar">
                  {validation.customerName.charAt(0).toUpperCase()}
                </div>

                <div className="summary-customer">
                  <span>Account verified</span>
                  <strong>{validation.customerName}</strong>
                  <small>{categoryName}</small>
                </div>

                <button
                  type="button"
                  className="text-button"
                  onClick={() => {
                    setCurrentStep(1);
                    setError("");
                  }}
                >
                  Edit
                </button>
              </div>

              <div className="amount-panel">
                <span>Outstanding balance</span>
                <strong>
                  {new Intl.NumberFormat("en-IE", {
                    style: "currency",
                    currency: validation.currency,
                  }).format(validation.outstandingAmount)}
                </strong>
                <small>Amount to pay</small>
              </div>

              <h3 className="subsection-title">Payment method</h3>

              <PaymentMethodSelector
                methods={methods}
                selectedMethod={selectedMethod}
                onSelect={setSelectedMethod}
              />

              <button
                className="primary-button"
                type="button"
                onClick={handlePayment}
                disabled={isLoading || !selectedMethod}
              >
                {isLoading
                  ? "Processing demo payment..."
                  : "Confirm payment"}
                {!isLoading && <span aria-hidden="true">→</span>}
              </button>

              <button
                className="back-button"
                type="button"
                onClick={() => {
                  setCurrentStep(1);
                  setError("");
                }}
                disabled={isLoading}
              >
                ← Back to services
              </button>

              <p className="demo-disclaimer">
                This is a demonstration. No real money is transferred.
              </p>
            </section>
          )}

          {currentStep === 3 && (
            <section className="success-panel">
              <div className="success-icon" aria-hidden="true">
                ✓
              </div>

              <span className="section-kicker">PAYMENT COMPLETE</span>
              <h2>You're all set!</h2>
              <p>
                Your demo payment for {categoryName.toLowerCase()} has
                been completed successfully.
              </p>

              {validation && (
                <div className="success-amount">
                  {new Intl.NumberFormat("en-IE", {
                    style: "currency",
                    currency: validation.currency,
                  }).format(validation.outstandingAmount)}
                </div>
              )}

              {paymentResult && validation && (
                <>
                  <div className="reference-row">
                    <span>Commission</span>
                    <strong>
                      {new Intl.NumberFormat("en-IE", {
                        style: "currency",
                        currency: validation.currency,
                      }).format(paymentResult.commissionAmount)}
                    </strong>
                  </div>

                  <div className="reference-row">
                    <span>Total Amount</span>
                    <strong>
                      {new Intl.NumberFormat("en-IE", {
                        style: "currency",
                        currency: validation.currency,
                      }).format(paymentResult.totalAmount)}
                    </strong>
                  </div>
                </>
              )}

              <div className="reference-row">
                <span>Demo reference</span>
                <strong>{paymentReference}</strong>
              </div>

              <div className="reference-row">
                <span>Payment method</span>
                <strong>{paymentResult?.paymentMethod ?? selectedMethod}</strong>
              </div>

              <p className="demo-disclaimer">
                Demonstration only — this is not a real payment receipt.
              </p>

              <button
                className="primary-button"
                type="button"
                onClick={startNewPayment}
              >
                Make another payment
                <span aria-hidden="true">→</span>
              </button>
            </section>
          )}
        </div>

        <aside className="checkout-sidebar">
          <div className="sidebar-decoration">
            <div className="decoration-orb orb-one" />
            <div className="decoration-orb orb-two" />

            <div className="sidebar-content">
              <span className="sidebar-badge">ONE PLACE. EVERY PAYMENT.</span>
              <h2>Your payments, all in one place.</h2>
              <p>
                A simpler way to manage your everyday payments with a
                streamlined checkout experience.
              </p>

              <div className="sidebar-feature">
                <span>✓</span>
                <div>
                  <strong>Simple checkout</strong>
                  <small>Just a few easy steps</small>
                </div>
              </div>

              <div className="sidebar-feature">
                <span>✓</span>
                <div>
                  <strong>Clear payment details</strong>
                  <small>Review your balance before continuing</small>
                </div>
              </div>
            </div>
          </div>

          <div className="sidebar-footer">
            <span>DEMO ENVIRONMENT</span>
            <span>© 2026 NexaPay</span>
          </div>
        </aside>
      </section>

      <footer className="app-footer">
        <span>Universal Payment Terminal</span>
        <span>Educational project · No real transactions</span>
      </footer>
    </main>
  );
}

export default App;