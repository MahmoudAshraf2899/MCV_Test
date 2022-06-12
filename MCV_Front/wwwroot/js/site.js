////// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
////// for details on configuring this project to bundle and minify static web assets.

////// Write your JavaScript code.



//////`<td>
//////<button type='button' id='button_1' class='btn btn-warning'>Edit</button>
//////<button type='button' id='button_2' class='btn btn-info'>Details</button>
//////<button type='button' id='button_3' class='btn btn-danger'>Delete</button>
////// </td>
//////`
//////"<td> <button type='button' id='button_1' class='btn btn-warning'>Edit</button> <button type='button' id='button_2' class='btn btn-info'>Info</button>  <button type='button' id='button_3' class='btn btn-danger'>Delete</button></td>"


////document.querySelector('#button_1').addEventListener("click", e => {
////    $.ajax({
////        type: "POST",
////        url: "https://localhost:44362/api/Department",
////        contentType: "text/html;charset=UTF-8",
////        dataType: "json",
////        success: function (result) {
////            alert('ok');
////        },
////        error: function (result) {
////            alert('error');
////        }
////    });
////});

////$(document).ready(function () {
////    $("#button_1").click(function (e) {
////        e.preventDefault();
////        $.ajax({
////            type: "POST",
////            url: "/pages/test/",
////            data: {
////                id: $("#button_1").val(),
////                access_token: $("#access_token").val()
////            },
////            success: function (result) {
////                alert('ok');
////            },
////            error: function (result) {
////                alert('error');
////            }
////        });
////    });

////    $("#button_2").click(function (e) {
////        e.preventDefault();
////        $.ajax({
////            type: "POST",
////            url: "/pages/test/",
////            data: {
////                id: $("#button_2").val(),
////                access_token: $("#access_token").val()
////            },
////            success: function (result) {
////                alert('ok');
////            },
////            error: function (result) {
////                alert('error');
////            }
////        });
////    });
////});