$(document).ready(function(){
  $('#areafilter').on('click', function() {
        UIkit.notification($(this).data());
    });
  $('#foodfilter').on('click', function() {
        UIkit.notification($(this).data());
    });
  $('#hobbyfilter').on('click', function() {
        UIkit.notification($(this).data());
    });
    var oldRating = "#rating-" + $("#avg-rating").val().toString();
    $(oldRating).attr("checked", true)
});
