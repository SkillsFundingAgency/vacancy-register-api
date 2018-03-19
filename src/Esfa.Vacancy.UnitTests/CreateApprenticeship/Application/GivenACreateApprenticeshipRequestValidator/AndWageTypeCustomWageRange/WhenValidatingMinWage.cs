﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.UnitTests.Extensions;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustomWageRange
{
    [TestFixture]
    public class WhenValidatingMinWage : CreateApprenticeshipRequestValidatorBase
    {
        private IFixture _fixture;
        private CreateApprenticeshipRequestValidator _validator;
        private Mock<IMinimumWageSelector> _mockSelector;
        private Mock<IHourlyWageCalculator> _mockCalculator;
        private decimal _expectedAllowedMinimumWage;
        private decimal _expectedAttemptedMinimumWage;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedAllowedMinimumWage = _fixture.Create<decimal>();
            _expectedAttemptedMinimumWage = _fixture.Create<decimal>();

            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_expectedAllowedMinimumWage);

            _mockCalculator = _fixture.Freeze<Mock<IHourlyWageCalculator>>();
            _mockCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Returns(_expectedAttemptedMinimumWage);

            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();
        }

        [Test]
        public async Task AndNoValueThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.Count
                .Should().Be(1);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Min Wage' must not be empty.");

        }

        [Test]
        public async Task AndHasValueThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = 99.99m
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndValueNotMonetaryThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = 99.99999m
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Min Wage' must be a monetary value.");
        }

        [Test]
        public async Task AndExpectedStartDateIsMinValue_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = DateTime.MinValue,
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndWageUnitIsNotApplicable_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = WageUnit.NotApplicable,
                ExpectedStartDate = _fixture.Create<DateTime>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndHoursPerWeekIsLessThanZero_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.Create<DateTime>(),
                HoursPerWeek = -9
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task ThenUsesSelectorToGetAllowedMinimumWage()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = expectedStartDate,
                MinWage = _fixture.Create<decimal>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            await _validator.ValidateAsync(context);

            _mockSelector.Verify(selector => selector.SelectHourlyRateAsync(expectedStartDate));
        }

        [Test]
        public async Task ThenUsesCalculatorToDetermineAttemptedMinimumWage()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.Create<DateTime>(),
                MinWage = _fixture.Create<decimal>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            await _validator.ValidateAsync(context);

            _mockCalculator.Verify(calculator => calculator.Calculate(request.MinWage.Value, request.WageUnit, (decimal)request.HoursPerWeek));
        }

        [Test]
        public void AndSelectorThrowsInfrastructureException_ThenLetsExceptionBubble()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.Create<DateTime>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .Throws<InfrastructureException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<InfrastructureException>();
        }

        [Test]
        public void AndSelectorThrowsWageRangeNotFoundException_ThenLetsExceptionBubble()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.Create<DateTime>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .Throws<WageRangeNotFoundException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<WageRangeNotFoundException>();
        }

        [Test]
        public async Task AndCalculatorThrowsException_ThenReturnsValidationResult()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.Create<DateTime>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _mockCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Throws<ArgumentOutOfRangeException>();

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(WageUnit.Weekly, 3.5m, 125.99m, false).SetName("And attempted weekly is less than allowed Then is invalid"),
            new TestCaseData(WageUnit.Weekly, 3.5m, 126.00m, true).SetName("And attempted weekly is same as allowed Then is valid"),
            new TestCaseData(WageUnit.Weekly, 3.5m, 126.01m, true).SetName("And attempted weekly is greater than allowed Then is valid"),
            new TestCaseData(WageUnit.Monthly, 3.5m, 545.99m, false).SetName("And attempted monthly is less than allowed Then is invalid"),
            new TestCaseData(WageUnit.Monthly, 3.5m, 546.00m, true).SetName("And attempted monthly is same as allowed Then is valid"),
            new TestCaseData(WageUnit.Monthly, 3.5m, 546.01m, true).SetName("And attempted monthly is greater than allowed Then is valid"),
            new TestCaseData(WageUnit.Annually, 3.5m, 6551.99m, false).SetName("And attempted annually is less than allowed Then is invalid"),
            new TestCaseData(WageUnit.Annually, 3.5m, 6552.00m, true).SetName("And attempted annually is same as allowed Then is valid"),
            new TestCaseData(WageUnit.Annually, 3.5m, 6552.01m, true).SetName("And attempted annually is greater than allowed Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCheckingAllowedVersusAttemtpedMinWage(WageUnit wageUnit, decimal allowedMinimumHourlyWage, decimal attemptedMinWage, bool expectedIsValid)
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = wageUnit,
                HoursPerWeek = 36,
                MinWage = attemptedMinWage,
                ExpectedStartDate = _fixture.Create<DateTime>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(allowedMinimumHourlyWage);

            _fixture.Inject<IHourlyWageCalculator>(new HourlyWageCalculator());
            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(expectedIsValid);
            if (!result.IsValid)
            {
                result.Errors.First().ErrorCode
                    .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
                result.Errors.First().ErrorMessage
                    .Should().Be(ErrorMessages.CreateApprenticeship.MinWageIsBelowApprenticeMinimumWage);
            }
        }
    }
}