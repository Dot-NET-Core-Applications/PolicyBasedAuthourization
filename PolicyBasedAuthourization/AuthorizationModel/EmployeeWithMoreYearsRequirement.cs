using System;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthourization.AuthorizationModel
{
    public class EmployeeWithMoreYearsRequirement : IAuthorizationRequirement
    {
        public EmployeeWithMoreYearsRequirement(int years)
        {
            Years = years;
        }

        public int Years { get; }
    }
}
