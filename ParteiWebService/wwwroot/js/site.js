// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var dragSrcRow = null;  // Keep track of the source row
    var selectedRows = null;   // Keep track of selected rows in the source table
    var srcTable = '';
    var test = '';// Global tracking of table being dragged for 'over' class setting
    var rows = [];   // Global rows for #example
    var rows2 = [];  // Global rows for #example2


    setEventListenerFroDragAndDrop("#dataTableMember");
    setEventListenerFroDragAndDrop("#dataTableTravelMember");
  

    console.log("Onready")
});