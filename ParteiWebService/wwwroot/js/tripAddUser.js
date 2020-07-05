

function handleDragStart(e) {
    // this / e.target is the source node.

    // Set the source row opacity
    this.style.opacity = '0.4';

    // Keep track globally of the source row and source table id
    dragSrcRow = this;
    srcTable = this.parentNode.parentNode.id

    // Keep track globally of selected rows
    selectedRows = $('#' + srcTable).DataTable().rows({ selected: true });
    console.log("drag start");

    // Allow moves
    e.dataTransfer.effectAllowed = 'move';
    test = e.target;
    // Save the source row html as text
    e.dataTransfer.setData('text/plain', e.target.outerHTML);

}

function handleDragOver(e) {
    if (e.preventDefault) {
        e.preventDefault(); // Necessary. Allows us to drop.
    }
    //console.log("drag over");
    // Allow moves
    e.dataTransfer.dropEffect = 'move';

    return false;
}

function handleDragEnter(e) {
    // this / e.target is the current hover target.
    // Get current table id
    var currentTable = this.parentNode.parentNode.id

    // Don't show drop zone if in source table
    if (currentTable !== srcTable) {
        this.classList.add('over');
    }
}

function handleDragLeave(e) {
    // this / e.target is previous target element.
    //console.log("Drag Leave");
    // Remove the drop zone when leaving element
    this.classList.remove('over');
}

function handleDrop(e) {
    // this / e.target is current target element.
    if (e.stopPropagation) {
        e.stopPropagation(); // stops the browser from redirecting.
    }

    // Get destination table id, row
    var dstTable = $(this.closest('table')).attr('id');


    var srcColLength = document.getElementById(srcTable).rows[0].cells.length;

    // No need to process if src and dst table are the same
    if (srcTable !== dstTable) {

        // If selected rows and dragged item is selected then move selected rows
        if (selectedRows.count() > 0 && $(dragSrcRow).hasClass('selected')) {
            console.log("kleiner/gleich null" + selectedRows.count());
            // Add row to destination Datatable
            $('#' + dstTable).DataTable().rows.add(selectedRows.data()).draw();

            // Remove row from source Datatable
            $('#' + srcTable).DataTable().rows(selectedRows.indexes()).remove().draw();

        } else {  // Otherwise move dragged row
            console.log(selectedRows.count());
            // Get source transfer data
            var srcData = e.dataTransfer.getData('text/plain');

            var travelid = document.getElementById('Travel_TravelId').value;
            if (srcColLength == 3) {
                var result = getTravelMemberData(test.id, travelid, 1);
                //var x = test.insertCell(srcColLength);
                //x.innerHTML = "0/20";
                //var x1 = test.insertCell(srcColLength + 1);
                //createSelect(x1,"seletStop",1);
                //x1.innerHTML = "<select id=\"inputState\" class=\"form - control form - control - sm\"></select > ";
                console.log(test)
            }

            if (srcColLength == 5) {
                var result = deleteTravelMemberData(test.id, travelid);
                //test.deleteCell(srcColLength - 1);
                //test.deleteCell(srcColLength - 2);
            }
            srcData = test.outerHTML;

            // Add row to destination Datatable
            //$('#' + dstTable).DataTable().row.add($(srcData)).draw();

            // Remove row from source Datatable
            $('#' + srcTable).DataTable().row(dragSrcRow).remove().draw();
        }

    }
    return false;
}

function handleDragEnd(e) {
    // this/e.target is the source node.
    // Reset the opacity of the source row
    this.style.opacity = '1.0';
}

