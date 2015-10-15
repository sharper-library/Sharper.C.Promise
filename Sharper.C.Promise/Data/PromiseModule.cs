using System;
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
            Console.WriteLine("outside");
            lock (lockobj)
            {
                Console.WriteLine("inside");
                if (!latch.IsSet)
                {
                    Console.WriteLine("fulfilling");
                    value = a;
                    latch.Signal();
                }
                Console.WriteLine("...inside");
            }
            Console.WriteLine("...outside");
            return UNIT;
        }

        public A Wait
        {
            get
            {
                Console.WriteLine("Delay...");
                latch.Wait();
                Console.WriteLine("...donE");
                return value;
            }
        }
    }

    public static Promise<A> MkPromise<A>()
    =>
        new Promise<A>();
}

}
