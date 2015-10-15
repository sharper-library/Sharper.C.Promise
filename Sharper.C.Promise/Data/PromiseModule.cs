using System;
using System.Threading;

namespace Sharper.C.Data
{

using static UnitModule;

public static class PromiseModule
{
    public sealed class Promise<A>
    {
        private static int id = 0;
        private readonly int[] lockobj = new[] {id++};
        private readonly CountdownEvent latch;
        private A value;

        internal Promise()
        {
            value = default(A);
            latch = new CountdownEvent(1);
        }

        public Unit Fulfill(A a)
        {
            Console.WriteLine($"outside {lockobj[0]}");
            lock (lockobj)
            {
                Console.WriteLine($"inside {lockobj[0]}");
                if (!latch.IsSet)
                {
                    Console.WriteLine($"fulfilling {lockobj[0]}");
                    value = a;
                    latch.Signal();
                }
                Console.WriteLine($"...inside {lockobj[0]}");
            }
            Console.WriteLine($"...outside {lockobj[0]}");
            return UNIT;
        }

        public A Wait
        {
            get
            {
                Console.WriteLine($"Delay... {lockobj[0]}");
                latch.Wait();
                Console.WriteLine($"...donE {lockobj[0]}");
                return value;
            }
        }
    }

    public static Promise<A> MkPromise<A>()
    =>
        new Promise<A>();
}

}
