import {
  DefectSeverity,
  DefectType,
  MachineStatus,
  ProductionOrderStatus,
  ProductionPlanStatus,
} from '../models/enum';
import { SelectOption } from '../models/select-option';

export const machineStatusOptions: SelectOption<number>[] = [
  { label: 'Running', value: MachineStatus.Running },
  { label: 'Idle', value: MachineStatus.Idle },
  { label: 'Maintenance', value: MachineStatus.Maintenance },
  { label: 'Out Of Service', value: MachineStatus.OutOfService },
];

export const defectTypeOptions: SelectOption<number>[] = [
  { label: 'Mechanical', value: DefectType.Mechanical },
  { label: 'Electrical', value: DefectType.Electrical },
  { label: 'Software', value: DefectType.Software },
  { label: 'Quality', value: DefectType.Quality },
];

export const defectSeverityOptions: SelectOption<number>[] = [
  { label: 'Low', value: DefectSeverity.Low },
  { label: 'Medium', value: DefectSeverity.Medium },
  { label: 'High', value: DefectSeverity.High },
  { label: 'Critical', value: DefectSeverity.Critical },
];

export const productionStatusOptions: SelectOption<number>[] = [
  { label: 'Planned', value: ProductionOrderStatus.Planned },
  { label: 'In Progress', value: ProductionOrderStatus.InProgress },
  { label: 'Completed', value: ProductionOrderStatus.Completed },
  { label: 'On Hold', value: ProductionOrderStatus.OnHold },
  { label: 'Cancelled', value: ProductionOrderStatus.Cancelled },
];

export const productionPlanStatusOptions: SelectOption<number>[] = [
  { label: 'Planned', value: ProductionPlanStatus.Planned },
  { label: 'In Progress', value: ProductionPlanStatus.InProgress },
  { label: 'Completed', value: ProductionPlanStatus.Completed },
  { label: 'On Hold', value: ProductionPlanStatus.OnHold },
  { label: 'Cancelled', value: ProductionPlanStatus.Cancelled },
];

export const productStatusOptions: SelectOption<string>[] = [
  { label: 'Active', value: 'Active' },
  { label: 'Discontinued', value: 'Discontinued' },
  { label: 'Out Of Stock', value: 'OutOfStock' },
];
