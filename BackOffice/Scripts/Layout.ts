class Layout implements IPageManager {

    private nowRessize: boolean = false;
    public ui: IUi;
    private topOffset: number = 0;


    private calcHeight(data: string): number {

        var datas = data.split(";");
        var result = 0;

        for (var i in datas) {
            var itm = datas[i];
            result += itm[0] === "#" ? $(itm).innerHeight() : parseInt(itm);
        }
        return result;
    }

    public ressizeDependency(c: { width: number; height: number }) {


        $('.headerIcon').each((indx, element) => {
            $(element).css('left', (c.width - 66) + 'px');
        });

        //      var headerH = $('#appHeader').outerHeight();

        $("[data-fullscreen]").each((indx, element) => {
            $(element).css('height', (c.height) + 'px');
        });


        $("[data-autoheight]").each((indx, element) => {

            var ahData = $(element).attr("data-autoheight");

            var oh: number = (ahData.length > 0 && ahData[0] === '#')
                ? -this.calcHeight(ahData)
                : parseInt(ahData);

            $(element).css('height', ((c.height) + oh) + 'px');

        });

        $("[data-top]").each((indx, element) => {
            var ow = $(element).attr("data-top");
            var h = $(ow).innerHeight();
            $(element).css("top", h + "px");
        });


        $("[data-autowidth]").each((indx, element) => {
            var aw = $(element).attr("data-autowidth");

            if (aw.length > 0 && aw[0] == '*') {
                var mul = parseInt(aw.substr(1, aw.length - 1));
                $(element).css('width', ((c.width) * mul) + 'px');
            }
            else
                $(element).css('width', (c.width + parseInt(aw)) + 'px');
        });

        $("[data-doublewidth]").each((indx, element) => {
            var ow = new Number($(element).attr("data-doublewidth"));
            $(element).css('width', (c.width * <number>ow) + 'px');
        });

        $('[data-maxheight]').each((indx, element) => {
            var ow = new Number($(element).attr("data-maxheight"));
            $(element).css('max-height', (c.height + <number>ow) + 'px');
        });

        $('[data-xcenter]').each((indx, element) => {
            var ow: number = <number>new Number($(element).attr("data-xcenter"));
            var w: number = <number>$(element).width();

            var left = c.width / 2 - w / 2 + ow;

            $(element).css('left', left + 'px');
        });

        $('[data-ycenter]').each((indx, element) => {
            var ow = <number>new Number($(element).attr("data-xcenter"));
            var top = <number>c.height / 2 - $(element).height() / 2 + ow;
            $(element).css('top', top + 'px');
        });

        $('[data-autotop]').each((indx, element) => {
            var ow = <number>new Number($(element).attr("data-autotop"));
            var top = c.height + ow;
            $(element).css('top', top + 'px');
        });

        $('[data-autoright]').each((indx, element) => {
            var ow = <number>new Number($(element).attr("data-autoright"));
            var left = c.width - $(element).width() + ow;
            $(element).css('left', left + 'px');
        });

        $('[data-autobottom]').each((indx, element) => {
            var ow = <number>new Number($(element).attr("data-autobottom"));
            var top = c.height - $(element).height() + ow;
            $(element).css('top', top + 'px');
        });


    }


    ressize():void {
        if (this.nowRessize)
            return;

        this.nowRessize = true;

        var w = window.innerWidth;
        var h = window.innerHeight - this.topOffset;
        var t = w - 1920;
        var l = h - 1200;

        $('#backgrnd').css("background-position", t + "px " + l + "px");

        var o = { left: 0, top: this.topOffset, width: w, height: h };

        $('#htmlcontent').css(o);
        this.ressizeDependency({ width: w, height: h });
        this.nowRessize = false;


        var nowContent = $('#nowContent');

        if (nowContent.length > 0) {
            var nh = nowContent.height();
            console.log("nh=" + nh + "; h=" + h);
        }

        this.ui.ressize();
    }

}


var layout = new Layout();