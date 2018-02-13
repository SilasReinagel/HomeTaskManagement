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

INSERT INTO HomeTask.Users (Id, Name, Username, Roles)
VALUES ('00000000-0000-0000-0000-000000000000', 'Nobody', 'Nobody', 'Basic')
GO

INSERT INTO HomeTask.Users (Id, Name, Username, Roles)
VALUES ('bbf38ff2-60ec-4ac3-8c2f-364f0d3277a3', 'App Worker', 'App Worker', 'Service')
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

---------- Create Task Instances Table ----------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'HomeTask'  AND  TABLE_NAME = 'TaskInstances'))
	DROP TABLE HomeTask.TaskInstances

CREATE TABLE HomeTask.TaskInstances (
	Id varchar(255) NOT NULL PRIMARY KEY,
	Description varchar(255) NOT NULL,
    Status varchar(255) NOT NULL,	
    TaskId varchar(255) NOT NULL,
	FOREIGN KEY (TaskId) REFERENCES HomeTask.Tasks(Id),
    UserId varchar(255) NOT NULL,
	FOREIGN KEY (UserId) REFERENCES HomeTask.Users(Id),
    Due datetime2 NOT NULL,
    Price int NOT NULL,
	IsFunded bit NOT NULL,
    FundedOn datetime2 NOT NULL,
	FundedByUserId varchar(255) NOT NULL,
	FOREIGN KEY (FundedByUserId) REFERENCES HomeTask.Users(Id),
    UpdatedStatusAt datetime2 NOT NULL,
	UpdatedStatusByUserId varchar(255) NOT NULL,
	FOREIGN KEY (UpdatedStatusByUserId) REFERENCES HomeTask.Users(Id)
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

CREATE INDEX idx_EntityType ON HomeTask.Events (EntityType)
CREATE INDEX idx_EntityType_EntityId ON HomeTask.Events (EntityType, EntityId)
CREATE INDEX idx_EntityType_EntityId_OccurredAt ON HomeTask.Events (EntityType, EntityId, OccurredAt)
GO

---------- Create BlobStore Table ----------

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'HomeTask'  AND  TABLE_NAME = 'Blobs'))
	DROP TABLE HomeTask.Blobs

CREATE TABLE HomeTask.Blobs (
	Id varchar(255) NOT NULL PRIMARY KEY,
	Value varbinary(MAX) NOT NULL
)
