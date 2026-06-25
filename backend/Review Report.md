# Review Report

## Backend Overview

The backend is an ASP.NET Core Web API for a Manufacturing Production Monitoring System. It uses JWT authentication, role-based authorization, EF Core repository pattern, service-layer validation, DTO request models, pagination parameters on list APIs, and audit logging for create/update/delete actions.

## Roles And Responsibilities

| Role | Main Responsibility | Can Perform | Cannot Perform |
| --- | --- | --- | --- |
| Admin | Full system control and monitoring | Read all modules, manage machines, products, defects, plans, orders, records, readings, maintenance logs, and view audit logs | No major backend restriction found |
| PlantManager | Plant-level management | Read most modules, create/update many production resources, delete most operational records except audit logs | Cannot view admin audit logs |
| ProductionManager | Production execution control | Read production data, create/update products, plans, orders, records, defects, and machine readings | Cannot delete most records unless also PlantManager/Admin |
| ProductionPlanner | Planning and product visibility | Read machines, products, plans, orders, records, analytics; create/update products and production plans | Cannot delete records, manage machines, or update orders/records unless allowed by another role |
| QualityInspector | Quality checks and defect handling | Read machines, products, defects, records, analytics; create/update defects | Cannot manage machines, products, plans, orders, or delete defects |
| MaintenanceTechnician | Maintenance and machine health | Read machines, readings, maintenance logs, analytics; create/update maintenance logs and update machine readings | Cannot create/delete machines or delete maintenance logs |
| Operator | Shop-floor operations | Read machines, readings, products, orders, records, analytics; create defects and production records | Cannot update/delete most master or planning data |

## Permission Summary

| Module | Read Allowed | Create Allowed | Update Allowed | Delete Allowed |
| --- | --- | --- | --- | --- |
| Machines | All roles | Admin, PlantManager | Admin, PlantManager | Admin |
| Machine Readings | All roles | Disabled in controller | Admin, PlantManager, ProductionManager, MaintenanceTechnician | Admin, PlantManager |
| Maintenance Logs | Admin, PlantManager, MaintenanceTechnician, ProductionManager, QualityInspector, Operator | Admin, PlantManager, MaintenanceTechnician | Admin, PlantManager, MaintenanceTechnician | Admin, PlantManager |
| Defects | Admin, QualityInspector, ProductionManager, PlantManager; single defect also Operator | Admin, QualityInspector, Operator | Admin, QualityInspector, ProductionManager | Admin, PlantManager |
| Products | Admin, Operator, ProductionManager, ProductionPlanner, PlantManager, QualityInspector | Admin, ProductionPlanner, ProductionManager, PlantManager | Admin, ProductionPlanner, ProductionManager, PlantManager | Admin, PlantManager |
| Production Plans | Admin, PlantManager, ProductionManager, ProductionPlanner | Admin, PlantManager, ProductionManager, ProductionPlanner | Admin, PlantManager, ProductionManager, ProductionPlanner | Admin, PlantManager |
| Production Orders | Admin, Operator, ProductionManager, ProductionPlanner, PlantManager | Admin, ProductionManager, PlantManager | Admin, ProductionManager, PlantManager | Admin, PlantManager |
| Production Records | Admin, Operator, ProductionManager, ProductionPlanner, PlantManager, QualityInspector | Admin, Operator, ProductionManager, PlantManager | Admin, ProductionManager, PlantManager | Admin, PlantManager |
| Production Analytics | All roles | Not exposed | Not exposed | Not exposed |
| Admin Logs | Admin | System writes logs automatically | Not exposed | Not exposed |
| Users/Auth | Public registration, login, and user lookup | Public register | Not exposed | Not exposed |

## API Endpoints

| Endpoint | One-line Purpose |
| --- | --- |
| `POST /user-register` | Register a new user and return registration response. |
| `POST /user-login` | Authenticate user and return JWT/login response. |
| `GET /api/Users/{id}` | Get one user by id. |
| `GET /api/Machines` | List machines with pagination. |
| `GET /api/Machines/{id}` | Get one machine by id. |
| `POST /api/Machines` | Create a machine. |
| `GET /api/MachineReadings` | List machine readings with pagination. |
| `GET /api/MachineReadings/{id}` | Get one machine reading by id. |
| `GET /api/MaintenanceLogs` | List maintenance logs with pagination. |
| `GET /api/MaintenanceLogs/{id}` | Get one maintenance log by id. |
| `POST /api/MaintenanceLogs` | Create a maintenance log for the current user. |
| `GET /api/Defects` | List defects with pagination. |
| `GET /api/Defects/{id}` | Get one defect by id. |
| `POST /api/Defects` | Report a defect for the current user. |
| `GET /api/Products` | List products with pagination. |
| `GET /api/Products/{id}` | Get one product by id. |
| `POST /api/Products` | Create a product. |
| `GET /api/ProductionPlans` | List production plans with pagination. |
| `GET /api/ProductionPlans/{id}` | Get one production plan by id. |
| `POST /api/ProductionPlans` | Create a production plan for the current user. |
| `GET /api/ProductionOrders` | List production orders with pagination. |
| `GET /api/ProductionOrders/{id}` | Get one production order by id. |
| `POST /api/ProductionOrders` | Create a production order. |
| `GET /api/ProductionRecords` | List production records with pagination. |
| `GET /api/ProductionRecords/{id}` | Get one production record by id. |
| `POST /api/ProductionRecords` | Create a production record. |
| `GET /api/ProductionAnalytics` | Get production analytics for dashboard views. |
| `GET /api/admin/logs` | Admin-only endpoint to view recent audit logs. |

## Notes

* Most `GET` list endpoints support `pageNumber` and `pageSize`.
* POST endpoints use DTO request models instead of directly accepting full entities.
* Create actions add audit-log entries.
* Machine reading creation is currently commented out, so it is not available as an API endpoint.