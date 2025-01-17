﻿using System;

namespace Core.Common.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message)
            : base(message)
        {

        }

        public NotFoundException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
