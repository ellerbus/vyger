(function ()
{
    $.growl = function (message, options)
    {
        options = $.extend({}, $.growl.default_options, options);

        var $alert = $('<div class="bootstrap-growl alert" />');

        if (options.type)
        {
            $alert.addClass('alert-' + options.type);
        }

        if (options.allow_dismiss)
        {
            var dismiss = '<button class="close" data-dismiss="alert" type="button"><span aria-hidden="true">&times;</span></button>';

            $alert.addClass('alert-dismissible').append(dismiss);
        }

        $alert.append(message);

        if (options.top_offset)
        {
            options.offset = {
                from: 'top',
                amount: options.top_offset
            };
        }

        var offsetAmount = options.offset.amount;

        $('.bootstrap-growl').each(function ()
        {
            var previous = parseInt($(this).css(options.offset.from)) + $(this).outerHeight() + options.stackup_spacing;

            return offsetAmount = Math.max(offsetAmount, previous);
        });

        var css = {
            'position': (options.ele === 'body' ? 'fixed' : 'absolute'),
            'margin': 0,
            'z-index': '9999',
            'display': 'none'
        };

        css[options.offset.from] = offsetAmount + 'px';

        $alert.css(css);

        if (options.width !== 'auto')
        {
            $alert.css('width', options.width + 'px');
        }

        $(options.ele).append($alert);

        switch (options.align)
        {
            case 'center':
                $alert.css({
                    'left': '50%',
                    'margin-left': '-' + ($alert.outerWidth() / 2) + 'px'
                });
                break;
            case 'left':
                $alert.css('left', '20px');
                break;
            default:
                $alert.css('right', '20px');
        }

        $alert.fadeIn();

        if (options.delay > 0)
        {
            $alert.delay(options.delay).fadeOut(function ()
            {
                return $(this).alert('close');
            });
        }

        return $alert;
    };

    $.growl.primary = function (message)
    {
        $.growl(message, { type: 'primary' });
    }

    $.growl.secondary = function (message)
    {
        $.growl(message, { type: 'secondary' });
    }

    $.growl.success = function (message)
    {
        $.growl(message, { type: 'success' });
    }

    $.growl.danger = function (message)
    {
        $.growl(message, { type: 'danger' });
    }

    $.growl.warning = function (message)
    {
        $.growl(message, { type: 'warning' });
    }

    $.growl.info = function (message)
    {
        $.growl(message, { type: 'info' });
    }

    $.growl.light = function (message)
    {
        $.growl(message, { type: 'light' });
    }

    $.growl.dark = function (message)
    {
        $.growl(message, { type: 'dark' });
    }

    $.growl.default_options = {
        ele: 'body',
        type: 'info',
        offset: {
            from: 'top',
            amount: 20
        },
        align: 'right',
        width: 400,
        delay: 4000,
        allow_dismiss: true,
        stackup_spacing: 10
    };
})();