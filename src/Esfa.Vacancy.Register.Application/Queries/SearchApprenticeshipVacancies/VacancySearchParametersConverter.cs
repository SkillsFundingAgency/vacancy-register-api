﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class VacancySearchParametersConverter
    {
        private readonly IStandardRepository _standardRepository;

        public VacancySearchParametersConverter(IStandardRepository standardRepository)
        {
            _standardRepository = standardRepository;
        }

        public async Task<VacancySearchParameters> ConvertFrom(SearchApprenticeshipVacanciesRequest request)
        {
            var sectorCodes = await ConvertStandardCodesToSearchableSectorCodes(request.StandardCodes.Select(int.Parse));

            return new VacancySearchParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                StandardSectorCodes = sectorCodes
            };
        }

        private async Task<List<string>> ConvertStandardCodesToSearchableSectorCodes(IEnumerable<int> standardCodes)
        {
            var standardSectorIds = await _standardRepository.GetStandardsAndRespectiveSectorIdsAsync();

            var errors = new List<ValidationFailure>();
            var sectorCodes = new HashSet<string>();

            standardCodes.ToList().ForEach(standardCode =>
            {
                var standardSector = standardSectorIds.FirstOrDefault(ss => ss.LarsCode == standardCode);
                if (standardSector == null)
                {
                    errors.Add(new ValidationFailure("StandardCode", $"StandardCode {standardCode} is invalid"));
                }
                else
                {
                    sectorCodes.Add($"{StandardSector.StandardSectorPrefix}.{standardSector.StandardSectorId}");
                }
            });

            if (errors.Any())
                throw new ValidationException(errors);

            return sectorCodes.ToList();
        }
    }
}
