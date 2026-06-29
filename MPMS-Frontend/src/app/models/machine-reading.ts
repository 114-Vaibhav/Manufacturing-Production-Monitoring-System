export interface MachineReading {
  readingId: number;
  machineId: number;
  temperature: number;
  vibration: number;
  powerConsumption: number;
  timestamp: string;
}

export interface MachineReadingRequest {
  machineId: number;
  temperature: number;
  vibration: number;
  powerConsumption: number;
}
