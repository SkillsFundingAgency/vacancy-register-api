﻿using Esfa.Vacancy.Api.Types.Request;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Manage.Api.Mappings;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApiTypes = Esfa.Vacancy.Api.Types;
using ApplicationTypes = Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Api.Mappings.GivenACreateApprenticeshipRequestMapper
{
    [TestFixture]
    public class WhenMappingFromApiParameters
    {
        private CreateApprenticeshipParameters _apiParameters;
        private CreateApprenticeshipRequest _mappedRequest;
        private int _ukprn = 12345678;
        private string _userEmail = "test@test.com";

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();

            var randomWageType = fixture.Create<int>();
            var randomWageUnit = fixture.Create<int>();
            var randomLocationType = fixture.Create<int>();
            var randomApplicationMethod = fixture.Create<int>();
            _apiParameters = fixture.Build<CreateApprenticeshipParameters>()
                .With(parameters => parameters.LocationType, (ApiTypes.LocationType)randomLocationType)
                .With(parameters => parameters.WageType, (ApiTypes.WageType)randomWageType)
                .With(parameters => parameters.WageUnit, (Vacancy.Api.Types.Request.WageUnit)randomWageUnit)
                .With(parameters => parameters.ApplicationMethod, (ApiTypes.ApplicationMethod)randomApplicationMethod)
                .Create();

            var mapper = new CreateApprenticeshipRequestMapper();

            _mappedRequest = mapper.MapFromApiParameters(_apiParameters, _ukprn, _userEmail);
        }

        [Test]
        public void ThenMapsTitle()
        {
            _mappedRequest.Title.Should().Be(_apiParameters.Title);
        }

        [Test]
        public void ThenMapsUserEmail()
        {
            _mappedRequest.UserEmail.Should().Be(_userEmail);
        }

        [Test]
        public void ThenMapsShortDescription()
        {
            _mappedRequest.ShortDescription.Should().Be(_apiParameters.ShortDescription);
        }

        [Test]
        public void ThenMapsLongDescription()
        {
            _mappedRequest.LongDescription.Should().Be(_apiParameters.LongDescription);
        }

        [Test]
        public void ThenMapsDesiredSkills()
        {
            _mappedRequest.DesiredSkills.Should().Be(_apiParameters.DesiredSkills);
        }

        [Test]
        public void ThenMapsDesiredPersonalQualities()
        {
            _mappedRequest.DesiredPersonalQualities.Should().Be(_apiParameters.DesiredPersonalQualities);
        }

        [Test]
        public void ThenMapsDesiredQualifications()
        {
            _mappedRequest.DesiredQualifications.Should().Be(_apiParameters.DesiredQualifications);
        }

        [Test]
        public void ThenMapsFutureProspects()
        {
            _mappedRequest.FutureProspects.Should().Be(_apiParameters.FutureProspects);
        }

        [Test]
        public void ThenMapsThingsToConsider()
        {
            _mappedRequest.ThingsToConsider.Should().Be(_apiParameters.ThingsToConsider);
        }

        [Test]
        public void ThenMapsTrainingToBeProvider()
        {
            _mappedRequest.TrainingToBeProvided.Should().Be(_apiParameters.TrainingToBeProvided);
        }

        [Test]
        public void ThenMapsExpectedDuration()
        {
            _mappedRequest.ExpectedDuration.Should().Be(_apiParameters.ExpectedDuration);
        }

        [Test]
        public void ThenMapsDurationType()
        {
            _mappedRequest.DurationType.Should().Be((DurationType)_apiParameters.DurationType);
        }

        [Test]
        public void ThenMapsApplicationMethod()
        {
            _mappedRequest.ApplicationMethod.Should().Be((ApplicationMethod)_apiParameters.ApplicationMethod);
        }

        [Test]
        public void ThenMapsSupplementaryQuestion1()
        {
            _mappedRequest.SupplementaryQuestion1.Should().Be(_apiParameters.SupplementaryQuestion1);
        }

        [Test]
        public void ThenMapsSupplementaryQuestion2()
        {
            _mappedRequest.SupplementaryQuestion2.Should().Be(_apiParameters.SupplementaryQuestion2);
        }

        [Test]
        public void ThenMapsExternalApplicationUrl()
        {
            _mappedRequest.ExternalApplicationUrl.Should().Be(_apiParameters.ExternalApplicationUrl);
        }

        [Test]
        public void ThenMapsExternalApplicationInstructions()
        {
            _mappedRequest.ExternalApplicationInstructions.Should().Be(_apiParameters.ExternalApplicationInstructions);
        }

        [Test]
        public void ThenMapsApplicationClosingDate()
        {
            _mappedRequest.ApplicationClosingDate.Should().Be(_apiParameters.ApplicationClosingDate);
        }

        [Test]
        public void ThenMapsExpectedStartDate()
        {
            _mappedRequest.ExpectedStartDate.Should().Be(_apiParameters.ExpectedStartDate);
        }

        [Test]
        public void ThenMapsWorkingWeek()
        {
            _mappedRequest.WorkingWeek.Should().Be(_apiParameters.WorkingWeek);
        }

        [Test]
        public void ThenMapsHoursPerWeek()
        {
            _mappedRequest.HoursPerWeek.Should().Be(_apiParameters.HoursPerWeek);
        }

        [Test]
        public void ThenMapsWageType()
        {
            _mappedRequest.WageType.Should().Be((WageType)_apiParameters.WageType);
        }

        [Test]
        public void ThenMapsWageTypeReason()
        {
            _mappedRequest.WageTypeReason.Should().Be(_apiParameters.WageTypeReason);
        }

        [Test]
        public void ThenMapsWageUnit()
        {
            _mappedRequest.WageUnit.Should().Be((ApplicationTypes.WageUnit)_apiParameters.WageUnit);
        }

        [Test]
        public void ThenMapsMinWage()
        {
            _mappedRequest.MinWage.Should().Be(_apiParameters.MinWage);
        }

        [Test]
        public void ThenMapsMaxWage()
        {
            _mappedRequest.MaxWage.Should().Be(_apiParameters.MaxWage);
        }

        [Test]
        public void ThenMapsWeeklyWage()
        {
            _mappedRequest.FixedWage.Should().Be(_apiParameters.FixedWage);
        }

        [Test]
        public void ThenMapsLocationType()
        {
            _mappedRequest.LocationType.Should().Be((ApplicationTypes.LocationType)_apiParameters.LocationType);
        }

        [Test]
        public void ThenMapsAddressLine1()
        {
            _mappedRequest.AddressLine1.Should().Be(_apiParameters.Location.AddressLine1);
        }

        [Test]
        public void ThenMapsAddressLine2()
        {
            _mappedRequest.AddressLine2.Should().Be(_apiParameters.Location.AddressLine2);
        }

        [Test]
        public void ThenMapsAddressLine3()
        {
            _mappedRequest.AddressLine3.Should().Be(_apiParameters.Location.AddressLine3);
        }

        [Test]
        public void ThenMapsAddressLine4()
        {
            _mappedRequest.AddressLine4.Should().Be(_apiParameters.Location.AddressLine4);
        }

        [Test]
        public void ThenMapsAddressLine5()
        {
            _mappedRequest.AddressLine5.Should().Be(_apiParameters.Location.AddressLine5);
        }

        [Test]
        public void ThenMapsTown()
        {
            _mappedRequest.Town.Should().Be(_apiParameters.Location.Town);
        }

        [Test]
        public void ThenMapsPostcode()
        {
            _mappedRequest.Postcode.Should().Be(_apiParameters.Location.Postcode);
        }

        [Test]
        public void ThenMapsNumberOfPositions()
        {
            _mappedRequest.NumberOfPositions.Should().Be(_apiParameters.NumberOfPositions);
        }

        [Test]
        public void ThenMapsProviderUkprn()
        {
            _mappedRequest.ProviderUkprn.Should().Be(_ukprn);
        }

        [Test]
        public void ThenMapsEmployersEdsUrn()
        {
            _mappedRequest.EmployerEdsUrn.Should().Be(_apiParameters.EmployerEdsUrn);
        }

        [Test]
        public void ThenMapsProviderSiteEdsUrn()
        {
            _mappedRequest.ProviderSiteEdsUrn.Should().Be(_apiParameters.ProviderSiteEdsUrn);
        }

        [Test]
        public void ThenMapsContactName()
        {
            _mappedRequest.ContactName.Should().Be(_apiParameters.ContactName);
        }

        [Test]
        public void ThenMapsContactEmail()
        {
            _mappedRequest.ContactEmail.Should().Be(_apiParameters.ContactEmail);
        }

        [Test]
        public void ThenMapsContactNumber()
        {
            _mappedRequest.ContactNumber.Should().Be(_apiParameters.ContactNumber);
        }

        [Test]
        public void ThenMapsTrainingType()
        {
            _mappedRequest.TrainingType.Should().Be((Domain.Entities.TrainingType)_apiParameters.TrainingType);
        }

        [Test]
        public void ThenMapsTrainingCode()
        {
            _mappedRequest.TrainingCode.Should().Be(_apiParameters.TrainingCode);
        }

        [Test]
        public void ThenMapsIsEmployerDisabilityConfident()
        {
            _mappedRequest.IsEmployerDisabilityConfident.Should().Be(_apiParameters.IsEmployerDisabilityConfident);
        }

        [Test]
        public void ThenMapsAdditionalLocationInformation()
        {
            _mappedRequest.AdditionalLocationInformation.Should().Be(_apiParameters.Location.AdditionalInformation);
        }

        [Test]
        public void ThenMapsEmployerDescription()
        {
            _mappedRequest.EmployerDescription.Should().Be(_apiParameters.EmployerDescription);
        }

        [Test]
        public void ThenMapsEmployerWebsite()
        {
            _mappedRequest.EmployerWebsiteUrl.Should().Be(_apiParameters.EmployerWebsiteUrl);
        }
    }
}
