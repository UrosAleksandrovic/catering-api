using Catering.Application.Aggregates.Identites.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Catering.Api.Controllers;

[Route("/api/companyEmployees")]
public class CompanyEmployeesController : ControllerBase
{
    private readonly ICustomerManagementAppService _customerAppService;

    public CompanyEmployeesController(ICustomerManagementAppService customerAppService)
    {
        _customerAppService = customerAppService;
    }


}
