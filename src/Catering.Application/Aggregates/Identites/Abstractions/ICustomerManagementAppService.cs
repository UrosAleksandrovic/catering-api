﻿using Catering.Application.Aggregates.Identites.Dtos;

namespace Catering.Application.Aggregates.Identites.Abstractions;

public interface ICustomerManagementAppService
{
    Task<string> CreateCompanyCustomer(CreateCustomerDto createCustomer, string creatorId);
    Task<CustomerInfoDto> GetCustomerInfo(string customerId);
    Task<CustomerBudgetInfoDto> GetCustomerBudgetInfo(string customerId);
}
