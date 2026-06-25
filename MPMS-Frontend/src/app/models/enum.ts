// enums.ts

export enum MachineStatus {
  Running = 0,
  Idle = 1,
  Maintenance = 2,
  OutOfService = 3
}

export enum ProductionOrderStatus {
  Planned = 0,
  InProgress = 1,
  Completed = 2,
  OnHold = 3,
  Cancelled = 4
}

export enum ShiftType {
  Morning = 0,
  Evening = 1,
  Night = 2
}

export enum DefectSeverity {
  Low = 0,
  Medium = 1,
  High = 2,
  Critical = 3
}

export enum DefectType {
  Mechanical = 0,
  Electrical = 1,
  Software = 2,
  Quality = 3
}

export enum LocationStatus {
  Active = 0,
  Inactive = 1,
  UnderMaintenance = 2
}

export enum ProductStatus {
  Active = 0,
  Discontinued = 1,
  OutOfStock = 2
}

export enum ProductionPlanStatus {
  Planned = 0,
  InProgress = 1,
  Completed = 2,
  OnHold = 3,
  Cancelled = 4
}

export enum UserRole {
  Admin = 0,
  Operator = 1,
  ProductionManager = 2,
  QualityInspector = 3,
  MaintenanceTechnician = 4,
  ProductionPlanner = 5,
  PlantManager = 6
}

export enum UserStatus {
  Active = 0,
  Inactive = 1,
  Suspended = 2
}