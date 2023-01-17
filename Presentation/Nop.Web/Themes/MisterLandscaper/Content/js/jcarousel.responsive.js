(function ($) {
    $(function () {
        var jcarousel = $('.mycarousel');

        jcarousel
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel.innerWidth();

                $('.jcarousel-control-prev').unbind();
                $('.jcarousel-control-next').unbind();

                if (width >= 768) {
                    width = width / 6;
                    $('.jcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=6');
                        return false;
                    });
                    $('.jcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=6');
                        return false;
                    });
                } else if (width >= 650) {
                    width = width / 5;
                    $('.jcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=5');
                        return false;
                    });
                    $('.jcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=5');
                        return false;
                    });
                }
                else if (width >= 520) {
                    width = width / 4;
                    $('.jcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=4');
                        return false;
                    });
                    $('.jcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=4');
                        return false;
                    });
                } else if (width >= 390) {
                    width = width / 3;
                    $('.jcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=3');
                        return false;
                    });
                    $('.jcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=3');
                        return false;
                    });
                }
                else if (width >= 260) {
                    width = width / 2;
                    $('.jcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=2');
                        return false;
                    });
                    $('.jcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=2');
                        return false;
                    });
                }
                else if (width >= 100) {
                    width = width / 1;
                    $('.jcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=1');
                        return false;
                    });
                    $('.jcarousel-control-next').click(function () {
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
