export interface ProductionRecord {
  id: number;
  productionPlanId: number;
  producedQuantity: number;
  productionDate: string;
}

export interface ProductionRecordRequest {
  productionPlanId: number;
  producedQuantity: number;
}
