﻿using AutoMapper;
using Esfa.Vacancy.Register.Api;
using FluentAssertions;
using NUnit.Framework;
using ApiTypes = Esfa.Vacancy.Api.Types;
using ApprenticeshipSummary = Esfa.Vacancy.Domain.Entities.ApprenticeshipSummary;
using GeoPoint = Esfa.Vacancy.Domain.Entities.GeoPoint;


namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Mappings
{
    [TestFixture]
    public class GivenApprenticeshipSummaryMapper
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [TestCase(1, null, ApiTypes.TrainingType.Standard, TestName = "Then load Standard type")]
        [TestCase(null, "10", ApiTypes.TrainingType.Framework, TestName = "Then load Framework type")]
        public void WhenMappingTraingingDetails(int? standardId, string frameworkCode, ApiTypes.TrainingType expectedTrainingType)
        {
            var expectedTrainingCode = standardId.HasValue ? standardId.ToString() : frameworkCode;
            var domainType = new ApprenticeshipSummary()
            {
                FrameworkLarsCode = frameworkCode,
                StandardLarsCode = standardId
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.TrainingType.Should().Be(expectedTrainingType);
            result.TrainingCode.Should().Be(expectedTrainingCode);
        }

        [Test]
        public void WhenMappingGeoCoordinates()
        {
            var domainType = new ApprenticeshipSummary
            {
                Location = new GeoPoint() { Lat = 51.3288148990, Lon = 0.4452948632 }
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.Location.Latitude.Should().Be(51.3288148990m, "Then map latitude to Location");
            result.Location.Longitude.Should().Be(0.4452948632m, "Then map longitude to Location");

        }

        [Test]
        public void WhenMappingShortDescription()
        {
            var description = "desc";
            var domainType = new ApprenticeshipSummary
            {
                Description = description
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.ShortDescription.Should().Be(description, "Then map Description to ShortDescription");
        }

        [Test]
        public void WhenMappingTrainingProviderName()
        {
            var providerName = "desc";
            var domainType = new ApprenticeshipSummary
            {
                ProviderName = providerName
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.TrainingProviderName.Should().Be(providerName, "Then map ProviderName to TrainingProviderName");
        }

        [TestCase("National", true, TestName = "And Location Type is National Then map set IsNationWide to true")]
        [TestCase("NonNational", false, TestName = "And Location Type is NonNational Then map set IsNationWide to false")]
        public void WhenMappingIsNationWide(string value, bool expectedResult)
        {
            var domainType = new ApprenticeshipSummary
            {
                VacancyLocationType = value
            };

            var result = _mapper.Map<ApiTypes.ApprenticeshipSummary>(domainType);

            result.IsNationwide.Should().Be(expectedResult);
        }

    }
}