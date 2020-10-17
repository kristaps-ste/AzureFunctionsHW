using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Exceptions
{
    public  class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException()
            : base("Configuration variables failed  nullcheck")
        {
        }
    }
}
