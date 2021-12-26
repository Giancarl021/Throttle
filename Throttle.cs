using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Throttle
{
    public class Throttler
    {
        public static async Task Throttle(IEnumerable<Func<Task>> callbacks, int ticks)
        {
            if (ticks <= 0)
            {
                throw new ArgumentException("ticks must be greater than 0");
            }

            if (ticks == 1)
            {
                foreach (var callback in callbacks) await callback();
                return;
            }

            int l = callbacks.Count();
            var block = new Task[ticks];

            for (int i = 0; i < l; i++)
            {
                var callback = callbacks.ElementAt(i);

                if (i > 0 && i % ticks == 0)
                {
                    await Task.WhenAll(block);
                    block = new Task[ticks];
                }

                block[i % ticks] = callback();
            }

            await Task.WhenAll(block.Where(t => t != null));
        }

        public static async Task<IEnumerable<T>> Throttle<T>(IEnumerable<Func<Task<T>>> callbacks, int ticks)
        {
            if (ticks <= 0)
            {
                throw new ArgumentException("ticks must be greater than 0");
            }

            var results = new List<T>();

            if (ticks == 1)
            {
                foreach (var callback in callbacks) results.Add(await callback());
                return results;
            }

            int l = callbacks.Count();
            var block = new Task<T>[ticks];

            for (int i = 0; i < l; i++)
            {
                var callback = callbacks.ElementAt(i);

                if (i > 0 && i % ticks == 0)
                {
                    results.AddRange(await Task.WhenAll(block));
                    block = new Task<T>[ticks];
                }

                block[i % ticks] = callback();
            }

            results.AddRange(await Task.WhenAll(block.Where(t => t != null)));

            return results;
        }
    }
}