using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Extensions
{
    public static class UserExtentions
    {
        public static void Map(this User dbUser, User user)
        {
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Age = user.Age;
        }

    }
}