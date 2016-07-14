// Global Variables
var map;
var defaultLatLng = { lat: 51.309810, lng: 10.169846 };
var bounds;
var loc;
var lastOpenedWindow;
var markers = [];
var NoValueError = "Please insert a value for searching";
var NoGeoloccationFoundError = "No Geolocation found";

// Dom Ready initializations
$(document).ready(function() {
    // Change Event Handler to get input from the petrolType drop down
    // Needs to track if changes at the sorting have to be made 
    // PetrolType: All => No Price Sorting possible
    $("#petrolType").change(function () {
        var selected = $("#petrolType").find(":selected").attr("value");
        var sortBy = $("#sortBy").find(":selected").attr("value");
        if (selected === "0") {
            $("#sortBy").find("option[value='0']").prop("disabled", true);
            if (sortBy === "0") {
                $("#sortBy").val("1");
            }
        } else {
            $("#sortBy").find("option[value='0']").prop("disabled", false);
        }
    });

    // Enter Click Event for Search
    $("#SearchBox").keydown(function (e) {
        if (e.keyCode === 13) {
            ProcessSearch();
        }
    });

    // Click Event for Search
    $("#SearchForPetrolStations").click(function () {
        ProcessSearch();
    });
});

// Function to initialize the Petrol Station Map
function initPetrolMap() {

    // Create a map object and specify the DOM element for display.
    map = new google.maps.Map($("#PetrolStationMap")[0], {
        center: defaultLatLng,
        scrollwheel: true,
        zoom: 7,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        mapTypeControl: false,
        scaleControl: true,
        streetViewControl: false
    });

    google.maps.event.addListener(map, 'tilesloaded', function() {
        $("#ajaxLoader").hide();
    });
}

// Removes all Markers from the map
function RemoveAllMarkers() {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(null);
    }
    markers = [];
}

// Update Method for Google maps an its marker
function MarkerUpdateFromResults() {

    // Clear all Marker before new ones are set
    RemoveAllMarkers();

    // Initialize a dummy info window
    lastOpenedWindow = new google.maps.InfoWindow();

    // Initialize Bounds
    bounds = new google.maps.LatLngBounds();

    // Go in Every Result and get hidden information
    $(".PetrolStationDetails").each(function () {
        var lat = $(this).find(".lat").html();
        var lng = $(this).find(".lng").html();

        loc = new google.maps.LatLng(lat, lng);
        bounds.extend(loc);

        // Create a Marker
        var marker = new google.maps.Marker({
            position: loc,
            map: map,
            title: 'Marker'
        });

        markers.push(marker);

        var textField = $(this).html();

        // Add a Click Callback for more Information on the map
        google.maps.event.addListener(marker, 'click', function () {
            map.panTo(marker.getPosition());

            lastOpenedWindow.close();

            var infowindow = new google.maps.InfoWindow({
                content: textField
            });

            // Remember the Last opened Info Window to close it later
            lastOpenedWindow = infowindow;

            infowindow.open(map, marker);
        });
    });

    map.fitBounds(bounds);
    map.panToBounds(bounds);
}

// Processes the Search 
function ProcessSearch() {
    // Get the input values
    var inputPlz = $("#SearchBox").val();

    // Validation
    if (inputPlz.length === 0) {
        // Error Case -> Change the Appearance in frontend
        $("#ErrorLabel").removeClass("ErrorLabelInitital").addClass("ErrorLabelVisible");
        $("#ErrorLabel").text(NoValueError);
        $("#SearchBox").removeClass("InputStandard").addClass("InputError");
        return;
    }
    // Gray Out the Screen to demonstrate Loading
    $(".grayout").fadeTo(500, 0.3);

    // Not Error case -> change appearance in frontend
    $("#SearchBox").removeClass("InputError").addClass("InputStandard");
    $("#ErrorLabel").removeClass("ErrorLabelVisible").addClass("ErrorLabelInitital");
    // End Validation

    // Get the Values of the dropdowns
    var selectedPetrolType = $("#petrolType").find(":selected").attr("value");
    var selectedSortBy = $("#sortBy").find(":selected").attr("value");

    // Save all values in the frontend in hidden values
    $("#HiddenInput").text(inputPlz);
    $("#HiddenPetrolType").text(selectedPetrolType);
    $("#HiddenSortBy").text(selectedSortBy);
    // Request the server with all the values
    CallForUpdate(inputPlz, selectedSortBy, selectedPetrolType);
}

// Does the ajax call for geocoding and updates the Map
function CallForUpdate(inputPlz, selected, selectedPetrolType) {
    $.ajax({
        type: "POST",
        url: "/Home/GeolocationMapper",
        data: { 'input': inputPlz },
        context: document.body
    }).done(function (data) {
        if (data.length === 0) {
            // Error case -> Change appearance
            $("#ErrorLabel").removeClass("ErrorLabelInitital").addClass("ErrorLabelVisible");
            $("#ErrorLabel").text(NoGeoloccationFoundError);
            $("#SearchBox").removeClass("InputStandard").addClass("InputError");
            return;
        }

        // No Error case -> Change appearance
        $("#SearchBox").removeClass("InputError").addClass("InputStandard");
        $("#ErrorLabel").removeClass("ErrorLabelVisible").addClass("ErrorLabelInitital");

        // Get the neccessary Lat and Lng
        var lat = data["Lat"];
        var lng = data["Lng"];
        // Do the Request for Petrol Stations
        UpdatePetrolStations(selected, lat, lng, selectedPetrolType);
    });
}

// Update which loads all Results into the markup based on the parametes 
function UpdatePetrolStations(selected, lat, lng, selectedPetrolType) {
    // Build the Url for the Request for petrol stations
    var url = "/Home/UpdatePetrolStations?sortBy=" + selected + "&lat=" + lat + "&lng=" + lng + "&petrolType=" + selectedPetrolType;
    $.get(url, null, function (data) {
        $("#PetrolServiceResults").html(data);
    }).done(function () {
        // Scroll down to the results
        $("html, body").animate({
            scrollTop: $("#HeadlinePetrolStation").offset().top - 55
        }, 1000);
        // Remove the gray Loading screen
        $(".grayout").fadeTo(500, 1.0, function () {
            $("#grayout").hide();
        });
        // Update the marker on the maps with the new results
        MarkerUpdateFromResults();
    });
}