/* Combining HTML5 Geolocation and Rx
 *
 * Geolocation is a good example of an async API that we can use with Rx once 
 * we write some code to bridge the gap. Since almost all API calls that do any
 * work in JavaScript are async and almost all of them follow a similar 
 * callback-based pattern, it's usually really easy and mechanical to wrap
 * them into an Observable.
 */

//
// Create an Rx version of getCurrentPosition, which calls getCurrentPosition
// then returns an IObservable object. 
// 

getCurrentPositionRx = function(opts) {
  opts = opts || {};
  var ret = new Rx.AsyncSubject();

  // Our callbacks will just OnNext the Subject, similar to how FromAsyncPattern
  // works.
  navigator.geolocation.getCurrentPosition(
    function(pos) { ret.OnNext([pos.coords.latitude, pos.coords.longitude]); ret.OnCompleted(); },
    function(err) { ret.OnError(err.code); },
    opts);

  return ret;
};

//
// Create an Observable that watches the Selection form element, and select out
// the value of the option selected (i.e. in the HTML, it's the 'value'
// attribute on each of the Option elements)
//

var mapChangeObservable = $("#mapOpts").toObservable("change")
  .Select(function(x) { return x.currentTarget; })
  .Select(function(x) { return x[x.options.selectedIndex].value })
  .StartWith("roadmap");

mapChangeObservable.Subscribe(function(x) { console.log(x); });

var currentMapUrl = mapChangeObservable.SelectMany(function(mapType) {

  // Get the current position - if it fails, we'll instead return a canned
  // default position, just like how you would do it in Rx.NET
  var mapPos = getCurrentPositionRx().Catch(
    Rx.Observable.Return([40.714728, -73.998672]));

  return mapPos.Select(function(pos) {
    return [mapType, pos];
  });
}).Select(function(typeAndPos) {
    var mapType = typeAndPos[0];  var coords = typeAndPos[1];

    // Our selector will return the URL of the Google Static Maps image
    return "http://maps.googleapis.com/maps/api/staticmap?zoom=12&size=400x400&sensor=true" 
      + "&maptype=" + mapType
      + "&center=" + coords[0] + "," + coords[1];
  });

currentMapUrl
  .Subscribe(function(x) { 
    // Set the 'src' parameter of the mapImage element to our URL
    $("#mapImage").attr("src", x); 
  });

// vim: ts=2 sw=2 et :
