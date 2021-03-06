﻿using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingStandardCodes : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        private static List<TestCaseData> SuccessTestCases => new List<TestCaseData>
        {
            new TestCaseData(ValidStandardCodes)
                .SetName("Then any number is valid"),
            new TestCaseData(" 1 ".Split(',').ToList())
                .SetName("Then outer spaces are allowed"),
        };

        [TestCaseSource(nameof(SuccessTestCases))]
        public void ThenPassValidation(List<string> input)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                StandardLarsCodes = input,
                FrameworkLarsCodes = new List<string>()
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().BeTrue();
        }

        private static List<TestCaseData> FailingTestCases => new List<TestCaseData>
        {
            new TestCaseData("e",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Standard, "e"),
                ErrorCodes.SearchApprenticeships.StandardCode)
                .SetName("And if non-number character Then is invalid"),
            new TestCaseData("1.1",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Standard, "1.1"),
                ErrorCodes.SearchApprenticeships.StandardCode)
                .SetName("And if number has decimal Then is invalid"),
            new TestCaseData("2 0",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Standard, "2 0"),
                ErrorCodes.SearchApprenticeships.StandardCode)
                .SetName("And the value has spaces Then is invalid"),
            new TestCaseData("2",
                ErrorMessages.SearchApprenticeships.GetTrainingCodeNotFoundErrorMessage(TrainingType.Standard,"2"),
                ErrorCodes.SearchApprenticeships.StandardCode)
                .SetName("And if number is not found Then is invalid")
        };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ThenFailValidation(string input, string expectedErrorMessage, string expectedErrorCode)
        {
            var request = new SearchApprenticeshipVacanciesRequest()
            {
                StandardLarsCodes = input.Split(',').ToList()
            };

            var result = Validator.Validate(request);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            result.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
            result.Errors.First().ErrorCode.Should().Be(expectedErrorCode);
        }
    }
}
