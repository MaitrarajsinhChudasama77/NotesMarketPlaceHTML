$(".toggle-password").click(function(){
    $(this).toggleClass("fa-eye-slash fa-eye");
    var input=$($(this).attr("toggle"));
    if(input.attr("type")=="password"){
        input.attr("type","text");
    }
    else{
        input.attr("type","password");
    }
});


/* Dropdown Menu for MyDownloads Page */
function mydownloadDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}
/* Dropdown Menu for MySoldNotes Page */
function mysoldnotesDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}
/* Dropdown Menu for MyRejectedNotes Page */
function myrejectednotesDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}
/* Dropdown Menu for BuyerRequests Page */
function mybuyerrequestDropdown(id) {
  document.getElementById("myDropdown_" + id).classList.toggle("show");
}
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
/*--FAQ Page--*/
var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
	acc[i].addEventListener("click", function () {
		this.classList.toggle("active");
		var panel = this.nextElementSibling;
		if (panel.style.maxHeight) {
			panel.style.maxHeight = null;
		} else {
			panel.style.maxHeight = panel.scrollHeight + "px";
		}
	});
}

$("button.accordion").click(
    function(){
        $(".accordion.active").next().css('border-bottom','1px solid #d1d1d1'); 
    }
);
$("button.accordion").click(
    function(){
        $(".accordion.active").next().css('border-bottom','1px solid #f3f3f3'); 
   }
);