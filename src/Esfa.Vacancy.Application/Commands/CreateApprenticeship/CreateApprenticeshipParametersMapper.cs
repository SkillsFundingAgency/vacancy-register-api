﻿using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipParametersMapper : ICreateApprenticeshipParametersMapper
    {
        public CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request)
        {
            return new CreateApprenticeshipParameters
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.LongDescription,
                ApplicationClosingDate = request.ApplicationClosingDate,
                ExpectedStartDate = request.ExpectedStartDate
            };
        }
    }
}