namespace Services.Job.Runtime
{
    public class EmployeeInfo
    {
        public string EmployeeStatus { get; set; }

        public EmployeeInfo(string employeeStatus)
        {
            EmployeeStatus = employeeStatus;
        }
    }
}