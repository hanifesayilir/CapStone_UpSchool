
export default class OrderModel {

    id: string;
    requestedAll: boolean;
    requestedQuantity: number;
    actualQuantity: number;
    productCrawlType: number;
    orderNumber: number;


    constructor(id: string, requestedAll: boolean, requestedQuantity: number, actualQuantity: number, productCrawlType: number, orderNumber: number) {
        this.id = id;
        this.requestedAll = requestedAll;
        this.requestedQuantity = requestedQuantity;
        this.actualQuantity = actualQuantity;
        this.productCrawlType = productCrawlType;
        this.orderNumber = orderNumber;
    }
}
