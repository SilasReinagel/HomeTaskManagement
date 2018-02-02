IF NOT EXISTS (SELECT  schema_name FROM information_schema.schemata WHERE   schema_name = 'HomeTask' ) 
	BEGIN
		EXEC ('CREATE SCHEMA HomeTask AUTHORIZATION HomeTaskAdmin')
	END
GO

---------- Create Users Table ----------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'HomeTask'  AND  TABLE_NAME = 'Users'))
	DROP TABLE HomeTask.Users

CREATE TABLE HomeTask.Users (
	Id varchar(255) NOT NULL PRIMARY KEY,
	Username varchar(255) NOT NULL,
	Name varchar(255) NOT NULL,
	Roles varchar(512) NOT NULL
)
GO

---------- Create Tasks Table ----------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'HomeTask'  AND  TABLE_NAME = 'Tasks'))
	DROP TABLE HomeTask.Tasks

CREATE TABLE HomeTask.Tasks (
	Id varchar(255) NOT NULL PRIMARY KEY,
	Name varchar(255) NOT NULL,
	UnitsOfWork int NOT NULL,
	Frequency varchar(32) NOT NULL,
	Importance varchar(32) NOT NULL
)
GO


---------- Create EventStore Table ----------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'HomeTask'  AND  TABLE_NAME = 'Events'))
	DROP TABLE HomeTask.Events

CREATE TABLE HomeTask.Events (
	EntityType varchar(255) NOT NULL,
	EntityId varchar(255) NOT NULL,
	Name varchar(255) NOT NULL,
	Version int NOT NULL,
	JsonPayload varchar(MAX) NOT NULL,
	OccurredAt datetime2 NOT NULL
)
GO

CREATE INDEX idx_EntityType ON HomeTask.Events (EntityType)
CREATE INDEX idx_EntityType_EntityId ON HomeTask.Events (EntityType, EntityId)
CREATE INDEX idx_EntityType_EntityId_OccurredAt ON HomeTask.Events (EntityType, EntityId, OccurredAt)
GO