﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators
{
    public partial class CreateApprenticeshipRequestValidator
    {
        private void ConfigureWageTypeValidator()
        {
            const int wageTypeReasonMaxLength = 240;

            RuleFor(request => request.WageType)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageType)
                .DependentRules(rules => rules.RuleFor(request => request.WageType)
                    .IsInEnum()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageType));

            RuleFor(request => request.WageUnit)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit)
                .DependentRules(rules => rules.RuleFor(request => request.WageUnit)
                    .IsInEnum()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit));

            When(request => request.WageType == WageType.CustomWageFixed, () =>
            {
                RuleFor(request => request.FixedWage)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage)
                    .MustBeAMonetaryValue()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .NotEqual(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);

                When(request => request.WageUnit != WageUnit.NotApplicable
                                && request.ExpectedStartDate > DateTime.Today
                                && request.FixedWage.HasValue
                                && request.HoursPerWeek > 0,
                () =>
                {
                    RuleFor(request => request.FixedWage)
                        .Must(BeGreaterThanOrEqualToApprenticeshipMinimumWage)
                        .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage)
                        .WithMessage(ErrorMessages.CreateApprenticeship.FixedWageIsBelowApprenticeMinimumWage);
                });
            });

            When(request => request.WageType == WageType.CustomWageRange, () =>
            {
                RuleFor(request => request.MinWage)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage)
                    .MustBeAMonetaryValue()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotNull()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage)
                    .MustBeAMonetaryValue()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage)
                    .GreaterThan(request => request.MinWage)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage)
                    .WithMessage(ErrorMessages.CreateApprenticeship.MaxWageCantBeLessThanMinWage);

                RuleFor(request => request.FixedWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .NotEqual(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);

                When(request => request.WageUnit != WageUnit.NotApplicable
                                && request.ExpectedStartDate > DateTime.Today
                                && request.MinWage.HasValue
                                && request.HoursPerWeek > 0,
                    () =>
                    {
                        RuleFor(request => request.MinWage)
                            .Must(BeGreaterThanOrEqualToApprenticeshipMinimumWage)
                            .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage)
                            .WithMessage(ErrorMessages.CreateApprenticeship.MinWageIsBelowApprenticeMinimumWage);
                    });
            });

            When(request => request.WageType == WageType.NationalMinimumWage, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.FixedWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            When(request => request.WageType == WageType.ApprenticeshipMinimumWage, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.FixedWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.WageTypeReason)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            When(request => request.WageType == WageType.Unwaged, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.FixedWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.WageTypeReason)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            When(request => request.WageType == WageType.CompetitiveSalary, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.FixedWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.WageTypeReason)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            When(request => request.WageType == WageType.ToBeSpecified, () =>
            {
                RuleFor(request => request.MinWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MinWage);

                RuleFor(request => request.MaxWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.MaxWage);

                RuleFor(request => request.FixedWage)
                    .Null()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.FixedWage);

                RuleFor(request => request.WageTypeReason)
                    .NotEmpty()
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);

                RuleFor(request => request.WageUnit)
                    .Equal(WageUnit.NotApplicable)
                    .WithErrorCode(ErrorCodes.CreateApprenticeship.WageUnit);
            });

            RuleFor(request => request.WageTypeReason)
                .MaximumLength(wageTypeReasonMaxLength)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason)
                .MatchesAllowedFreeTextCharacters()
                .WithErrorCode(ErrorCodes.CreateApprenticeship.WageTypeReason);
        }

        private bool BeGreaterThanOrEqualToApprenticeshipMinimumWage(CreateApprenticeshipRequest request, decimal? wage)
        {
            try
            {
                var allowedMinimumWage = _getMinimumWagesService
                    .GetApprenticeMinimumWageRate(request.ExpectedStartDate);

                var attemptedMinimumWage = _hourlyWageCalculator.Calculate(
                    wage.GetValueOrDefault(),
                    request.WageUnit,
                    (decimal)request.HoursPerWeek);

                return attemptedMinimumWage >= allowedMinimumWage;
            }
            catch (ArgumentOutOfRangeException outOfRangeException)
            {
                _logger.Debug(outOfRangeException.Message);
                return false;
            }
        }
    }
}