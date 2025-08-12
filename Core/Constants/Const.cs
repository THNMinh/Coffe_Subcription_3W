using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class Consts
    {
        public const int JWT_EXPIRED_TIME = 60; //minutes
        public const int RESET_PASSWORD_TOKEN_EXPIRED_TIME = 1; //hours
        public const int PASSWORD_MIN_LENGTH = 6;
        public const int RANDOM_PASSWORD_LENGTH = 20;
        public const string COMPANY_NAME = "CoffeeSubscription";
    }
}
