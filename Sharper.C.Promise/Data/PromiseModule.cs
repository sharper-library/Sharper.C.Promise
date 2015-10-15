using System.Threading;
using Sharper.C.Data;

namespace Sharper.C.Data
{

using static UnitModule;

public static class PromiseModule
{
    public sealed class Promise<A>
    {
        private readonly CountdownEvent latch;
        private A value;

        internal Promise()
        {
            value = default(A);
            latch = new CountdownEvent(1);
        }

        public Unit Fulfill(A a)
        {
            lock (latch)
            {
                if (!latch.IsSet)
                {
                    value = a;
                    latch.Signal();
                }
            }
            return UNIT;
        }

        public A Wait
        {
            get
            {
                latch.Wait();
                return value;
            }
        }
    }

    public static Promise<A> MkPromise<A>()
    =>
        new Promise<A>();
}

}
