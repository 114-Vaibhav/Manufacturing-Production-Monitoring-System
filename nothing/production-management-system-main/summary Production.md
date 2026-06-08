# Production Management System - Summary

## 1. System Overview
This system is a web-based production management system for a factory environment. It supports production planning, tracking of machines and materials, work shift assignments, production process monitoring, sorting of finished goods and waste, and reporting of completed production results.

## 2. Primary Actors
- **Admin**: Manages master data, creates and updates production requests, plans, and shift assignments, monitors machines and materials, and views reports.
- **Leader**: Manages planning details, shifts, production execution, machine and material allocations, and sorting results from active work assignments.
- **Authenticated User**: Accesses the system after login with a role of `admin` or `leader`.

## 3. Main Features and Services

### 3.1 Authentication
- Login service authenticates users using stored credentials in the `user` table.
- Sessions carry `user_id`, `username`, and `role` for role-based access control.
- Admin users are routed to the admin dashboard; leaders are routed to the leader dashboard.

### 3.2 Dashboard
- Displays summary counts of active projects, planning items, work shifts, and completed reports.
- Shows joined production and sorting reports with associated project, customer, and staff details.

### 3.3 Customer Management
- Add, update, delete, and list customer records.
- Customer records include name, address, phone, and email.
- Customer information links to production project requests.

### 3.4 Product and Project Management
- Product management includes product name, summary, and application details.
- Project requests represent customer orders, linking customers to products.
- Project fields include requested quantity, diameter, entry date, and status.

### 3.5 Production Planning
- Plans are created from projects and define a production target quantity and end date.
- Each plan can be linked to shift assignments.
- Planning status indicates whether the plan is active or completed.

### 3.6 Shift Assignment
- Shift assignments map planning entries to specific shifts and staff.
- Assignments include start date and status.
- Staff and shift schedule are managed as separate master entities.

### 3.7 Production Execution
- Production records are driven from shift assignments.
- Each plan-shift assignment may have machine and material allocations.
- Production details include target quantity, start date, assigned product, shift, and staff.

### 3.8 Machine Management
- Machines are managed with name, capacity, and availability status.
- Machines can be allocated to shift assignments through `p_machine` records.
- Allocation tracks whether a machine is in use or available.
- Completing a machine allocation returns the machine to available status.

### 3.9 Material Management
- Materials are tracked with name and available stock.
- Material usage is recorded per plan-shift assignment through `p_material` records.
- When materials are assigned, stock is reduced automatically.
- Deleting a material usage record restores consumed stock.

### 3.10 Sorting and Reporting
- Sorting reports capture waste and finished output for each plan-shift assignment.
- Sorting output is linked to shift assignment and is used to update cumulative finished production.
- Finished reports aggregate total finished goods per project.
- Reports support viewing and printing details of sorting and finished output.

## 4. Functional Flows

### 4.1 Admin Flow
- Login as admin.
- Manage master entities: customers, products, staff, machines, materials, and shift schedules.
- Create projects from customer requests.
- Create planning records from projects.
- Assign planning to shifts and staff as shift assignments.
- Allocate machines and materials to shift assignments.
- Review sorting reports and production completion.
- View finished goods totals by project.

### 4.2 Leader Flow
- Login as leader.
- Review planning items and project details.
- Inspect details of each plan including customer and product information.
- Manage shift assignments and production work details.
- Assign machines and materials to active shift assignments.
- Enter sorting results and waste/finished counts.
- View production and sorting reports.

## 5. Database Schema

### 5.1 Core Entities

#### `user`
- `user_id` (PK)
- `username`
- `password`
- `role` (`admin`, `leader`)

#### `customer`
- `id_cust` (PK)
- `cust_name`
- `address`
- `telp`
- `email`

#### `product`
- `id_product` (PK)
- `product_name`
- `summary`
- `application`

#### `project`
- `id_project` (PK)
- `project_name`
- `id_cust` (FK → `customer.id_cust`)
- `id_product` (FK → `product.id_product`)
- `diameter`
- `qty_request`
- `entry_date`
- `pr_status`

#### `planning`
- `id_plan` (PK)
- `plan_name`
- `id_project` (FK → `project.id_project`)
- `qty_target`
- `end_date`
- `pl_status`

#### `shiftment`
- `id_shift` (PK)
- `shift_name`
- `start_time`
- `end_time`

#### `staff`
- `id_staff` (PK)
- `staff_name`
- `phone`
- `email`
- `st_status`

### 5.2 Assignment and Allocation Entities

#### `plan_shift`
- `id_planshift` (PK)
- `id_plan` (FK → `planning.id_plan`)
- `id_shift` (FK → `shiftment.id_shift`)
- `id_staff` (FK → `staff.id_staff`)
- `start_date`
- `ps_status`

#### `machine`
- `id_machine` (PK)
- `machine_name`
- `capacity`
- `mc_status`

#### `p_machine`
- `id_pmachine` (PK)
- `id_planshift` (FK → `plan_shift.id_planshift`)
- `id_machine` (FK → `machine.id_machine`)
- `mc_stats`

#### `material`
- `id_material` (PK)
- `material_name`
- `stock`

#### `p_material`
- `id_pmaterial` (PK)
- `id_planshift` (FK → `plan_shift.id_planshift`)
- `id_material` (FK → `material.id_material`)
- `used_stock`

### 5.3 Reporting Entities

#### `sorting_report`
- `id_sorting` (PK)
- `id_planshift` (FK → `plan_shift.id_planshift`)
- `waste`
- `finished`

#### `finished_report`
- `id_finished` (PK)
- `id_project` (FK → `project.id_project`)
- `total_finished`
- `fdate`

## 6. Data Relationships
- A `customer` can own many `project` requests.
- A `project` is associated with one `product` and one `customer`.
- A `planning` item is derived from a `project` and carries a production target.
- A `plan_shift` ties a `planning` item to a specific `shiftment` and `staff` member.
- Machines and materials are allocated per active `plan_shift` through `p_machine` and `p_material`.
- Sorting output and waste are recorded per `plan_shift` in `sorting_report`.
- `finished_report` aggregates final finished quantities by `project`.

## 7. Important Business Rules
- Only authenticated users with role-based access can enter or modify production data.
- Machine allocation marks machine status as in-use and can be cleared to available after completion.
- Material assignment deducts stock from the master material inventory; removing usage restores stock.
- Sorting only applies to active plan-shift assignments and updates cumulative finished goods for the related project.
- Planning and shift assignments are central to moving customer project requests into actual production execution.

## 8. Notes
- The system is focused on production operations rather than inventory procurement or financial accounting.
- It emphasizes the workflow: customer request → project creation → production planning → shift assignment → resource allocation → output sorting/reporting.
- Reporting views are built around completed production and waste tracking.
