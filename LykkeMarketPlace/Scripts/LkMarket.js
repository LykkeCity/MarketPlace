/// <reference path="typings/signalr/signalr.d.ts" />
/// <reference path="utils.ts" />
var LkMarket = (function () {
    function LkMarket(hub) {
        var _this = this;
        this.hub = hub;
        hub.client.price = function (data) { return _this.price(data); };
        hub.client.refreshBalance = function (data) { return _this.refreshBalance(data); };
        hub.client.message = function (data) { return _this.message(data); };
        hub.client.assets = function (data) { return _this.assetsRecieved(data); };
        hub.client.orderBooks = function (data) { return _this.orderBooks(data); };
    }
    LkMarket.prototype.setCurrentCurrency = function (currency) {
        this.currentCurrency = currency;
        this.hub.server.refreshOrderBooks(currency);
    };
    LkMarket.prototype.renderOrderBooks = function (data) {
        var jq = $('#orderBook' + data.asset);
        if (jq.length === 0)
            return;
        var asset = this.findAsset(data.asset);
        var html = '<table class="table table-striped" style="font-size: 10px;"><tr><th style="text-align:center;padding: 2px;">BID</th><th style="text-align:center;padding: 2px;">Rate</th><th style="text-align:center;padding: 2px;">ASK</th></tr>';
        if (data.data != undefined)
            for (var i = 0; i < data.data.length; i++) {
                var itm = data.data[i];
                html += '<tr><td style="padding: 2px;">' + (itm.b == undefined ? '' : itm.b) + '</td>' +
                    '<td style="padding: 2px;">' + Utils.roundFloat(itm.r, asset.acc) + '</td>' +
                    '<td style="padding: 2px;">' + (itm.a == undefined ? '' : itm.a) + '</td></tr>';
            }
        jq.html(html + '</table>');
    };
    LkMarket.prototype.orderBooks = function (data) {
        console.log(data);
        for (var i = 0; i < this.assets.length; i++) {
            var rendered = false;
            var asset = this.assets[i].id;
            for (var j = 0; j < data.length; j++)
                if (data[j].asset === asset) {
                    this.renderOrderBooks(data[j]);
                    rendered = true;
                    break;
                }
            if (!rendered)
                this.renderOrderBooks({ asset: asset });
        }
    };
    LkMarket.prototype.assetsRecieved = function (assets) {
        this.assets = assets;
    };
    LkMarket.prototype.findAsset = function (id) {
        for (var i = 0; i < this.assets.length; i++) {
            if (this.assets[i].id === id)
                return this.assets[i];
        }
        return undefined;
    };
    LkMarket.prototype.message = function (data) {
        ui.showError(data.divId, data.message);
    };
    LkMarket.prototype.price = function (data) {
        var asset = this.findAsset(data.id);
        if (asset == undefined)
            return;
        //if (asset.b == this.currentCurrency) {
        $('#bid' + data.id).html(Utils.roundFloat(data.bid, asset.acc));
        $('#ask' + data.id).html(Utils.roundFloat(data.ask, asset.acc));
        //} else {
        //    $('#bid' + data.id).html(Utils.roundFloat((1 / data.bid), asset.acc));
        //    $('#ask' + data.id).html(Utils.roundFloat((1 / data.ask), asset.acc));
        //}
    };
    LkMarket.prototype.refreshBalance = function (data) {
        for (var i = 0; i < data.length; i++) {
            var b = data[i];
            $('#amount' + b.cur).html(b.val);
        }
    };
    LkMarket.prototype.start = function () {
        $.connection.hub.start().done(function () {
        }).fail(function (result) {
        });
        $.connection.hub.disconnected(function () {
        });
    };
    LkMarket.prototype.marketOrder = function (cur, action) {
        var volume = $('#volume' + cur).val();
        this.hub.server.doMarketOrder({ action: action, curFrom: this.currentCurrency, curTo: cur, volume: volume });
    };
    LkMarket.prototype.limitOrder = function (cur, action) {
        var volume = $('#volume' + cur).val();
        if (action === "sell") {
            this.hub.server.doLimitOrder({ action: action, curFrom: this.currentCurrency, curTo: cur, price: $('#slr' + cur).val(), volume: volume });
            return;
        }
        if (action === "buy") {
            this.hub.server.doLimitOrder({ action: action, curFrom: this.currentCurrency, curTo: cur, price: $('#blr' + cur).val(), volume: volume });
            return;
        }
        var bid = $('#lobid' + cur).val();
        var ask = $('#loask' + cur).val();
        this.hub.server.doLimitOrder2({ action: action, curFrom: this.currentCurrency, curTo: cur, bid: bid, ask: ask, volume: volume });
    };
    return LkMarket;
})();
//# sourceMappingURL=LkMarket.js.map