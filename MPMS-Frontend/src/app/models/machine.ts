import { MachineStatus } from './enum';

export interface Machine {
  machineId: number;
  machineName: string;
  machineCode: string;
  locationId: number;
  status: MachineStatus;
  lastMaintenanceDate: string | null;
}

export interface MachineRequest {
  machineName: string;
  machineCode: string;
  locationId: number;
  status: MachineStatus;
  lastMaintenanceDate: string | null;
}
