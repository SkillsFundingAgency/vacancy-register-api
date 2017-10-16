﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class VacancySearchParametersConverter : IVacancySearchParametersConverter
    {
        private readonly IStandardCodeConverter _standardCodeConverter;
        private readonly IFrameworkCodeConverter _frameworkCodeConverter;

        public VacancySearchParametersConverter(IStandardCodeConverter standardCodeConverter, IFrameworkCodeConverter frameworkCodeConverter)
        {
            _standardCodeConverter = standardCodeConverter;
            _frameworkCodeConverter = frameworkCodeConverter;
        }

        public async Task<VacancySearchParameters> ConvertFrom(SearchApprenticeshipVacanciesRequest request)
        {
            var combinedSubCategoryCodes = new List<string>();
            var combinedValidationFailures = new List<ValidationFailure>();

            var convertedStandards = await _standardCodeConverter.ConvertAsync(request.StandardCodes);
            var convertedFrameworks = await _frameworkCodeConverter.ConvertAsync(request.FrameworkCodes);

            combinedSubCategoryCodes.AddRange(convertedStandards.SubCategoryCodes);
            combinedSubCategoryCodes.AddRange(convertedFrameworks.SubCategoryCodes);

            combinedValidationFailures.AddRange(convertedStandards.ValidationFailures);
            combinedValidationFailures.AddRange(convertedFrameworks.ValidationFailures);

            if (combinedValidationFailures.Any())
                throw new ValidationException(combinedValidationFailures);

            return new VacancySearchParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SubCategoryCodes = combinedSubCategoryCodes
            };
        }
    }
}
