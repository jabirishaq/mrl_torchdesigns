(function ($) {
    $(function () {
        var jcarousel = $('.Alsocarousel');

        jcarousel
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel.innerWidth();

                $('.Alsojcarousel-control-prev').unbind();
                $('.Alsojcarousel-control-next').unbind();

                if (width >= 550)
                {
                    width = width / 3;
                    $('.Alsojcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=3');
                        return false;
                    });
                    $('.Alsojcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=3');
                        return false;
                    });
                }
                else if (width >= 350)
                {
                    width = width / 2;
                    $('.Alsojcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=2');
                        return false;
                    });
                    $('.Alsojcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=2');
                        return false;
                    });
                }
                else {
                    width = width / 1;
                    $('.Alsojcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=1');
                        return false;
                    });
                    $('.Alsojcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=1');
                        return false;
                    });
                }

                jcarousel.jcarousel('items').css('width', width + 'px');
            })
            .jcarousel({
                wrap: 'circular'
            });


    });
})(jQuery);

