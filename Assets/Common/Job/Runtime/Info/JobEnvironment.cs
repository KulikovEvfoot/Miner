namespace Common.Job.Runtime 
{
    public class JobEnvironment
    {
        public class JobQualification
        {
            public const string None = "None";
            public const string Todo = "Todo";
            public const string InProgress = "InProgress";
            public const string Done = "Done";
        }
        
        public class JobStatus
        {
            public const string Todo = "Todo";
            public const string InProgress = "InProgress";
            public const string Done = "Done";
        }
    }
}