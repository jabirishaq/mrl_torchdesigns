jQuery(document).ready(function(){
    var scripts = document.getElementsByTagName("script");
    var jsFolder = "";
    for (var i= 0; i< scripts.length; i++)
    {
        if( scripts[i].src && scripts[i].src.match(/initcarousel-1\.js/i))
            jsFolder = scripts[i].src.substr(0, scripts[i].src.lastIndexOf("/") + 1);
    }
    if ( typeof html5Lightbox === "undefined" )
    {
        html5Lightbox = jQuery(".html5lightbox").html5lightbox({
            skinsfoldername:"",
            jsfolder:jsFolder,
            barheight:64,
            showtitle:false,
            showdescription:true,
            shownavigation:false,
            thumbwidth:80,
            thumbheight:80,
            thumbtopmargin:8,
            thumbbottommargin:8,
            titlebottomcss:'{color:#333; font-size:14px; font-family:Armata,sans-serif,Arial; overflow:hidden; text-align:left;}',
            descriptionbottomcss:'{color:#333; font-size:12px; font-family:Arial,Helvetica,sans-serif; overflow:hidden; text-align:left; margin:4px 0px 0px; padding: 0px;}'
        });
    }
    jQuery("#amazingcarousel-1").amazingcarousel({
        jsfolder:jsFolder,
        width:140,
        height:200,
        skinsfoldername:"",
        arrowhideonmouseleave:1000,
        itembottomshadowimagetop:100,
        donotcrop:true,
        navheight:24,
        random:false,
        showhoveroverlay:false,
        height:200,
        arrowheight:32,
        itembackgroundimagewidth:100,
        skin:"Stylish",
        responsive:true,
        bottomshadowimage:"bottomshadow-110-100-5.png",
        navstyle:"none",
        enabletouchswipe:false,
        backgroundimagetop:-40,
        arrowstyle:"always",
        bottomshadowimagetop:100,
        transitionduration:1000,
        lightboxshowtitle:false,
        hoveroverlayimage:"hoveroverlay-64-64-4.png",
        itembottomshadowimage:"itembottomshadow-100-100-5.png",
        lightboxshowdescription:true,
        width:140,
        showitembottomshadow:false,
        showhoveroverlayalways:false,
        navimage:"bullet-24-24-0.png",
        lightboxtitlebottomcss:"{color:#333; font-size:14px; font-family:Armata,sans-serif,Arial; overflow:hidden; text-align:left;}",
        lightboxshownavigation:false,
        showitembackgroundimage:false,
        itembackgroundimage:"",
        backgroundimagewidth:110,
        playvideoimagepos:"center",
        circular:true,
        arrowimage:"arrows-32-32-0.png",
        scrollitems:1,
        showbottomshadow:false,
        lightboxdescriptionbottomcss:"{color:#333; font-size:12px; font-family:Arial,Helvetica,sans-serif; overflow:hidden; text-align:left; margin:4px 0px 0px; padding: 0px;}",
        supportiframe:false,
        transitioneasing:"easeOutExpo",
        itembackgroundimagetop:0,
        showbackgroundimage:false,
        lightboxbarheight:64,
        showplayvideo:false,
        spacing:40,
        lightboxthumbwidth:80,
        scrollmode:"page",
        navdirection:"horizontal",
        itembottomshadowimagewidth:100,
        backgroundimage:"",
        lightboxthumbtopmargin:8,
        arrowwidth:32,
        transparent:true,
        navmode:"page",
        lightboxthumbbottommargin:8,
        interval:0,
        lightboxthumbheight:80,
        navspacing:4,
        pauseonmouseover:false,
        imagefillcolor:"FFFFFF",
        playvideoimage:"playvideo-64-64-0.png",
        visibleitems:6,
        navswitchonmouseover:false,
        direction:"horizontal",
        usescreenquery:false,
        bottomshadowimagewidth:110,
        screenquery:{
	mobile: {
		screenwidth: 600,
		visibleitems: 1
	}
},
        navwidth:24,
        loop:0,
        autoplay:false
    });
});