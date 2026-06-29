import { ProductionPlanStatus } from './enum';

export interface ProductionPlan {
  planId: number;
  productName: string;
  targetQuantity: number;
  startDate: string;
  endDate: string;
  status: ProductionPlanStatus;
  createdBy: number;
}

export interface ProductionPlanRequest {
  productName: string;
  targetQuantity: number;
  startDate: string;
  endDate: string;
  status: ProductionPlanStatus;
}
