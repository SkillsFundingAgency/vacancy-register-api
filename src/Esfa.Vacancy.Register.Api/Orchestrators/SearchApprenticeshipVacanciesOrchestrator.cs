﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class SearchApprenticeshipVacanciesOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SearchApprenticeshipVacanciesOrchestrator(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<SearchResponse<ApprenticeshipSummary>> SearchApprenticeship(
            SearchApprenticeshipParameters apprenticeSearchParameters)
        {
            if (apprenticeSearchParameters == null) throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure("apprenticeSearchParameters", "At least one search parameter is required.")
                { ErrorCode = ErrorCodes.SearchApprenticeships.SearchApprenticeshipParametersIsNull}
            });

            var request = _mapper.Map<SearchApprenticeshipVacanciesRequest>(apprenticeSearchParameters);
            var response = await _mediator.Send(request);
            var results = _mapper.Map<SearchResponse<ApprenticeshipSummary>>(response);
            return results;
        }
    }
}
