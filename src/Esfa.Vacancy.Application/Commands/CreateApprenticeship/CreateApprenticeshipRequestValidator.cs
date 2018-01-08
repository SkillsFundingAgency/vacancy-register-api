﻿using System;
using System.Linq.Expressions;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using FluentValidation;
using static Esfa.Vacancy.Domain.Validation.ErrorCodes.CreateApprenticeship;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipRequestValidator : AbstractValidator<CreateApprenticeshipRequest>
    {
        public CreateApprenticeshipRequestValidator()
        {
            SetValidatorDisallowEmpty(request => request.Title, TitleIsRequired, new TitleValidator());
            SetValidatorDisallowEmpty(request => request.ShortDescription, ShortDescriptionIsRequired, new ShortDescriptionValidator());
            SetValidatorDisallowEmpty(request => request.LongDescription, LongDescriptionIsRequired, new LongDescriptionValidator());
            SetValidatorDisallowEmpty(request => request.ApplicationClosingDate, "7", new ApplicationClosingDateValidator());
        }

        private void SetValidatorDisallowEmpty<TProperty>(Expression<Func<CreateApprenticeshipRequest, TProperty>> selector, string emptyErrorCode, AbstractValidator<TProperty> validatorToAdd)
        {
            //AbstractValidators do not work against null fields so adding the validator as a dependent rule and doing a NotEmpty check first
            RuleFor(selector)
                .NotEmpty()
                .WithErrorCode(emptyErrorCode)
                .DependentRules(rules =>
                {
                    rules.RuleFor(selector).SetValidator(validatorToAdd);
                });
        }
    }
}