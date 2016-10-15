using System;
using System.Reactive;
using Microsoft.Reactive.Testing;

namespace RxBookLinqpadHelper
{
    public static class TestUtils
    {
        /// <summary>
        /// RunToMilliseconds moves the TestScheduler to the specified time in
        /// milliseconds.
        /// </summary>
        /// <param name="milliseconds">The time offset to set the TestScheduler
        /// to, in milliseconds. Note that this is *not* additive or
        /// incremental, it sets the time.</param>
        public static void AdvanceToMilliseconds(this TestScheduler sched, double milliseconds)
        {
            Console.WriteLine("Running to time t={0}", milliseconds);
            sched.AdvanceTo(sched.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds)));
        }

        /// <summary>
        /// OnNextAt is a method to help create simulated input Observables in
        /// conjunction with CreateHotObservable or CreateColdObservable.
        /// </summary>
        /// <param name="milliseconds">The time offset to fire the notification
        /// on the recorded notification.</param>
        /// <param name="value">The value to produce.</param>
        /// <returns>A recorded notification that can be provided to
        /// TestScheduler.CreateHotObservable.</returns>
        public static Recorded<Notification<T>> OnNextAt<T>(this TestScheduler sched, double milliseconds, T value)
        {
            return new Recorded<Notification<T>>(
                sched.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds)),
                Notification.CreateOnNext<T>(value));
        }

        /// <summary>
        /// OnErrorAt is a method to help create simulated input Observables in
        /// conjunction with CreateHotObservable or CreateColdObservable.
        /// </summary>
        /// <param name="milliseconds">The time offset to fire the notification
        /// on the recorded notification.</param>
        /// <param name="exception">The exception to terminate the Observable
        /// with.</param>
        /// <returns>A recorded notification that can be provided to
        /// TestScheduler.CreateHotObservable.</returns>
        public static Recorded<Notification<T>> OnErrorAt<T>(this TestScheduler sched, double milliseconds, Exception ex)
        {
            return new Recorded<Notification<T>>(
                sched.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds)),
                Notification.CreateOnError<T>(ex));
        }
        
        /// <summary>
        /// OnCompletedAt is a method to help create simulated input Observables in
        /// conjunction with CreateHotObservable or CreateColdObservable.
        /// </summary>
        /// <param name="milliseconds">The time offset to fire the notification
        /// on the recorded notification.</param>
        /// <returns>A recorded notification that can be provided to
        /// TestScheduler.CreateHotObservable.</returns>
        public static Recorded<Notification<T>> OnCompletedAt<T>(this TestScheduler sched, double milliseconds)
        {
            return new Recorded<Notification<T>>(
                sched.FromTimeSpan(TimeSpan.FromMilliseconds(milliseconds)),
                Notification.CreateOnCompleted<T>());
        }

        internal static long FromTimeSpan(this TestScheduler sched, TimeSpan span)
        {
            return span.Ticks;
        }
    }
}

// vim: tw=120 ts=4 sw=4 et :
