import { DefectSeverity, DefectType } from './enum';

export interface Defect {
  defectId: number;
  orderId: number;
  machineId: number;
  type: DefectType;
  severity: DefectSeverity;
  description: string;
  reportedBy: number;
  createdAt: string;
}

export interface DefectRequest {
  orderId: number;
  machineId: number;
  type: DefectType;
  severity: DefectSeverity;
  description: string;
}
