var currentlyOpenedInfoWindow = null;

function init_map(map_canvas_id, lat, lng, zoom, markers, infoWindowContents) {
    var myLatLng = new google.maps.LatLng(lat, lng);
    
    var options = {
        zoom: zoom,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    
    var map_canvas = document.getElementById(map_canvas_id);

    var map = new google.maps.Map(map_canvas, options);

    if (markers && markers.length > 0) {
        var bounds = new google.maps.LatLngBounds();
    
        for (var i = 0; i < markers.length; i++) {
            var marker = new google.maps.Marker(markers[i]);
            marker.setMap(map);

            bounds.extend(marker.getPosition());

            if (infoWindowContents && infoWindowContents.length > i)
                createInfoWindow(map, marker, infoWindowContents[i]);
        }

        map.fitBounds(bounds);
        map.setCenter(bounds.getCenter());
    }
}

function createInfoWindow(map, marker, infoWindowProperties) {
    var info = new google.maps.InfoWindow(infoWindowProperties);

    google.maps.event.addListener(marker, 'click', function() {
        if (currentlyOpenedInfoWindow != null)
            currentlyOpenedInfoWindow.close();

        info.open(map, marker);
        currentlyOpenedInfoWindow = info;
    });
}