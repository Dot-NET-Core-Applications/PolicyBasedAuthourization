using System;


namespace PolicyBasedAuthourization.AuthorizationModel
{
    public interface IEmployeeNumberOfYears
    {
        /// <summary>
        /// Gets the no. of years of for Employee service
        /// </summary>
        /// <param name="name">Name of the employee.</param>
        /// <returns>No of years of service.</returns>
        int Get(string name);
    }
}
