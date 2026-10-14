const API_BASE_URL = (
  import.meta.env.VITE_API_BASE_URL ?? "https://localhost:7001"
).replace(/\/$/, "");

export interface ServiceCategory {
  category: string;
  providerId: string;
}

export interface ServiceValidationResult {
  isValid: boolean;
  customerName: string;
  outstandingAmount: number;
  currency: string;
}

export type PaymentMethod = "PayPal" | "ApplePay" | "GooglePay";

export interface PayRequest {
  providerId: string;
  paymentMethod: PaymentMethod;
  amount: number;
  currency: string;
}
export interface PayResult {
  providerId: string;
  paymentMethod: PaymentMethod;
  amount: number;
  currency: string;
  commissionAmount: number;
  totalAmount: number;
}
async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...options?.headers,
    },
  });

  if (!response.ok) {
    const message = await response.text();
    throw new Error(message || `API request failed (${response.status})`);
  }

  return response.json() as Promise<T>;
}

export const paymentApi = {
  getCategories: () => request<ServiceCategory[]>("/api/payment-categories"),

  getMethods: () => request<PaymentMethod[]>("/api/payment-methods"),

  validateService: (providerId: string, customerIdentifier: string) =>
    request<ServiceValidationResult>("/api/service-payments/validate", {
      method: "POST",
      body: JSON.stringify({
        providerId,
        customerIdentifier,
      }),
    }),

  pay: (payload: PayRequest) =>
    request<PayResult>("/api/payments", {
      method: "POST",
      body: JSON.stringify(payload),
    }),
};
