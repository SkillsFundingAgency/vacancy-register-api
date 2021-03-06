﻿using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApiTypes = Esfa.Vacancy.Api.Types;
using ApprenticeshipVacancy = Esfa.Vacancy.Domain.Entities.ApprenticeshipVacancy;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRaaApprenticeshipMapper
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenMappingLiveApprenticeshipWithNationalMinimumWageType
    {
        private const int VacancyReference = 1234;
        private const int LiveVacancyStatusId = 2;
        private const string UnknownwWageText = "Unknown";
        private Register.Api.Mappings.ApprenticeshipMapper _sut;

        [SetUp]
        public void SetUp()
        {
            var provideSettings = new Mock<IProvideSettings>();
            _sut = new Register.Api.Mappings.ApprenticeshipMapper(provideSettings.Object);
        }

        [TestCase(null, 7.05, "Unknown - £211.50")]
        [TestCase(4.05, null, "£121.50 - Unknown")]
        [TestCase(4.05, 7.05, "£121.50 - £211.50")]
        [TestCase(null, null, "Unknown")]
        public void ShouldHaveWageSetForVacancy(decimal? lowerBound, decimal? upperBound, string expectedWageText)
        {
            var minNationalWageLowerBound = lowerBound;
            var minNationalWageUpperBound = upperBound;
            const decimal hoursPerWeek = 30;

            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, VacancyReference)
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                .With(v => v.WageType, (int)LegacyWageType.NationalMinimum)
                .With(v => v.MinimumWageLowerBound, minNationalWageLowerBound)
                .With(v => v.MinimumWageUpperBound, minNationalWageUpperBound)
                .With(v => v.HoursPerWeek, hoursPerWeek)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(VacancyReference);
            vacancy.WageUnit.Should().Be(ApiTypes.WageUnit.Unspecified);
            vacancy.WageText.Should().Be(expectedWageText);
        }

        [TestCase(null, 7.05)]
        [TestCase(4.05, null)]
        [TestCase(4.05, 7.05)]
        [TestCase(null, null)]
        public void ShouldHaveUnknownWageForVacancyWithUndefinedHoursPerWeek(decimal? lowerBound, decimal? upperBound)
        {
            var minNationalWageLowerBound = lowerBound;
            var minNationalWageUpperBound = upperBound;

            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, VacancyReference)
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                .With(v => v.WageType, (int)LegacyWageType.NationalMinimum)
                .With(v => v.MinimumWageLowerBound, minNationalWageLowerBound)
                .With(v => v.MinimumWageUpperBound, minNationalWageUpperBound)
                .Without(v => v.HoursPerWeek)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(VacancyReference);
            vacancy.WageUnit.Should().Be(ApiTypes.WageUnit.Unspecified);
            vacancy.WageText.Should().Be(UnknownwWageText);
        }

        [TestCase(null, 7.05)]
        [TestCase(4.05, null)]
        [TestCase(4.05, 7.05)]
        [TestCase(null, null)]
        public void ShouldHaveUnknownWageForVacancyWithZeroHoursPerWeek(decimal? lowerBound, decimal? upperBound)
        {
            var minNationalWageLowerBound = lowerBound;
            var minNationalWageUpperBound = upperBound;

            var apprenticeshipVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(v => v.VacancyReferenceNumber, VacancyReference)
                .With(v => v.ApprenticeshipTypeId, 1)
                .With(v => v.VacancyStatusId, LiveVacancyStatusId)
                .With(v => v.WageType, (int)LegacyWageType.NationalMinimum)
                .With(v => v.MinimumWageLowerBound, minNationalWageLowerBound)
                .With(v => v.MinimumWageUpperBound, minNationalWageUpperBound)
                .With(v => v.HoursPerWeek, 0)
                .Without(v => v.WageUnitId)
                .Create();

            var vacancy = _sut.MapToApprenticeshipVacancy(apprenticeshipVacancy);

            vacancy.VacancyReference.Should().Be(VacancyReference);
            vacancy.WageUnit.Should().Be(ApiTypes.WageUnit.Unspecified);
            vacancy.WageText.Should().Be(UnknownwWageText);
        }
    }
}
