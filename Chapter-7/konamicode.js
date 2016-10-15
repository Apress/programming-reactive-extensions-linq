/* Detecting the Konami code in RxJS
 *
 * Here's a more complex example - let's check for the Konami Code on our
 * webpage and activate a cheat code if someone types it in. This looks more
 * complex than original example, but only because JavaScript has no equivalent
 * of Enumerable.SequenceEqual (i.e. check if two lists have the same elements)
 */

// NB: These values work for a US keyboard, but they probably don't if you're 
// using another keyboard layout. You might have to do some experimentation to
// find the right values.
var up = 38;
var down = 40;
var left = 37;
var right = 39;
var b = 66;
var a = 65;
var enter = 13;

var konamiCode = [up, up, down, down, left, right, left, right, b, a, enter];

var konamiCodeFound = $(window).toObservable("keyup")
  .Select(function(x) { return x.keyCode })
  .BufferWithCount(konamiCode.length, 1)
  .Select(function(sequence) {
    // We now have two arrays, our konamiCode array, and an array of the last 7
    // keys pressed - compare the two to see if they're equal (i.e. the last 7
    // keys pressed was the Konami code)
    for (var i = 0; i < konamiCode.length; i++) {
      if (sequence[i] !== konamiCode[i]) {
        return true;
      }
    }

    return false;
  });

konamiCodeFound
  .Where(function(x) { return (x === true); })
  .Timeout(10 * 1000)
  .Take(1)
  .Catch(Rx.Observable.Return(false))
  .Subscribe(function(foundInTime) {
    if (foundInTime) {
      $("#content").text("CheatCode Found!");
    } else {
      $("#hint").fadeIn("fast");
    }
  });

// vim: ts=2 sw=2 et :
