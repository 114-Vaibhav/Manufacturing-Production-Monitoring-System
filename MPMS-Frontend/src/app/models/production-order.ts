import { ProductionOrderStatus } from './enum';

export interface ProductionOrder {
  orderId: number;
  planId: number;
  machineId: number;
  quantity: number;
  producedQuantity: number;
  status: ProductionOrderStatus;
}

export interface ProductionOrderRequest {
  planId: number;
  machineId: number;
  quantity: number;
  producedQuantity: number;
  status: ProductionOrderStatus;
}
