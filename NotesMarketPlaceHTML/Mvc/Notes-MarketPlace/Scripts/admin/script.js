$(".toggle-password").click(function(){
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input=$($(this).attr("toggle"));
    if(input.attr("type")=="password"){
        input.attr("type","text");
    }
    else{
        input.attr("type","password");
    }
});
/*--Dropdown Menu for Admin-Members Page--*/
/*function myAdminMembersDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}*/
/*
*//*--Dropdown Menu for Spam-Report Page--*//*
function mySpamReportDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}

*//*--Dropdown Menu for Published-Notes Page--*//*
function myPublishedNotesDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}

*//*--Dropdown Menu for Rejected-Notes Page--*//*
function myRejectedNotesDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}

*//*--Dropdown Menu for Member-Details Page--*//*
function myMemberDetailsDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}

*//*--Dropdown Menu for Download-Notes Page--*//*
function myDownloadedNotesDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}

*//*--Dropdown Menu for Dashboard Page--*//*
function myDashboardDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}*/

// Close the dropdown for outside click
window.onclick = function(event) {
  if (!event.target.matches('.dropbtn')) {
    var dropdowns = document.getElementsByClassName("dropdown-content");
    var i;
    for (i = 0; i < dropdowns.length; i++) {
      var openDropdown = dropdowns[i];
      if (openDropdown.classList.contains('show')) {
        openDropdown.classList.remove('show');
      }
    }
  }
}