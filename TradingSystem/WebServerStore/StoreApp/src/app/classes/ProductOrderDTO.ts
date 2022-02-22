import { OrderDTO } from "./OrderDTO";

export class ProductOrderDTO {

    productOrderId: number;
    deliveryDate: Date;
    orderingDate: Date;
    orders: OrderDTO[];

    constructor(
        productOrderId: number,
        deliveryDate: Date,
        orderingDate: Date,
        orders: OrderDTO[]) {
            this.productOrderId = productOrderId;
            this.deliveryDate = deliveryDate;
            this.orderingDate = orderingDate;
            this.orders = orders;
    }
}