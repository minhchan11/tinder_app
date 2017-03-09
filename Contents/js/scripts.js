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

    $("#sign-up").click(function(){
      $(".map").append("<p>" + "Use Current Location" + "</p>" +
          "<input type='hidden' name='location' id='location' value=''>" +
          '<div id="map-block">'+
            '<div id="map"></div>'+
            '<script type="text/javascript" src="/Contents/js/map.js"></script>'+
            '<script async defer src="http://maps.google.com/maps/api/js?key=AIzaSyB2yaBqgDWOKFDfzPfifIqwXk_j2OZJnCI&callback=initMap"></script>'+
          '</div>');
    })

  });
