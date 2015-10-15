using System.Threading;

namespace Sharper.C.Data
{

using static UnitModule;

public static class PromiseModule
{
    public sealed class Promise<A>
    {
        private readonly object lockobj = new object();
        private readonly CountdownEvent latch;
        private A value;

        internal Promise()
        {
            value = default(A);
            latch = new CountdownEvent(1);
        }

        public Unit Fulfill(A a)
        {
            lock (lockobj)
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
