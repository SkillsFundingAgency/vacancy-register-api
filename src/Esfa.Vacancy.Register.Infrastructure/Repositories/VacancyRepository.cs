﻿using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using DomainEntities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly IProvideSettings _provideSettings;

        public VacancyRepository(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public async Task<DomainEntities.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingConstants.AvmsPlusDatabaseConnectionStringKey);

            DomainEntities.Vacancy vacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<DomainEntities.Vacancy, DomainEntities.Address, DomainEntities.Vacancy>(
                        VacancyDetailsQuery,
                        param: new { ReferenceNumber = referenceNumber },
                        map: (v, a) => { v.EmployerAddress = a; return v; },
                        splitOn: "AddressLine1");

                vacancy = results.FirstOrDefault();
            }

            return vacancy;
        }

        const string VacancyDetailsQuery = @"
SELECT  V.VacancyReferenceNumber AS Reference
,       V.Title
,       V.ShortDescription
,       V.[Description]
,       V.VacancyTypeId
,       V.WageUnitId
,       V.WeeklyWage
,       V.WorkingWeek
,       V.WageText
,       V.HoursPerWeek
,       V.ExpectedDuration 
,       V.ExpectedStartDate
,		VH.HistoryDate AS DatePosted
,       V.ApplicationClosingDate
,       V.NumberofPositions 
,       V.TrainingTypeId
,       RS.LarsCode AS StandardCode
,       AF.ShortName AS FrameworkCode
,       E.FullName AS EmployerName
,       V.EmployerDescription
,       V.EmployersWebsite AS EmployerWebsite
,       TextFields.[TrainingToBeProvided]
,       TextFields.[QulificationsRequired]
,       TextFields.[SkillsRequired]
,       TextFields.[PersonalQualities]
,       TextFields.[ImportantInformation]
,       TextFields.[FutureProspects]
,       TextFields.[RealityCheck]
,       E.AddressLine1
,       E.AddressLine2
,       E.AddressLine3
,       E.AddressLine4
,       E.AddressLine5
,       E.Latitude
,       E.Longitude
,       E.PostCode
,       E.Town
FROM[dbo].[Vacancy]        V
INNER JOIN (SELECT VacancyId, Min(HistoryDate) HistoryDate
            FROM [dbo].[VacancyHistory]
            WHERE VacancyHistoryEventTypeId = 1
            AND VacancyHistoryEventSubTypeId = 2
            GROUP BY VacancyId
           ) VH
	ON V.VacancyId = VH.VacancyId 
LEFT JOIN   [Reference].[Standard] AS RS 
    ON V.StandardId = RS.StandardId
LEFT JOIN   [dbo].[ApprenticeshipFramework] AS AF
    ON      V.ApprenticeshipFrameworkId = AF.ApprenticeshipFrameworkId
INNER JOIN VacancyOwnerRelationship AS R 
    ON      V.VacancyOwnerRelationshipId = R.VacancyOwnerRelationshipId
INNER JOIN Employer AS E 
    ON      R.EmployerId = E.EmployerId
LEFT JOIN (
            SELECT 
                 VacancyId
            ,    MAX(CASE WHEN Field = 1 THEN [Value] END ) AS [TrainingToBeProvided]
            ,    MAX(CASE WHEN Field = 2 THEN [Value] END ) AS [QulificationsRequired]
            ,    MAX(CASE WHEN Field = 3 THEN [Value] END ) AS [SkillsRequired]
            ,    MAX(CASE WHEN Field = 4 THEN [Value] END ) AS [PersonalQualities]
            ,    MAX(CASE WHEN Field = 5 THEN [Value] END ) AS [ImportantInformation]
            ,    MAX(CASE WHEN Field = 6 THEN [Value] END ) AS [FutureProspects]
            ,    MAX(CASE WHEN Field = 7 THEN [Value] END ) AS [RealityCheck]
            FROM VacancyTextField AS T
            GROUP BY VacancyId
          ) AS TextFields
    ON      TextFields.VacancyId = V.VacancyId
WHERE V.VacancyStatusId = 2
AND V.VacancyReferenceNumber = @ReferenceNumber
";
    }
}