
interface StepIndicatorProps {
  currentStep: 1 | 2 | 3;
}

const steps = [
  { number: 1, title: "Service" },
  { number: 2, title: "Payment" },
  { number: 3, title: "Complete" },
] as const;

export default function StepIndicator({
  currentStep,
}: StepIndicatorProps) {
  return (
    <div className="step-indicator">
      {steps.map((step, index) => {
        const isCompleted = step.number < currentStep;
        const isActive = step.number === currentStep;

        return (
          <div className="step-wrapper" key={step.number}>
            <div className="step-item">
              <div
                className={[
                  "step-circle",
                  isActive ? "active" : "",
                  isCompleted ? "completed" : "",
                ]
                  .filter(Boolean)
                  .join(" ")}
                aria-current={isActive ? "step" : undefined}
              >
                {isCompleted ? "✓" : step.number}
              </div>

              <span
                className={`step-title ${
                  isActive ? "active" : ""
                }`}
              >
                {step.title}
              </span>
            </div>

            {index < steps.length - 1 && (
              <div
                className={`step-connector ${
                  step.number < currentStep ? "completed" : ""
                }`}
                aria-hidden="true"
              />
            )}
          </div>
        );
      })}
    </div>
  );
}