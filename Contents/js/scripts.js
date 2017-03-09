$(document).ready(function(){
    $("#encode").click(function(){
      var geocoder = new google.maps.Geocoder();
      var address = jQuery('#address').val();

      geocoder.geocode( { 'address': address}, function(results, status) {

        if (status == google.maps.GeocoderStatus.OK) {
          var latitude = results[0].geometry.location.lat();
          var longitude = results[0].geometry.location.lng();
          jQuery('#coordinates').val(latitude+', '+longitude);
        }
      })
    });

    $("form").submit(function(event) {
      var newString = "C:\\Users\\epicodus\\desktop\\tinder_app\\Contents\\Profile\\input\\" + $("input#myFile").val().replace("C:\\fakepath\\","");
      $("#path").val(newString);
    });

  });
