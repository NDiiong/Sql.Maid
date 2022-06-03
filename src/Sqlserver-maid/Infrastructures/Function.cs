using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sqlserver.maid.Infrastructures
{
    public static class Function
    {
        internal static void Run<T>(Action action)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                var message = $"Invoke command {typeof(T).Name} - in {sw.ElapsedMilliseconds} ms";
            }
        }

        internal static T Run<T>(Func<T> action)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                return action.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                var message = $"Invoke command {typeof(T).Name} - in {sw.ElapsedMilliseconds} ms";
            }

            return default;
        }

        internal static void Run<T>(Action action, string methodName)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var message = $"Invoke command {typeof(T).Name}";
                action.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                var message = $"\t> Invoke method {methodName} - in {sw.ElapsedMilliseconds} ms";
            }
        }

        internal static void Run<T>(Action action, Action callbackException)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Run<T>(callbackException, "CallBackWhenException");
            }
            finally
            {
                var message = $"Invoke command {typeof(T).Name} - in {sw.ElapsedMilliseconds} ms";
            }
        }

        internal static async Task RunAsync<T>(Func<Task> func)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                var message = $"Invoke {typeof(T).Name} - in {sw.ElapsedMilliseconds} ms";
            }
        }
    }
}
