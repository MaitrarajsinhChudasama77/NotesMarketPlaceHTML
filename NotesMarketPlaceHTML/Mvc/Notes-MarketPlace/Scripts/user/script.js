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

$('input:radio').click(function () {
    $("#inputSellPrice").prop("disabled", true);
    if ($(this).hasClass('enable_price')) {
        $("#inputSellPrice").prop("disabled", false);
    }
});
$(':radio[name=flexRadioDefault]').change(function () {
    if ($(this).val() == 'Paid') {

        document.getElementById('attach-for-paid').innerHTML = "Upload Preview Notes"
    }
    else {

        document.getElementById('attach-for-paid').innerHTML = ""
    }
});