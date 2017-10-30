﻿using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    public class WhenMappingNationwideOnly
    {
        [Test]
        public void AndIsTrue_ThenMapsToNationwide()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = true });

            result.LocationType.Should().Be("Nationwide");
        }

        [Test]
        public void AndIsFalse_ThenMapsToNull()
        {
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest
                { NationwideOnly = false });

            result.LocationType.Should().BeNull();
        }
    }
}