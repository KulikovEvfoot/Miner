using System;
using System.Collections.Generic;
using Common.Job.Runtime.Employee;
using Core.EventProducer.Runtime;

namespace Common.Job.Runtime.Info
{
    public class JobInfo
    {
        private readonly EventProducer<IJobInfoObserver> m_EventProducer;

        private string m_JobStatus;
        private IEmployee m_Assignee;

        public IJob Job { get; }
        public Type Qualification { get; }

        public string JobStatus
        {
            get => m_JobStatus;
            set
            {
                if (m_JobStatus == value)
                {
                    return;
                }
                
                m_JobStatus = value;
                OnValueChanged();
            }
        }
        
        public IEmployee Assignee
        {
            get => m_Assignee;
            set
            {
                if (m_Assignee == value)
                {
                    return;
                }
                
                m_Assignee = value;
                OnValueChanged();
            }
        }
        
        public JobInfo(IJob job, Type qualification, EventProducer<IJobInfoObserver> eventProducer)
        {
            m_EventProducer = eventProducer;
            
            Job = job;
            Qualification = qualification;
            JobStatus = JobEnvironment.JobStatus.Todo;
        }
        
        public bool HasAssignee()
        {
            return Assignee != null;
        }
        
        private void OnValueChanged()
        {
            m_EventProducer.NotifyAll(obs => obs.NotifyOnJobInfoChanged(this));
        }
    }
}