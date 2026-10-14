
import type { PaymentMethod } from "../api/paymentApi";

interface PaymentMethodSelectorProps {
  methods: PaymentMethod[];
  selectedMethod: PaymentMethod | "";
  onSelect: (method: PaymentMethod) => void;
}

const methodDetails: Record<
  PaymentMethod,
  { title: string; description: string; icon: string }
> = {
  PayPal: {
    title: "PayPal",
    description: "Pay securely with PayPal",
    icon: "P",
  },
  ApplePay: {
    title: "Apple Pay",
    description: "Fast, secure Apple payment",
    icon: "●",
  },
  GooglePay: {
    title: "Google Pay",
    description: "Simple and secure checkout",
    icon: "G",
  },
};

export default function PaymentMethodSelector({
  methods,
  selectedMethod,
  onSelect,
}: PaymentMethodSelectorProps) {
  return (
    <div className="payment-method-list">
      {methods.map((method) => {
        const details = methodDetails[method];
        const isSelected = selectedMethod === method;

        return (
          <button
            key={method}
            type="button"
            className={`payment-method-card ${
              isSelected ? "selected" : ""
            }`}
            onClick={() => onSelect(method)}
            aria-pressed={isSelected}
          >
            <span
              className={`payment-method-icon ${method.toLowerCase()}`}
              aria-hidden="true"
            >
              {details.icon}
            </span>

            <span className="payment-method-content">
              <span className="payment-method-title">
                {details.title}
              </span>

              <span className="payment-method-description">
                {details.description}
              </span>
            </span>

            <span
              className={`selection-radio ${
                isSelected ? "checked" : ""
              }`}
              aria-hidden="true"
            />
          </button>
        );
      })}
    </div>
  );
}