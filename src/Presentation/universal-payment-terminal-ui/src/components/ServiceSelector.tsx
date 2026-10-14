
import type { ServiceCategory } from "../api/paymentApi";

interface ServiceSelectorProps {
  categories: ServiceCategory[];
  selectedProviderId: string;
  onSelect: (category: ServiceCategory) => void;
}

const serviceDetails: Record<
  string,
  { title: string; description: string; icon: string }
> = {
  Education: {
    title: "Education",
    description: "Tuition and student payments",
    icon: "🎓",
  },
  Utility: {
    title: "Utilities",
    description: "Electricity, water and other bills",
    icon: "⚡",
  },
  Bank: {
    title: "Bank loan",
    description: "Loan and credit payments",
    icon: "🏦",
  },
  Fine: {
    title: "Fines",
    description: "Check and pay outstanding fines",
    icon: "🚘",
  },
};

export default function ServiceSelector({
  categories,
  selectedProviderId,
  onSelect,
}: ServiceSelectorProps) {
  return (
    <div className="service-grid">
      {categories.map((category) => {
        const details = serviceDetails[category.category] ?? {
          title: category.category,
          description: "View and pay outstanding balance",
          icon: "💳",
        };

        const isSelected =
          selectedProviderId === category.providerId;

        return (
          <button
            key={category.providerId}
            type="button"
            className={`service-card ${
              isSelected ? "selected" : ""
            }`}
            onClick={() => onSelect(category)}
            aria-pressed={isSelected}
          >
            <span className="service-icon" aria-hidden="true">
              {details.icon}
            </span>

            <span className="service-card-content">
              <span className="service-title">
                {details.title}
              </span>

              <span className="service-description">
                {details.description}
              </span>
            </span>

            <span className="service-check" aria-hidden="true">
              {isSelected ? "✓" : "↗"}
            </span>
          </button>
        );
      })}
    </div>
  );
}
