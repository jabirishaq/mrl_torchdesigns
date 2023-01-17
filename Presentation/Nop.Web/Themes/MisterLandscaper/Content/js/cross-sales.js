(function ($) {
    $(function () {
        var jcarousel = $('.crosssalescorousel');

        jcarousel
            .on('jcarousel:reload jcarousel:create', function () {
                var width = jcarousel.innerWidth();

                $('crosssalescorouseljcarousel-control-prev').unbind();
                $('.crosssalescorouseljcarousel-control-next').unbind();
                if (width >= 768) {
                    width = width / 4;
                    $('.crosssalescorouseljcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=4');
                        return false;
                    });
                    $('.crosssalescorouseljcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=4');
                        return false;
                    });
                } else if (width >= 650) {
                    width = width / 4;
                    $('.crosssalescorouseljcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=4');
                        return false;
                    });
                    $('.crosssalescorouseljcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=4');
                        return false;
                    });
                }
                else if (width >= 520) {
                    width = width / 3;
                    $('.crosssalescorouseljcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=4');
                        return false;
                    });
                    $('.crosssalescorouseljcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=4');
                        return false;
                    });
                } else if (width >= 390) {
                    width = width / 2;
                    $('.crosssalescorouseljcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=2');
                        return false;
                    });
                    $('.crosssalescorouseljcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=2');
                        return false;
                    });
                }
                else if (width >= 260) {
                    width = width / 2;
                    $('.crosssalescorouseljcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=2');
                        return false;
                    });
                    $('.crosssalescorouseljcarousel-control-next').click(function () {
                        jcarousel.jcarousel('scroll', '+=2');
                        return false;
                    });
                }
                else if (width >= 100) {
                    width = width / 1;
                    $('.crosssalescorouseljcarousel-control-prev').click(function () {
                        jcarousel.jcarousel('scroll', '-=1');
                        return false;
                    });
                    $('.crosssalescorouseljcarousel-control-next').click(function () {
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

