﻿using Catering.Application.Aggregates.Identities.Abstractions;
using Catering.Application.Aggregates.Identities.Dtos;

namespace Catering.Application.Aggregates.Identities;

internal class CustomerReportsAppService : ICustomerReportsAppService
{
    private readonly ICustomerReportsRepository _customerReportsRepository;

    public CustomerReportsAppService(ICustomerReportsRepository customerReportsRepository)
    {
        _customerReportsRepository = customerReportsRepository;
    }

    public Task<List<CustomerMonthlySpendingDto>> GetMonthlySendingAsync(int month, int year)
        => _customerReportsRepository.GetMonthlySendingReportAsync(month, year);
}