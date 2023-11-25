using System.Collections.Generic;
using System.Linq;
using Common.Job.Runtime.Employee;
using Common.Job.Runtime.Info;

namespace Common.Job.Runtime
{
    public class JobService : IJobFactoryObserver, IEmployeeFactoryObserver, IJobInfoObserver
    {
        private readonly List<IEmployee> m_Employees;
        private readonly List<IJob> m_Jobs;

        public JobService()
        {
            m_Employees = new List<IEmployee>();
            m_Jobs = new List<IJob>();
        }

        public void NotifyOnJobCreated(IJob job)
        {
            m_Jobs.Add(job);
            GiveJobsToUnemployed();
        }

        public void NotifyOnEmployeeCreated(IEmployee employee)
        {
            m_Employees.Add(employee);
            GiveJobsToUnemployed();
        }

        public void NotifyOnJobInfoChanged(JobInfo jobInfo)
        {
            if (jobInfo.JobStatus == JobEnvironment.JobStatus.Done)
            {
                jobInfo.Assignee.DequeueJob();
                m_Jobs.Remove(jobInfo.Job);
                GiveJobsToUnemployed();
            }
        }

        private void GiveJobsToUnemployed()
        {
            foreach (var employee in m_Employees.Where(e => e.GetEmployeeStatus() == EmployeeStatus.Unemployed))
            {
                foreach (var job in m_Jobs)
                {
                    var jobInfo = job.GetJobInfo();
                    if (jobInfo.JobStatus != JobEnvironment.JobStatus.Todo || jobInfo.HasAssignee())
                    {
                        continue;
                    }
                    
                    var qualification = job.GetJobInfo().Qualification;
                    if (employee.GetType().IsAssignableFrom(qualification))
                    {
                        continue;
                    }
                    
                    jobInfo.Assignee = employee;
                    jobInfo.Assignee.EnqueueJob(job);
                    break;
                }
            }
        }
        

    }
}