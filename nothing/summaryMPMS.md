# MPMS Project Summary

## Project Purpose

The Manufacturing Process Management System (MPMS) is a process application designed to manage and orchestrate manufacturing execution from order reception through product delivery. It supports:
- process modeling and definition
- automated task assignment to agents
- runtime process monitoring and task status tracking
- integration with human operators and automated agents

## Main Business Capabilities

### Process Definition and Execution
- Persist and manage process definitions through `ProcessDef` entities.
- Store process objectives for cost, time, flexibility, and quality.
- Support multiple process variants represented by BPMN artifacts in `src/main/resources/bpmn/`.
- Provide lookup of processes by ID or name.

### Task Definition and Scheduling
- Persist and manage task definitions through `TaskDef` entities.
- Associate tasks with a process definition, role, location, and duration.
- List and retrieve tasks by process ID.
- Maintain unique task identifiers for integration and process mapping.

### Role and Skill Management
- Persist and manage roles through `Role` entities.
- Capture role name and description.
- Store abilities and skills separately as `Ability` and `Skill` entities, suggesting support for higher-level capability modeling.

### Agent Management and Assignment
- Store a generic `Agent` entity with performance and availability metadata.
- Manage specialized agents:
  - `HumanAgent` for human operators
  - `AutoAgent` for automated resources and machines
- Track agent status, team membership, role assignment, and availability.
- Query available agents by role or name.
- Query agents by team assignment.

### Monitoring and Integration
- The system supports runtime monitoring and allocation decisions using agent status fields.
- Integration services exist for persistence and retrieval of process, task, role, and agent data.
- Websocket and external component integration is noted in project documentation, enabling status updates and alerts.

## Database Schema

The project uses JPA-backed entity classes with direct column mapping. All main entities use generated identity primary keys and unique constraints on natural names.

### `Agent`
- `agent_id` (PK, bigint identity)
- `agent_name` (unique)
- `agent_type`
- `agent_fullname`
- `agent_performance_time`
- `agent_performance_quality`
- `agent_travel_speed`
- `agent_online` (boolean)
- `agent_queue`
- `agent_busy_time` (timestamp)
- `agent_cost`
- `agent_role_id` (foreign key reference by ID)

### `AutoAgent`
- `auto_agent_id` (PK, bigint identity)
- `auto_agent_name` (unique)
- `auto_agent_team_id`
- `auto_agent_role_id`
- `auto_agent_operation_status`

### `HumanAgent`
- `human_agent_id` (PK, bigint identity)
- `human_agent_name` (unique)
- `human_agent_team_id`
- `human_agent_role_id`
- `human_agent_operation_status`

### `ProcessDef`
- `process_id` (PK, bigint identity)
- `process_name` (unique)
- `process_descr`
- `process_objective_cost`
- `process_objective_time`
- `process_objective_flexibility`
- `process_objective_quality`

### `TaskDef`
- `task_id` (PK, bigint identity)
- `task_modeler_id`
- `task_unique_id` (unique)
- `task_name`
- `task_process_id` (foreign key reference by ID)
- `task_description`
- `task_role_id`
- `task_location`
- `task_duration`

### `Role`
- `role_id` (PK, bigint identity)
- `role_name` (unique)
- `role_description`

### `Ability`
- `ability_id` (PK, bigint identity)
- `ability_name` (unique)
- `ability_category`
- `ability_group`
- `ability_description`
- `ability_example_low`
- `ability_example_high`

### `Skill`
- `skill_id` (PK, bigint identity)
- `skill_name` (unique)
- `skill_description`

### `WSABatchEntity`
Defined in `src/main/resources/sql/create.sql`:
- `batch_id` (PK, bigint identity)
- `batch_no` (varchar(128))
- `batch_type` (varchar(128))
- `batch_quantity` (int)

## Entity Relationships and Data Flow

- `TaskDef.task_process_id` links tasks to process definitions.
- `TaskDef.task_role_id`, `Agent.agent_role_id`, `HumanAgent.human_agent_role_id`, and `AutoAgent.auto_agent_role_id` align task requirements with agent roles.
- `HumanAgent` and `AutoAgent` can be grouped by team via `*_team_id`.
- Agent availability is expressed through boolean `agent_online` and string `*_operation_status`.
- No explicit JPA object relationships are modeled; foreign keys are stored as long ID fields.

## Services and Use Cases

### Integration Services
The integration layer provides direct persistence and query operations for each domain entity:
- `AgentIntegrationService`
- `AutoAgentIntegrationService`
- `HumanAgentIntegrationService`
- `ProcessDefIntegrationService`
- `TaskDefIntegrationService`
- `RoleIntegrationService`

Supported operations include:
- persist new entities
- find by primary key
- update existing entities
- search by natural keys or status/role filters

### Business Services
The business layer wraps integration services and exposes higher-level operations such as:
- managing agent registration and updates
- storing process and task definitions
- business-level persistence workflows for process management

### Key Queries
- Find online agents by role
- Find online agents by name
- Find available human or auto agents by role
- List tasks belonging to a process
- Find a process by name or ID

## Process Artifacts

The project contains prebuilt BPMN process definitions under `src/main/resources/bpmn/`:
- `core_process.bpmn`
- `ar_autoagent_task_process.bpmn`
- `hts_autoagent_task_process.bpmn`
- `kuka_autoagent_task_process.bpmn`

These files indicate support for:
- core manufacturing orchestration
- augmented reality / autoagent tasks
- human-task system workflows
- KUKA robot integration

## Summary

MPMS is a process-driven manufacturing orchestration application with a domain model around processes, tasks, roles, abilities, skills, and agents. The database schema is centered on entities for process and task definitions, plus agent registries for humans and machines. The service layer enables persistence, retrieval, updates, and availability-based queries needed for runtime scheduling and monitoring.
