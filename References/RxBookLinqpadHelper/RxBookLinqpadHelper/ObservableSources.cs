using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Text;

namespace RxBookLinqpadHelper
{
    public partial class RxBook
    {
        public static IObservable<string> FetchWebpage(string url)
        {
            var hwr = WebRequest.CreateDefault(new Uri(url)) as HttpWebRequest;
            var requestFunc = Observable.FromAsyncPattern<WebResponse>(hwr.BeginGetResponse, hwr.EndGetResponse);

            return requestFunc().Select(x => {
                var ms = new MemoryStream();
                x.GetResponseStream().CopyTo(ms);
                return Encoding.UTF8.GetString(ms.ToArray());
            });
        }
    }
}
