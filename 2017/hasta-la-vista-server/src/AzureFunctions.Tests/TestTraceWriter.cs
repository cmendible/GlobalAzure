using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctions.Tests
{
    public class TestTraceWriter : TraceWriter
    {
        public TestTraceWriter(TraceLevel level) : base(level)
        {
        }

        public override void Trace(TraceEvent traceEvent)
        {
            Debug.WriteLine(traceEvent.Message);
        }
    }
}
