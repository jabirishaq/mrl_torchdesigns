(function ($) {
    $(function () {
        var jcarousel = $('.relatedproductmycarousel');

        jcarousel
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel.innerWidth();

                $('.relatedproductjcarousel-control-prev').unbind();
                $('.relatedproductjcarousel-control-next').unbind();

                if (width >= 520) {
                    width = width / 3;
                    $('.relatedproductjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=3');
                        return false;
                    });
                    $('.relatedproductjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=3');
                        return false;
                    });
                } else if (width >= 390) {
                    width = width / 2;
                    $('.relatedproductjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=2');
                        return false;
                    });
                    $('.relatedproductjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=2');
                        return false;
                    });
                }
                else if (width >= 260) {
                    width = width / 1;
                    $('.relatedproductjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=1');
                        return false;
                    });
                    $('.relatedproductjcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=1');
                        return false;
                    });
                }
                else if (width >= 100) {
                    width = width / 1;
                    $('.relatedproductjcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=1');
                        return false;
                    });
                    $('.relatedproductjcarousel-control-next').click(function () {
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
