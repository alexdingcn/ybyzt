//首页5图滚动
(function($) {

    $.fn.extend({

        jsCarousel: function(options) {

            var settings = $.extend({

                scrollspeed: 700,

                delay: 8000,

                itemstodisplay: 5,

                autoscroll: false,

                onthumbnailclick: null

            }, options);

            return this.each(function() {

            var slidercontents = $(this).addClass('jscarousal-contents');

                var slider = $('<div/>').addClass('jscarousal');

                var leftbutton = $('<div/>').addClass('jscarousal-left');

                var rightbutton = $('<div/>').addClass('jscarousal-right');

                slidercontents.before(slider);

                slider.append(leftbutton);

                slider.append(slidercontents);

                slider.append(rightbutton);



                var total = $('> div', slidercontents).css('display', 'none').length;

                var index = 0;

                var start = 0;

                var current = $('<div/>');

                var noOfBlocks;

                var interval;

                var left;

                var display = settings.itemstodisplay;

                var speed = settings.scrollspeed;

                var containerWidth;

                var height;

                var direction = "forward";



                function initialize() {

                    index = -1;

                    noOfBlocks = parseInt(total / display);

                    if (total % display > 0) noOfBlocks++;

                    var startIndex = 0;

                    var endIndex = display;

                    var copy = false;

                    var allElements = $('> div', slidercontents);

                    $('> div', slidercontents).remove();

                    allElements.addClass('thumbnail-inactive').hover(function() { $(this).removeClass('thumbnail-inactive').addClass('thumbnail-active'); }, function() { $(this).removeClass('thumbnail-active').addClass('thumbnail-inactive'); })

                    for (var i = 0; i < noOfBlocks; i++) {

                        if (total > display) {

                            startIndex = i * display;

                            endIndex = startIndex + display;

                            if (endIndex > total) {

                                startIndex -= (endIndex - total);

                                endIndex = startIndex + display;

                                copy = true;

                            }

                        }

                        else {

                            startIndex = 0;

                            endIndex = total;

                        }

                        var wrapper = $('<div/>')

                        allElements.slice(startIndex, endIndex).each(function(index, el) {

                            if (!copy)

                                wrapper.append(el);

                            else wrapper.append($(el).clone(true));



                        });

                        wrapper.find("img").click(

                         function() {

                             if (settings.onthumbnailclick != null) settings.onthumbnailclick($(this).attr('src'));

                         });

                        slidercontents.append(wrapper);

                    }

                    $('> div', slidercontents).addClass('hidden');

                    $('> div > div', slidercontents).css('display', '');

                    right = $('> div:eq(' + index + ')', slidercontents).css('right');



                    containerWidth = slidercontents.width();

                    height = slidercontents.get(0).offsetHeight;

                    $('> div', slidercontents).css('right', '-' + containerWidth + 'px');

                    $('> div:eq(0)', slidercontents).addClass('visible').removeClass('hidden');

                    $('> div:eq(0)', slidercontents).stop().animate({ right: 0 }, speed, function() { index += 1; });

                    slider.mouseenter(function() {  if (settings.autoscroll) stopAnimate(); }).mouseleave(function() { if (settings.autoscroll) animate(); });

                    if (settings.autoscroll)

                        animate();



                    rightbutton.click(function() {

                        direction = "forward";

                        showThumbs();

                    });

                    leftbutton.click(function() {

                        direction = "backward";

                        showThumbs();

                    });

                }

                initialize();

                function stopAnimate() {



                    clearTimeout(interval);

                    slider.children().clearQueue();

                    slider.children().stop();

                }

                function animate() {

                    clearTimeout(interval);

                    if (settings.autoscroll)

                        interval = setTimeout(changeSlide, settings.delay);

                }

                function changeSlide() {

                    if (direction == "forward") {

                        if (index >= noOfBlocks - 1) { index = -1; }

                    } else {

                        if (index <= 0) index = noOfBlocks - 1;

                    }

                    showThumbs();

                    interval = setTimeout(changeSlide, settings.delay);

                }

                function getDimensions(value) {

                    return value + 'px';

                }



                function showThumbs() {

                    var current = $('.visible');

                    var scrollSpeed = speed;



                    if (direction == "forward") {

                        index++;

                        if (index < noOfBlocks) {

                            $('>div:eq(' + index + ')', slidercontents).removeClass('hidden').addClass('visible').css({

                                'right': getDimensions(-containerWidth)

                            }).stop().animate({ 'right': '+=' + getDimensions(containerWidth) }, scrollSpeed);



                            current.stop().animate({ 'right': '+=' + getDimensions(containerWidth) }, scrollSpeed,

                            function() {

                                $(this).removeClass('visible').addClass('hidden');

                                $(this).css('right', getDimensions(-containerWidth));

                            });



                        } else index = noOfBlocks - 1;

                    }

                    else if (direction == "backward") {

                        index--;

                        if (index >= 0) {

                            $('>div:eq(' + index + ')', slidercontents).css('right', getDimensions(containerWidth)).removeClass('hidden').addClass('visible').stop().animate({ 'right': '-=' + getDimensions(containerWidth) }, scrollSpeed);

                            current.stop().animate({ 'right': '-=' + getDimensions(containerWidth) }, scrollSpeed, function() {

                                $(this).removeClass('visible').addClass('hidden');

                                $(this).css('left', left);

                            });

                        } else index = 0;

                    }

                }

            });

        }

    });

})(jQuery);

