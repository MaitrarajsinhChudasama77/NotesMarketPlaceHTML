/* ---------- Home Page show hide Nav ---------- */
$(function(){
    //  Show/hide header on page scroll
    showHideNav();
    $(window).scroll(function(){
        //  Show/hide header on window scroll
        showHideNav();
    });
    function showHideNav(){
        if($(window).scrollTop() > 99){
            $("nav").addClass("white-nav-top");
            $("nav").removeClass("fixed-top");
            $(".logo img").attr("src","image/Dashboard/logo.png");
            $("#home-section #mynav .hamburger-icon").css("color", "#6255a5");
        }
        else{
            $("nav").addClass("fixed-top");
            $("nav").removeClass("white-nav-top");
            $(".logo img").attr("src","image/home/top-logo.png");
            $("#home-section #mynav .hamburger-icon").css("color", "#fff");
        }
    }
});