using System;
using System.Runtime.Serialization;

namespace FileWorkerService
{
    [System.Serializable]
    internal class WorkProcedureException : Exception
    {
        private Exception ex;

        public WorkProcedureException()
        {
        }

        public WorkProcedureException(Exception ex)
        {
            this.ex = ex;
        }

        public WorkProcedureException(string message) : base(message)
        {
        }

        public WorkProcedureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WorkProcedureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}