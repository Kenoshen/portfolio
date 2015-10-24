// Thumbnail Slider- by JavaScript Kit (www.javascriptkit.com)
// Date created: Jan 8th, 2012
// Visit JavaScript Kit at http://www.javascriptkit.com/ for full source code

(function($) {


$.fn.imageSlider=function(options){
     var container=$(this);
     function deployCanvas(options,container){
            var auto_scroll=options.auto_scroll;
            var auto_scroll_speed=options.auto_scroll_speed;
            var images_path=options.images_path;
            var video_path=options.video_path;
            var thumbs =options.thumbs;
            var stop_on_click=options.stop_on_click
            var stop_after=options.stop_after; 
            var canvas_height=options.canvas_height;
            var canvas_width=options.canvas_width;
            var complete_cycle_slide=options.complete_cycle_slide;
            var main_image_container=$(document.createElement('div'))
                                        .addClass("main_image_container")
                                        .append($(document.createElement('div')).addClass("main_image_scroller"))
                                        .append($(document.createElement('div')).addClass("description_container"));
            var thumbs_list_container=$(document.createElement('div')).addClass("thumbs_list_container");
            

            container.append(main_image_container)
                     .append(thumbs_list_container)
                     .addClass("slider_container");
            
            
            //resize the canvas to the custom width/height
            container.find(".main_image_scroller")
                .css("height",canvas_height+"px")
                .css("width",canvas_width+"px")
            container.find(".thumbs_list_container").css("height",canvas_height+"px");
            
            var current_thumb=0;
            var current_cycle=1;
            var slide_interval=null;
            var images_number=thumbs.length;
            var thumbfrag=$();
            var mainfrag=$();
            for(var x=0;x<images_number;x++){
                var current_image=images_path+thumbs[x];
                thumbfrag=thumbfrag.add(new newThumb(current_image,video_path,x,container));
                mainfrag=mainfrag.add(new newMain(current_image,video_path,x,container))
            }
            container.find(".thumbs_list_container").append(thumbfrag);
            container.find(".main_image_scroller").append(mainfrag);
            var main_image_scroller=container.find(".main_image_scroller") //cache reference
            var description_container=container.find(".description_container") //cache reference
            var largeimages=main_image_scroller.find('div.main_image') //cache reference
            var thumbs=container.find(".thumbs_list_container").find("div") //cache ref to thumb divs
            var largeimageheight=largeimages.eq(0).outerHeight()

            if(thumbs.length==0)
                container.find(".thumbs_list_container").css('visibility','hidden');
                description_container.hide();
            
            function newThumb(src,vidpath,no,container){
                
                return $(document.createElement('div'))
                    .html($((vidpath == "" ? "" : ("<a href='" + vidpath + "'>")) + "<img src='"+encodeURI(src)+"' width=100 height=60><span class='play'></span>" + (vidpath == "" ? "" : "</a>")))
                    .addClass("thumb")
                    .attr("src","'"+src+"'")
                    .attr("id",'thumb_'+no)
                    .prop("container",container)
                    .mouseover(function(){
                        clearTimeout(slide_interval);
                        thumbs.removeClass('thumb_selected').eq(no).addClass('thumb_selected')
                    main_image_scroller.stop().animate({top:'-'+(largeimageheight*no)},complete_cycle_slide);
                        
                        if(auto_scroll==true)
                            {
                            current_thumb=no
                                current_thumb++;
                                if(current_thumb==images_number)
                                    {
                                        current_thumb=0;
                                        current_cycle++;
                                    }                                               
                                if((current_cycle>stop_after)&&(stop_after!=0))
                                    {
                                        autoscroll=false;
                                        return;
                                    }
                                slide_interval=setTimeout(function(){
                                    thumbs.eq(current_thumb).trigger("mouseover");
                                },auto_scroll_speed);
                            }
                        
                    })
                    .mouseup(function(){
                        if(stop_on_click==true)
                            auto_scroll=false;  
                    }); 
            };
            function newMain(src,vidpath,no,container){
                return $(document.createElement('div'))
                    .append($((vidpath == "" ? "" : ("<a href='" + vidpath + "'>")) + '<div class="image" style="width:100%;height:100%;background:url('+encodeURI(src)+')  center center no-repeat;background-size:contain;"></div>' + (vidpath == "" ? "" : "</a>")))
                    .addClass("main_image")
                    .attr("src",src)
                    .attr("id","main_"+no)
                    .click(function(){
                    })
                    .mouseover(function(){
                    })
                    .mouseout(function(){
                    })
            };  
            
            container.find(".thumbs_list_container").find("div").eq(0).trigger("mouseover");
     }
     
    var options = $.extend( {
      'auto_scroll':false,
      'auto_scroll_speed':2500,
      'video_path':'',
      'images_path':'',//this is the path to the folder added in front of all the pages (if specified);
      'thumbs':[], //The names of the images you want in the slider each separated by a comma
      'stop_on_click':true,//set this to true if you want the aoutoscroll to stop once the user clicks on a thumbnail
            /*this is the number of cycles the autoscroll will run for; if autoscroll is set to false, this is ignored;
             *  if stop_on_click is set to true, this will be ignored after the user clicks a thumbnail
             *  if this is set to 0, the loop will run infinitely*/
      'stop_after':0,
      'complete_cycle_slide':300,//this determines the speed needed to cycle from first image to the last. Cycling from image 1 to image 2 is complete_cycle_slide/[number of images];
      //change these 2 values to set the canvas height and width; the thumbs area will be resized in height automatically, depending on the canvas height;
      'canvas_height':385,
      'canvas_width':410
    }, options);     
                

     return this.each(function(){
        var preloadimages=[]
        for (var i=0, c=options.thumbs.length; i<c; i++){
            preloadimages[i]=new Image()
            preloadimages[i].src=options.thumbs[i]
        }   
        new deployCanvas(options,container);
     });
    
} 
  
  
})(jQuery)