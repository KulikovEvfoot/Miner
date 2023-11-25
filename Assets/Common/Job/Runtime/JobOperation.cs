using System;
using System.Collections.Generic;

namespace Common.Job.Runtime.Employee
{
    public class JobOperation
    {
        public const string Done = "done";
        
        public Action Operation { get; }
        public Dictionary<string, string> Result { get; }

        public JobOperation(Action operation, Dictionary<string, string> result)
        {
            Operation = operation;
            Result = result;
        }
    }
}