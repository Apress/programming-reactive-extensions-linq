
var searchGitHub = function(term) {
  var params = {
    url: "http://github.com/api/v2/json/repos/search/" + encodeURI(term),
    data: 'json'
  };

  return $.ajaxAsObservable(params)
    .Select(function(x) { return x.data.repositories });
}

var textBoxChanges = $('#searchInput')
  .toObservable('keyup')
  .Select(function(x) { return $('#searchInput').val(); })
  .Throttle(600)
  .Where(function(x) { return /^\s*$/.test(x) !== true; });

var searchResults = textBoxChanges
  .Select(function(x) { return searchGitHub(x); })
  .Switch();

searchResults.Subscribe(function(repos) {
  $('#content').empty();

  var count = 0;
  $.each(repos, function(x, value) {
    if (++count > 10) {
      return;
    }
    $('#content').append('<li><b>' + value.name + ':</b> - ' + value.description + '</li>');
  });
}, function(ex) { 
  console.log(ex); 
});


// vim: ts=2 sw=2 et :
