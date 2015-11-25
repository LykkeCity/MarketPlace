/// <reference path="typings/signalr/signalr.d.ts" />
/// <reference path="utils.ts" />

interface ILkHubProxy {
    client: ILkHubClient;
    server: ILkHubServer;
}

interface IMarketOrder {
    curFrom: string;
    curTo: string;
    volume: string;
    action:string;
}

    
interface ILimitOrderModel extends IMarketOrder {
    price:number;
}

interface ILimitOrder2Model extends IMarketOrder {
    bid: string;
    ask: string;
}

interface ILkHubServer {
    refreshOrderBooks(currency:string);
    doMarketOrder(data: IMarketOrder);
    doLimitOrder(data: ILimitOrderModel);
    doLimitOrder2(data: ILimitOrder2Model);
}

interface IPriceModel {
    id: string;
    bid: number;
    ask: number;
}

interface IBalanceModel {
    cur: string;
    val:string;
}

interface IMessageModel {
    divId: string;
    message:string;
}

interface IAssetModel {
    id: string;
    b: string;
    q: string;
    acc: number;
}

interface ILimitOrderBookDataModel {
    r: number;
    b: number;
    a: number;

}

interface IOrderBookModel {
    asset:string;
    data?: ILimitOrderBookDataModel[];
}


interface ILkHubClient {
    assets(assets: IAssetModel[]);
    price(data: IPriceModel);

    refreshBalance(data: IBalanceModel[]);
    message(data: IMessageModel);
    orderBooks(data: IOrderBookModel[]);
}



class LkMarket {

    constructor(private hub: ILkHubProxy) {
        hub.client.price = data => this.price(data);
        hub.client.refreshBalance = data => this.refreshBalance(data);
        hub.client.message = data => this.message(data);
        hub.client.assets = data => this.assetsRecieved(data);
        hub.client.orderBooks = data => this.orderBooks(data);
    }

    private currentCurrency:string;


    setCurrentCurrency(currency: string) {
        this.currentCurrency = currency;
        this.hub.server.refreshOrderBooks(currency);
    }

    private assets: IAssetModel[];

    private renderOrderBooks(data: IOrderBookModel) {
        var jq = $('#orderBook' + data.asset);
        if (jq.length === 0)
            return;

        var asset = this.findAsset(data.asset);
        var html = '<table class="table table-striped" style="font-size: 10px;"><tr><th style="text-align:center;padding: 2px;">BID</th><th style="text-align:center;padding: 2px;">Rate</th><th style="text-align:center;padding: 2px;">ASK</th></tr>';

        if (data.data != undefined)
        for (var i = 0; i < data.data.length; i++) {
                var itm = data.data[i];

                html += '<tr><td style="padding: 2px;">'+(itm.b == undefined ? '' : itm.b)+'</td>' +
                      '<td style="padding: 2px;">' + Utils.roundFloat(itm.r, asset.acc) + '</td>' +
                '<td style="padding: 2px;">' + (itm.a == undefined ? '' : itm.a) +'</td></tr>';
        }


        jq.html(html + '</table>');
    }


    private orderBooks(data: IOrderBookModel[]) {
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
                this.renderOrderBooks({ asset: asset});

        }
    }

    private assetsRecieved(assets: IAssetModel[]) {
        this.assets = assets;
    }

    private findAsset(id: string): IAssetModel {
        for (let i = 0; i < this.assets.length; i++) {
            if (this.assets[i].id === id)
                return this.assets[i];
        }

        return undefined;
    }

    private message(data: IMessageModel) {
        ui.showError(data.divId, data.message);
    }

    private price(data: IPriceModel) {

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
    }

    private refreshBalance(data: IBalanceModel[]) {
        for (var i = 0; i < data.length; i++) {
            var b = data[i];
            $('#amount' + b.cur).html(b.val);
        }
    }

    start():void {
        $.connection.hub.start().done(() => {
           

        }).fail(result=> {

        });

        $.connection.hub.disconnected(() => {

        }); 
    }

    marketOrder(cur: string, action: string) {
        var volume = $('#volume' + cur).val();
        this.hub.server.doMarketOrder({ action: action, curFrom:this.currentCurrency, curTo:cur, volume: volume });
    }

    limitOrder(cur: string, action?: string) {
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

    }

}


