﻿using System;

namespace Esfa.Vacancy.Register.Application.Exceptions
{
    public sealed class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base() { }

        public ResourceNotFoundException(string message) : base(message) { }
    }
}
