using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using RxBookLinqpadHelper;

namespace Chapter4
{
    public static class AsyncMethods
    {
        public static int AddTwoNumbers(int a, int b)
        {
            return a + b;
        }

        public static IObservable<int> AddTwoNumbersAsync(int a, int b)
        {
            return Observable.Start(() => AddTwoNumbers(a, b));
        }

        public static IObservable<int> AddTwoNumbersObservable(int a, int b)
        {
            return Observable.Return(AddTwoNumbers(a, b));
        }

        public static IObservable<int> MultiplyByFiveObservable(int a)
        {
            return Observable.Return(a * 5);
        }
    }

    public static class DownloadPageText
    {
        /* Getting from A to B:
         *
         * This is what it might look like if we wrapped this method by-hand. Writing
         * this code over and over again would get really boring, but it's good to do it
         * once to see what it would look like.
         */
        public static IObservable<string> DownloadPageTextRx(string url)
        {
            var subject = new AsyncSubject<string>();

            // Call our original method
            try
            {
                DownloadPageTextAsync(url, (pageText) =>
                {
                    subject.OnNext(pageText);
                    subject.OnCompleted();
                });
            }
            catch (Exception ex)
            {
                subject.OnError(ex);
            }

            return subject;
        }


        /* Now, let's make this really generic - let's wrap *any* method that takes one
         * parameter. We could write really similar versions of this for 2,3,4,etc
         * parameter methods  */

        public static Func<T1, IObservable<TRet>> FromCallbackPattern<T1, TRet>(Action<T1, Action<TRet>> originalMethod)
        {
            return param1 =>
            {
                var subject = new AsyncSubject<TRet>();

                try
                {
                    originalMethod(param1, (result) =>
                    {
                        subject.OnNext(result);
                        subject.OnCompleted();
                    });
                }
                catch (Exception ex)
                {
                    subject.OnError(ex);
                }

                return subject;
            };
        }


        // Normally this would be implemented some other way, ignore this man
        // behind the curtain!
        public static void DownloadPageTextAsync(string url, Action<string> pageData)
        {
            RxBook.FetchWebpage(url).Subscribe(pageData);
        }
    }
}
