
$(document).ready(function(){
    $("form").submit(function(event) {
      event.preventDefault();
      var newString = "C:\\Users\\epicodus\\desktop\\tinder_app\\Contents\\Profile\\input\\" + $("input#myFile").val().replace("C:\\fakepath\\","");
      $("#path").val(newString);
    });
});
