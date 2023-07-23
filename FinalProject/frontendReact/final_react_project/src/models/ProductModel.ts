import DateTimeFormat = Intl.DateTimeFormat;

export default class ProductModel {

    id: string;
    orderId: string;
    name: string;
    isOnSale: boolean;
    price: number;
    salePrice: number;
    createdOn: DateTimeFormat
    picture: string;

    constructor(id: string, orderId: string, name: string, isOnSale: boolean, price: number, salePrice: number, createdOn: Intl.DateTimeFormat, picture: string) {
        this.id = id;
        this.orderId = orderId;
        this.name = name;
        this.isOnSale = isOnSale;
        this.price = price;
        this.salePrice = salePrice;
        this.createdOn = createdOn;
        this.picture = picture;
    }
}
