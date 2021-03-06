﻿using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.WhenValidatingLocation.AndLocationTypeOfOther
{
    [TestFixture]
    public class WhenValidatingTown
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(false, null, "'Town' should not be empty.")
                    .SetName("And is null Then is invalid"),
                new TestCaseData(false, new string('a', 101), "'Town' must be less than 101 characters. You entered 101 characters.")
                    .SetName("And exceeds 100 characters Then is invalid"),
                new TestCaseData(false, "<p>", "'Town' can't contain invalid characters")
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData(true, "Coventry", null)
                    .SetName("And is in allowed format Then is valid"),
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateTown(bool isValid, string town, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation,
                Town = town
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(r => r.Town, request);
            }
            else
            {
                sut
                    .ShouldHaveValidationErrorFor(r => r.Town, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.Town)
                    .WithErrorMessage(errorMessage);
            }
        }
    }
}