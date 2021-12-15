using System;

namespace PolicyBasedAuthourization.AuthorizationModel
{
    public class EmployeeNumberOfYears : IEmployeeNumberOfYears
    {
        public EmployeeNumberOfYears()
        {
        }

        public int Get(string name)
        {
            return name.Equals("test2") ? 21 : 10;
        }
    }
}
