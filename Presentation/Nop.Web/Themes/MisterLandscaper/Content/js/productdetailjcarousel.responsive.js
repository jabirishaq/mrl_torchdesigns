(function ($) {
    $(function () {
        var jcarousel = $('.productdetailmycarousel');

        jcarousel
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel.innerWidth();

                $('.productdetailjcarousel-control-prev').unbind();
                $('.productdetailjcarousel-control-next').unbind();

                 if (width >= 520) {
                    width = width / 6;
                    $('.productdetailjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=6');
                        return false;
                    });
                    $('.productdetailjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=6');
                        return false;
                    });
                } else if (width >= 390) {
                    width = width / 4;
                    $('.productdetailjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=4');
                        return false;
                    });
                    $('.productdetailjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=4');
                        return false;
                    });
                }
                else if (width >= 290) {
                    width = width / 3;
                    $('.productdetailjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=3');
                        return false;
                    });
                    $('.productdetailjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=3');
                        return false;
                    });
                }
                else if (width >= 215) {
                    width = width / 2;
                    $('.productdetailjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=2');
                        return false;
                    });
                    $('.productdetailjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=2');
                        return false;
                    });
                }
                else if (width >= 115) {
                    width = width / 1;
                    $('.productdetailjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=1');
                        return false;
                    });
                    $('.productdetailjcarousel-control-next').click(function () {
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
