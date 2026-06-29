export interface MaintenanceLog {
  logId: number;
  machineId: number;
  engineerId: number;
  issueDescription: string;
  resolution: string;
  maintenanceDate: string;
}

export interface MaintenanceLogRequest {
  machineId: number;
  issueDescription: string;
  resolution: string;
}
