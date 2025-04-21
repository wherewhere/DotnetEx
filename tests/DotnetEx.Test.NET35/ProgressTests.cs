using NUnit.Framework;
using System;
using System.Threading;

namespace DotnetEx.Test
{
    /// <summary>
    /// The tests for the <see cref="Progress{T}"/> class.
    /// </summary>
    [TestFixture]
    public static class ProgressTests
    {
        [Test]
        public static void Ctor()
        {
            new Progress<Int32>();
            new Progress<Int32>(i => { });
            Assert.Throws<ArgumentNullException>(() => new Progress<Int32>(null));
        }

        [Test]
        public static void NoWorkQueuedIfNoHandlers()
        {
            RunWithoutSyncCtx(() =>
            {
                TrackingSynchronizationContext tsc = new();
                SynchronizationContext.SetSynchronizationContext(tsc);
                Progress<Int32> p = new();
                for (int i = 0; i < 3; i++)
                    ((IProgress<Int32>)p).Report(i);
                Assert.AreEqual(0, tsc.Posts);
                SynchronizationContext.SetSynchronizationContext(null);
            });
        }

        [Test]
        public static void TargetsCurrentSynchronizationContext()
        {
            RunWithoutSyncCtx(() =>
            {
                TrackingSynchronizationContext tsc = new();
                SynchronizationContext.SetSynchronizationContext(tsc);
                Progress<Int32> p = new(i => { });
                for (int i = 0; i < 3; i++)
                    ((IProgress<Int32>)p).Report(i);
                Assert.AreEqual(3, tsc.Posts);
                SynchronizationContext.SetSynchronizationContext(null);
            });
        }

        private static void RunWithoutSyncCtx(Action action)
        {
            using ManualResetEvent manual = new(false);
            _ = ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    action();
                }
                finally
                {
                    manual.Set();
                }
            });
            manual.WaitOne();
        }

        private sealed class Int32(int value) : EventArgs
        {
            public int Value => value;
            public static implicit operator Int32(int value) => new(value);
            public static implicit operator int(Int32 value) => value.Value;
        }

        private sealed class TrackingSynchronizationContext : SynchronizationContext
        {
            internal int Posts = 0;

            public override void Post(SendOrPostCallback d, object state)
            {
                Posts++;
                base.Post(d, state);
            }
        }
    }
}
