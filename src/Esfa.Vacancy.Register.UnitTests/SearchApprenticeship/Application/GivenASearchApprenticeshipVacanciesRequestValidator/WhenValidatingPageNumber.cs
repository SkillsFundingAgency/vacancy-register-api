﻿using System;
using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    public class WhenValidatingPageNumber
    {
        private SearchApprenticeshipVacanciesRequestValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SearchApprenticeshipVacanciesRequestValidator();
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingIsValid(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = _validator.Validate(searchRequest);

            actualResult.IsValid.Should().Be(expectedResult.IsValid);
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorMessages(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = _validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorMessage));
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorCodes(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = _validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorCode));
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new List<string> {"2345"}
                }, new ValidationResult())
                .SetName("Then default is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new List<string> {"2345"},
                    PageNumber = 0
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("PageNumber", "'Page Number' must be greater than or equal to '1'.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.PageNumberLessThan1
                    }}
                })
                .SetName("Then less than 1 is invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new List<string> {"2345"},
                    PageNumber = new Random().Next()
                }, new ValidationResult())
                .SetName("Then greater than 1 is valid")
        };
    }
}
