/* Example 2 - translating an existing C# sample to RxJS
 *
 * Let's take an example that was illustrated earlier, the Merge operator, and
 * translate it to RxJS. 
 *
 */

Rx.Observable.Concat(
  Rx.Observable.Return(1),
  Rx.Observable.Return(2),
  Rx.Observable.Return(3),
  Rx.Observable.Return(4)
).Subscribe(function(x) {
  window.alert("Number " + x);
});

// vim: ts=2 sw=2 et :
