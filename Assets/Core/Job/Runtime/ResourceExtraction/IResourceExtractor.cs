﻿using System;
using Common.Job.Runtime.Employee;
using Common.Navigation.Runtime.Waypoint;

namespace Common.Job.Runtime.ResourceExtraction
{
    public interface IResourceExtractor : IEmployee
    {
        void MoveTo(IWaypoint waypoint, Action<string> operationResult);
        void OnExtractingResource(Action<string> operationResult);
        void FinalizeJob();
    }
}