﻿using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingStandardCodes
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
                    StandardCodes = new [] {"e"}
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("StandardCodes[0]", "e is invalid, expected a number.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotInt32
                    } }
                })
                .SetName("Then chars are not allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new [] {"1.1"}
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("StandardCodes[0]", "1.1 is invalid, expected a number.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotInt32
                    } }
                })
                .SetName("Then floats are not allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new [] {"6 2"}
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("StandardCodes[0]", "6 2 is invalid, expected a number.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.StandardCodeNotInt32
                    } }
                })
                .SetName("Then inner spaces are not allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new [] {"  5345   "}
                }, new ValidationResult())
                .SetName("Then outer spaces are allowed"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    StandardCodes = new List<string> {"123424"}
                }, new ValidationResult())
                .SetName("Then ints are allowed")
        };
    }
}