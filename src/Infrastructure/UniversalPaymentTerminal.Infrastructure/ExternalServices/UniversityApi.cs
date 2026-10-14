namespace UniversalPaymentTerminal.Infrastructure.ExternalServices;

public class UniversityApi
{
    public Task<(string Name, decimal Debt)?> GetStudentAsync(
       string studentCode)
    {
        if (studentCode != "STU123")
            return Task.FromResult<
                (string Name, decimal Debt)?>(null);

        return Task.FromResult<
            (string Name, decimal Debt)?>(
                ("Ali H.", 50m));
    }
}
public class BankApi
{
    public Task<(string Name, decimal Debt)?> GetLoanAsync(
        string loanNumber)
    {
        if (loanNumber != "LOAN123")
            return Task.FromResult<
                (string Name, decimal Debt)?>(null);

        return Task.FromResult<
            (string Name, decimal Debt)?>(
                ("Customer A", 200m));
    }
}

public class UtilityApi
{
    public Task<(string Name, decimal Debt)?> GetUtilityAsync(
        string loanNumber)
    {
        if (loanNumber != "UTIL123")
            return Task.FromResult<
                (string Name, decimal Debt)?>(null);

        return Task.FromResult<
            (string Name, decimal Debt)?>(
                ("Address A", 150m));
    }
}

public class FineApi
{
    public Task<(string Name, decimal Debt)?> GetFineAsync(
       string loanNumber)
    {
        if (loanNumber != "FINE123")
            return Task.FromResult<
                (string Name, decimal Debt)?>(null);

        return Task.FromResult<
            (string Name, decimal Debt)?>(
                ("Citizen A", 150m));
    }
}