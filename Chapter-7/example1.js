/* The simplest thing that could possibly work
 *
 * Let's try to take a very simple sample and convert it to RxJS:
 *
 * The biggest difference here, is that we can see that Observable.Return is 
 * prefixed by an "Rx." - any method that is normally static is now prefixed
 * with Rx to avoid polluting the global scope.
 */

var simpleSubscription = Rx.Observable.Return(17);

simpleSubscription
  .Select(function(x) { return x.toString(); })
  .Subscribe(function(x) {
    $("#content").text("The value is " + x.toString());
});


// vim: ts=2 sw=2 et :
