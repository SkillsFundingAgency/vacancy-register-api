﻿namespace Esfa.Vacancy.Register.Domain.Validation
{
    public static class ErrorMessages
    {
        public static class SearchApprenticeships
        {
            public const string SearchApprenticeshipParametersIsNull  = "At least one search parameter is required.";

            public const string StandardAndFrameworkCodeNotProvided   = "At least one of StandardCodes or FrameworkCodes is required.";
            public const string StandardCodeNotInt32                  = "{0} is invalid, expected a number.";
            public const string FrameworkCodeNotInt32                 = "{0} is invalid, expected a number.";

            public const string FrameworkCodeNotFound                 = "FrameworkCode {0} is invalid.";
            public const string StandardCodeNotFound                  = "StandardCode {0} is invalid.";
        }
    }
}