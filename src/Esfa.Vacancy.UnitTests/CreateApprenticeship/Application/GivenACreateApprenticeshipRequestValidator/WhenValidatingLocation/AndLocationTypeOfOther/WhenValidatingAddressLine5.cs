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
    public class WhenValidatingAddressLine5
    {
        private static List<TestCaseData> TestCases() =>
            new List<TestCaseData>
            {
                new TestCaseData(null, true, null)
                    .SetName("And is null Then is valid"),
                new TestCaseData(string.Empty, true, null)
                    .SetName("And is empty Then is valid"),
                new TestCaseData("   ", true, null)
                    .SetName("And is whitespace Then is valid"),
                new TestCaseData(new string('a', 301), false, "'Address Line5' must be less than 301 characters. You entered 301 characters.")
                    .SetName("And exceeds 300 characters Then is invalid"),
                new TestCaseData("<p>", false, "'Address Line5' can't contain invalid characters")
                    .SetName("And contains illegal chars Then is invalid"),
                new TestCaseData("10 Downing Street", true, null)
                    .SetName("And is in allowed format Then is valid")
            };

        [TestCaseSource(nameof(TestCases))]
        public void ValidateAddressLine5(string addressLine5, bool isValid, string errorMessage)
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation,
                AddressLine5 = addressLine5
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(r => r.AddressLine5, request);
            }
            else
            {
                sut
                    .ShouldHaveValidationErrorFor(r => r.AddressLine5, request)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.AddressLine5)
                    .WithErrorMessage(errorMessage);
            }
        }
    }
}