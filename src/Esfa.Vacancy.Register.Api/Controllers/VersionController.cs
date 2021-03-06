﻿using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using Esfa.Vacancy.Register.Api.Models;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    /// <summary>
    /// Operational information about the service
    /// </summary>
    [RoutePrefix("api")]
    public class VersionController : ApiController
    {
        /// <summary>
        /// The version of the service
        /// </summary>
        /// <returns>Some details about the versions</returns>
        [Route("version")]
        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public VersionInformation Index()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version.ToString();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var assemblyInformationalVersion = fileVersionInfo.ProductVersion;

            return new VersionInformation
            {
                SemanticVersion = assemblyInformationalVersion,
                AssemblyVersion = version,
                PackageVersion = assemblyInformationalVersion?.Split('-').FirstOrDefault()
            };
        }
    }
}
