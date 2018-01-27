CREATE SCHEMA HomeTask AUTHORIZATION HomeTaskAdmin
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

CREATE UNIQUE CLUSTERED INDEX UCX_Users_Id ON HomeTask.Users (Id)
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

CREATE UNIQUE CLUSTERED INDEX UCX_Tasks_Id ON HomeTask.Tasks (Id)
GO
