﻿namespace OpenCensus.Trace.Export
{
    using System.Threading;
    using OpenCensus.Common;

    public sealed class SpanExporter : SpanExporterBase
    {
        private SpanExporterWorker _worker { get; }

        private readonly Thread workerThread;

        internal SpanExporter(SpanExporterWorker worker)
        {
            _worker = worker;
            workerThread = new Thread(worker.Run)
            {
                IsBackground = true,
                Name = "SpanExporter"
            };
            workerThread.Start();
        }

        internal static ISpanExporter Create(int bufferSize, IDuration scheduleDelay)
        {
            SpanExporterWorker worker = new SpanExporterWorker(bufferSize, scheduleDelay);
            return new SpanExporter(worker);
        }

        public override void AddSpan(ISpan span)
        {
            _worker.AddSpan(span);
        }

        public override void RegisterHandler(string name, IHandler handler)
        {
            _worker.RegisterHandler(name, handler);
        }

        public override void UnregisterHandler(string name)
        {
            _worker.UnregisterHandler(name);
        }

        public override void Dispose()
        {
            _worker.Dispose();
        }

        internal Thread ServiceExporterThread
        {
            get
            {
                return workerThread;
            }
        }
    }
}
