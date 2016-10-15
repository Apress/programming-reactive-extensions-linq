using System;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chapter4.Examples
{
    [TestClass]
    public class AsyncMethodsTest
    {
        [TestMethod]
        public void ChainMethodsAsynchronously()
        {
            int result = 0;
            AsyncMethods.AddTwoNumbersAsync(5, 10)
                .SelectMany(aPlusB => AsyncMethods.MultiplyByFiveObservable(aPlusB))
                .Subscribe(x => result = x);

            // This isn't a good idea in general, see chapter 9!
            Thread.Sleep(1000);
            Assert.AreEqual(75, result);
        }
    }

    [TestClass]
    public class DownloadPageTest
    {
        [TestMethod]
        public void DownloadPageTextAsyncCallback()
        {
            string result = null;
            DownloadPageText.DownloadPageTextAsync("http://jesseliberty.com/", s => result = s);

            // I must stress again how terrible of an idea this is, see chapter 9!
            int i = 15*4;
            while(result == null && --i > 0)
            {
                Thread.Sleep(250);
            }

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Contains("Podcast"));
        }

        [TestMethod]
        public void DownloadPageTextRx()
        {
            var result = DownloadPageText.DownloadPageTextRx("http://jesseliberty.com/")
                .First();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Contains("Podcast"));
        }


        [TestMethod]
        public void DownloadPageTextFromCallbackPattern()
        {
            var rxFunc = DownloadPageText.FromCallbackPattern<string, string>(
                DownloadPageText.DownloadPageTextAsync);

            var result = rxFunc("http://jesseliberty.com/").First();

            Assert.IsTrue(result != null);
            Assert.IsTrue(result.Contains("Podcast"));
        }
    }
}
