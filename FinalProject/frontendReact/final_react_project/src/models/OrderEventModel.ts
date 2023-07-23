import DateTimeFormat = Intl.DateTimeFormat;

export default class OrderEventModel {
id: string;
orderId: string;
status: string;
requestedAll : boolean;
requestedQuantity: number;
actualQuantity: number;
createdOn : DateTimeFormat;


    constructor(id: string, orderId: string, status: string, requestedAll: boolean, requestedQuantity: number, actualQuantity: number, createdOn: Intl.DateTimeFormat) {
        this.id = id;
        this.orderId = orderId;
        this.status = status;
        this.requestedAll = requestedAll;
        this.requestedQuantity = requestedQuantity;
        this.actualQuantity = actualQuantity;
        this.createdOn = createdOn;
    }
}