function setEventListenerFroDragAndDrop(id) {
    $(id).DataTable({
        // data: data2,
        paging: false,
        order: [[1, 'asc']],

        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },

        // Add HTML5 draggable class to each row
        createdRow: function (row, data, dataIndex, cells) {
            $(row).attr('draggable', 'true');
        },

        drawCallback: function () {
            // Add HTML5 draggable event listeners to each row
            rows2 = document.querySelectorAll(id + ' tbody tr');
            [].forEach.call(rows2, function (row) {
                row.addEventListener('dragstart', handleDragStart, false);
                row.addEventListener('dragenter', handleDragEnter, false)
                row.addEventListener('dragover', handleDragOver, false);
                row.addEventListener('dragleave', handleDragLeave, false);
                row.addEventListener('drop', handleDrop, false);
                row.addEventListener('dragend', handleDragEnd, false);
            });
        }
    });
}

function getTravelMemberData(memberID, travelId) {
    return $.ajax({
        type: "GET",
        url: '/TripAddUser/GetTravelMemberData?MemberID=' + memberID + '&TravelId=' + travelId,
        contentType: 'json',
        success: function (result) {
            //console.log(result);
            $('#travelMemberTable').html(result);
            setEventListenerFroDragAndDrop("#dataTableTravelMember");
            updateTravelerCard(travelId);
        }
    });
}

function deleteTravelMemberData(memberID, travelId) {
    //console.log(memberID);
    return $.ajax({
        type: "GET",
        url: '/TripAddUser/DeleteTravelMemberData?MemberID=' + memberID + '&TravelId=' + travelId,
        contentType: 'json',
        success: function (result) {
            console.log("Erster Call haut hin");
            $('#travelMemberTable').html(result);
            setEventListenerFroDragAndDrop("#dataTableTravelMember");
            updateTravelerCard(travelId);
            $.ajax({
                url: '/TripAddUser/GetMemberData?TravelId=' + travelId,
                type: "GET",
                contentType: 'json',
                success: function (resultTwo) {
                    console.log("Zweiter Call haut hin");
                    $('#memberTable').html(resultTwo);
                    setEventListenerFroDragAndDrop('#dataTableMember');
                }
            })
        }
    });
}

function updateStop(memberName, memberID, travelId) {
    var memName = memberName;
    var stop = document.getElementById(memName);
    var stopId = stop.options[stop.selectedIndex].value;           
    return $.ajax({
        type: "GET",
        url: '/TripAddUser/UpdateStop?MemberID=' + memberID + '&TravelId=' + travelId + '&StopId=' + stopId,
        contentType: 'json',
        success: function (result) {
            //console.log(result);
            
        }
    });
}

function updateTravelerCard(travelId) {
    //console.log("updateTravelerCard = " + travelId + " ");
    return $.ajax({
        type: "GET",
        url: '/TripAddUser/UpdateTravelerCard?travelId=' + travelId,
        contentType: 'json',
        success: function (result) {
            //console.log(result);
            $('#travelParticipantsCard').html(result);
        }
    });
}

function updateTravelCost(travelMemberId) {
    //console.log(travelMemberId);
    var checkBox = document.getElementById("checkbox_" + travelMemberId);
    if (checkBox.checked == true) {
        return $.ajax({
            type: "GET",
            url: '/TripAddUser/UpdateTravelCost?TravelMemberId=' + travelMemberId + '&Paid=' + checkBox.checked,
            contentType: 'json',
            success: function (result) {
                //console.log(result);
                updateTravelCostCard();
            }
        });
    } else {
        return $.ajax({
            type: "GET",
            url: '/TripAddUser/UpdateTravelCost?TravelMemberId=' + travelMemberId + '&Paid=' + checkBox.checked,
            contentType: 'json',
            success: function (result) {
                //console.log(result);
                updateTravelCostCard();
            }
        });
    }
    
 
}

function updateTravelCostCard() {
    var travelId = document.getElementById("Travel_TravelId").value;
    //console.log("updateTravelerCard = " + travelId + " ");
    return $.ajax({
        type: "GET",
        url: '/TripAddUser/UpdateTravelCostCard?travelId=' + travelId,
        contentType: 'json',
        success: function (result) {
            //console.log(result);
            $('#travelCostCard').html(result);
        }
    });
}
