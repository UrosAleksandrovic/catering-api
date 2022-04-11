using Ardalis.GuardClauses;
using Catering.Domain.Exceptions;

namespace Catering.Domain.Entities.UserAggregate;

public class User : BaseEntity<string>
{
    public string Email { get; private set; }
    public UserRole Role { get; private set; }
    
    public UserBudget Budget { get; private set; }

    private User() { }

    public User(string id, string email, bool isRestourantEmployee)
    {
        Guard.Against.NullOrWhiteSpace(id);
        Guard.Against.NullOrWhiteSpace(email);

        Id = id;
        Email = email;
        Role = isRestourantEmployee ? UserRole.RestourantEmployee : UserRole.CompanyEmployee;

        if (!isRestourantEmployee)
            Budget = new UserBudget(id, 0);
    }

    public bool IsComapnyEmployee => Role != UserRole.RestourantEmployee;

    public void ResetBudget(decimal newBalance)
    {
        ConfirmCompanyEmployee();

        Budget.SetBalance(newBalance);
    }

    public void ReserveAssets(decimal amountToReserve)
    {
        ConfirmCompanyEmployee();

        Budget.Reserve(amountToReserve);
    }

    public void ProcessPayment(decimal amountToPay)
    {
        ConfirmCompanyEmployee();

        Budget.Remove(amountToPay);
    }

    public void CancelPayment(decimal amountToCancel)
    {
        ConfirmCompanyEmployee();

        Budget.CancelReservation(amountToCancel);
    } 

    public void MakeAdministrator(User userToBecomeAdministrator)
    {
        if (Role != UserRole.CompanyAdministrator)
            throw new ActionNotAllowedException(nameof(MakeAdministrator));

        userToBecomeAdministrator.BecomeAdministrator();
    }

    private void BecomeAdministrator()
    {
        if (Role != UserRole.CompanyEmployee)
            throw new ActionNotAllowedException(nameof(MakeAdministrator));

        Role = UserRole.CompanyAdministrator;
    }

    private void ConfirmCompanyEmployee()
    {
        if (!IsComapnyEmployee)
            throw new RestourantNotAllowedActionException();
    }
}
