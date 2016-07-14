// Dom Ready initializations
$(document).ready(function () {
    // Print all Analytics Results initially 
    UpdateAnalyticsData();

    // Click Event for the Start Button
    $("#collectData").click(function () {
        // Change the appearance of the Button
        $(this).hide();
        $("#stopCollectData").show();
        // Request the server to start
        ExecuteAjaxCall("Analytics/StartDataCollection");
    });

    // Click Event for the Stop Button
    $("#stopCollectData").click(function () {
        // Change the appearance of the button
        $(this).hide();
        $("#collectData").show();
        // If a GUID is set, request the server to stop the data collection
        ExecuteAjaxCall("Analytics/StopDataCollection");
    });
});

// Resie event of the Window
$(window).resize(function () {
    // Graph has to be drawn again on resize too fit again into the grid
    UpdateGraph(petrolStationFilter, daysFilter);
});


// Updates the Analytics Result View
function UpdateAnalyticsData() {
    //Helper to get and save globally all selected values of the filter
    var daysFilter = GetCurrentValueOfFilter("days");
    var petrolStationFilter = GetCurrentValueOfFilter("petrolStation");

    // Build the url with parameters for the call
    var url = "/Analytics/UpdateAnalytics?petrolStationId=" + petrolStationFilter + "&requestDay=" + daysFilter;
    // Execute the call
    ExecutePartialLoading(url, petrolStationFilter, daysFilter);
}

// Helper to get the current Values of the given Filter and save them globally
function GetCurrentValueOfFilter(filter) {
    // Get the Values of the Dropdowns 
    // On the first call, dropdowns have the value "undefined"
    if (filter === "days") {
        daysFilter = $("#chosenDay").find(":selected").attr("value");
        // Map the undefined value on the first call to "All"
        if (daysFilter === undefined) {
            daysFilter = "All";
        }
        return daysFilter;
    }
    else if (filter === "petrolStation") {
        petrolStationFilter = $("#chosenPetrolStationId").find(":selected").attr("value");
        // Map the undefined value on the first call to "All"
        if (petrolStationFilter === undefined) {
            petrolStationFilter = "All";
        }
        return petrolStationFilter;
    }
    return undefined;
}

// Executes a partial Loading with the given url
function ExecutePartialLoading(url, petrolStationFilter, daysFilter) {
    $.get(url, null, function (data) {
        $("#AnalyticsData").html(data);
    }).done(function () {
        // Change Event for the Petrol Statid Id Dropdown
        $("#chosenPetrolStationId").change(function () {
            UpdateAnalyticsData();
        });
        // Change event of day filter dropdown
        $("#chosenDay").change(function () {
            UpdateAnalyticsData();
        });
        // Click event to show a graph of the current results
        $("#ShowGraphButton").click(function () {
            $("#graphContainer").show();
            $(this).hide();
            $("#HideGraphButton").show();
            UpdateGraph(petrolStationFilter, daysFilter);
            // Scroll down to the results
            $("html, body").animate({
                scrollTop: $("#HeadlineAnalyticsResults").offset().top - 50
            }, 1000);
        });
        // Click event to hide the graph again
        $("#HideGraphButton").click(function () {
            $("#ShowGraphButton").show();
            $(this).hide();
            $("#graphContainer").hide();
        });


        $("#graphContainer").hide();

        var daysFilter = GetCurrentValueOfFilter("days");
        var petrolStationFilter = GetCurrentValueOfFilter("petrolStation");
        // Update the Graph with the new Values
        if (daysFilter !== "All" && petrolStationFilter !== "All") {
            $("#ShowGraphButton").show();
        } else {
            $("#ShowGraphButton").hide();
        }
    });
}

// Executes the Ajax Call with a specific url
function ExecuteAjaxCall(url) {
    $.ajax({
        type: "GET",
        url: url,
        context: document.body
    });
}

// Updates the Graph for analytics results
function UpdateGraph(petrolStationFilter, daysFilter) {
    $.ajax({
        type: "POST",
        url: "Analytics/UpdateGraphData",
        data: { petrolStationId: petrolStationFilter, requestDay: daysFilter },
        context: document.body
    }).done(function (data) {

        // Array Initialization
        var x = [];
        var allTwoD = [];
        var twoDs = [];
        var yValueArray = [];
        var yIndex;

        // Go through all given data
        for (var index = 0; index < data.length; index++) {
            // Prepare the given Datetime -> JsonResult gives a not usable dateformat
            var jsonDatetime = data[index]["RequestDatetime"];
            var dateTime = new Date(parseInt(jsonDatetime.substr(6)));
            // Get Minutes delivers only 1 digit on minutes less than 10 which leads to problems in graph
            var minutesPrefix = dateTime.getMinutes() < 10 ? "0" : "";
            // Build a Time Format valid for the graph
            var requestDatetime = dateTime.getHours() + "." + minutesPrefix + dateTime.getMinutes();

            // Get the current values of the data to be displayed with the time and get the label
            yValueArray = [
                ["e5", data[index]["E5"]],
                ["e10", data[index]["E10"]],
                ["Diesel", data[index]["Diesel"]]
            ];

            // On the first run, initialize the twoDs correctly
            if (index === 0) {
                for (var index = 0; index < yValueArray.length; index++) {
                    twoDs.push(parseInt(index));
                    twoDs[parseInt(index)] = [];
                }
            }

            // Get the Values of the x-Axis
            // Distinct on the x Values
            if ($.inArray(requestDatetime, x) !== -1) {
                continue;
            }

            x.push(requestDatetime);

            // Get the Values of the y-Axis
            // Distinct on the y Values based on the results of the x values
            for (yIndex = 0; yIndex < yValueArray.length; yIndex++) {
                twoDs[parseInt(yIndex)].push([requestDatetime, yValueArray[yIndex][1]]);
            }
        }

        // Take all given values of all arrays and put them into one big single array with all information
        for (yIndex = 0; yIndex < yValueArray.length; yIndex++) {
            allTwoD.push({ label: yValueArray[yIndex][0], data: twoDs[yIndex] });
        }


        // Prepare the tooltip on the graph
        $("<div id='tooltip'></div>").css({
            position: "absolute",
            display: "none",
            border: "1px solid #fdd",
            padding: "10px",
            "background-color": "#fee",
            opacity: 0.80
        }).appendTo("body");

        // Prepare the hover effect for the tooltip 
        $("#graph").bind("plothover", function (event, pos, item) {
            if (item) {
                var x = item.datapoint[0].toFixed(2),
                    y = item.datapoint[1].toFixed(2);

                $("#tooltip").html(item.series.label + ": " + x + " Uhr " + y + "€")
                    .css({ top: item.pageY + 5, left: item.pageX + 5 })
                    .fadeIn(200);
            } else {
                $("#tooltip").hide();
            }
        });

        // Plot all Graphs
        $.plot("#graph", allTwoD, {
            series: {
                lines: {
                    show: true
                },
                points: {
                    show: true
                }
            },
            grid: {
                hoverable: true,
                clickable: true
            },
            legend: {
                noColumns: allTwoD.length,
                container: $("#graphLegend")
            }
        });
    });
}