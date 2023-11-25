using System.Collections.Generic;
using UnityEngine;

namespace Common.Job.Runtime
{
    public class JobOperationController
    {
        private readonly Dictionary<string, JobOperation> m_JobOperations;

        private string m_CurrentOperationName;
        
        public JobOperationController(Dictionary<string, JobOperation> jobOperations)
        {
            m_JobOperations = jobOperations;
        }

        public void InvokeOperation(string operationName)
        {
            var operation = m_JobOperations[operationName].Operation;
            m_CurrentOperationName = operationName;
            operation?.Invoke();
        }

        public void OnOperationComplete(string resultKey)
        {
            var operationResult = m_JobOperations[m_CurrentOperationName].Result;
            
            if (!operationResult.TryGetValue(resultKey, out var nextOperationName))
            { 
                Debug.LogError($"{nameof(JobOperationController)} >> can't find operation by key = {resultKey}");
                return;
            }

            if (nextOperationName == JobOperation.Done)
            {
                return;
            }
            
            var nextOperation = m_JobOperations[nextOperationName].Operation;
            m_CurrentOperationName = nextOperationName;
            nextOperation?.Invoke();
        }
    }
}