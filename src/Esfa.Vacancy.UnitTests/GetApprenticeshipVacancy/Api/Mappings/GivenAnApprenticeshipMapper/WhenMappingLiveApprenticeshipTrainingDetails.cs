﻿using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Settings;
using Esfa.Vacancy.Register.Api.Mappings;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using ApprenticeshipVacancy = Esfa.Vacancy.Domain.Entities.ApprenticeshipVacancy;
using TrainingType = Esfa.Vacancy.Api.Types.TrainingType;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingLiveApprenticeshipTrainingDetails
    {
        [Test]
        public void WithFrameworkThenLoadFrameworkDetails()
        {
            var vacancy = new Fixture().Build<ApprenticeshipVacancy>()
                                        .With(v => v.WageUnitId, null)
                                        .Without(v => v.Standard)
                                        .With(v => v.Framework, new Framework
                                        {
                                            Title = "Title",
                                            Code = 13,
                                            Uri = "sdfe"
                                        })
                                        .Create();

            var sut = new ApprenticeshipMapper(Mock.Of<IProvideSettings>());

            var result = sut.MapToApprenticeshipVacancy(vacancy);

            Assert.AreEqual(TrainingType.Framework, result.TrainingType);
        }

        [Test]
        public void WithStandardThenLoadStandardDetails()
        {
            var vacancy = new Fixture().Build<ApprenticeshipVacancy>()
                                        .With(v => v.WageUnitId, null)
                                        .Without(v => v.Framework)
                                        .With(v => v.Standard, new Standard
                                        {
                                            Title = "Title",
                                            Code = 13,
                                            Uri = "sdfe"

                                        })
                                        .Create();

            var sut = new ApprenticeshipMapper(Mock.Of<IProvideSettings>());

            var result = sut.MapToApprenticeshipVacancy(vacancy);

            Assert.AreEqual(TrainingType.Standard, result.TrainingType);
        }

        [Test]
        public void WithFrameworkOrStandardIsMissingThenReturnUnavailable()
        {
            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                        .With(v => v.WageUnitId, null)
                                        .Without(v => v.Framework)
                                        .Without(v => v.Standard)
                                        .Create();

            var sut = new ApprenticeshipMapper(Mock.Of<IProvideSettings>());

            var result = sut.MapToApprenticeshipVacancy(vacancy);

            Assert.AreEqual(TrainingType.Unavailable, result.TrainingType);
        }
    }
}