using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PolicyBasedAuthourization.AuthorizationModel
{
    public class EmployeeWithMoreYearsHandler : AuthorizationHandler<EmployeeWithMoreYearsRequirement>
    {
        /// <summary>
        /// No. of years of service of an Employee.
        /// </summary>
        private readonly IEmployeeNumberOfYears employeeNumberOfYears;

        /// <summary>
        /// Instance of Authorization handler.
        /// </summary>
        /// <param name="employeeNumberOfYears"><see cref="EmployeeNumberOfYears"/></param>
        public EmployeeWithMoreYearsHandler(IEmployeeNumberOfYears employeeNumberOfYears)
        {
            this.employeeNumberOfYears = employeeNumberOfYears;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeeWithMoreYearsRequirement requirement)
        {
            if (context.User.HasClaim(claim => claim.Type == ClaimTypes.Name))
            {
                Claim name = context.User.FindFirst(claim => claim.Type == ClaimTypes.Name);
                int yearsOfExperience = employeeNumberOfYears.Get(name.Value);
                if (yearsOfExperience >= requirement.Years)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
