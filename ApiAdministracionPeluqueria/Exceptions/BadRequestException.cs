﻿namespace ApiAdministracionPeluqueria.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string? message = null) : base(message)
        {
        }
    }
}
