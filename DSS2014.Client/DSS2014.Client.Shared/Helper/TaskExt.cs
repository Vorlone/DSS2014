using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSS2014.Client.Helper
{
    public static class TaskExt
    {
        public static async Task<TEventArgs> FromEvent<TEventArgs>(
                Action<EventHandler<TEventArgs>> registerEvent,
                Action action,
                Action<EventHandler<TEventArgs>> unregisterEvent,
                CancellationToken token)
        {
            var tcs = new TaskCompletionSource<TEventArgs>();

            EventHandler<TEventArgs> handler = (sender, args) =>
                tcs.TrySetResult(args);

            registerEvent(handler);
            try
            {
                using (token.Register(() => tcs.SetCanceled()))
                {
                    action();
                    return await tcs.Task;
                }
            }
            finally
            {
                unregisterEvent(handler);
            }
        }
    }
}
