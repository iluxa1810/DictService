CREATE DATABASE DictService

--DROP DATABASE DictService
GO
USE DictService
GO
CREATE TABLE OctopusServer (
	Server_id INT IDENTITY (1,1) NOT NULL,
	ServerIp NVARCHAR(20) NOT NULL ,
	OctopusDbName NVARCHAR(20) NOT NULL ,
	ServerName  NVARCHAR(20) NOT NULL ,
	ClientToolsPath  NVARCHAR(255) NOT NULL ,
	CONSTRAINT PK_OctopusServer PRIMARY KEY (Server_id))
GO
CREATE TABLE Module (
	Server_id INT NOT NULL,
	Module_id INT NOT NULL,
	Name NVARCHAR(255) NOT NULL,
	FilePath NVARCHAR(255) NOT NULL,
	DateDel DATETIME,
	CONSTRAINT PK_Module PRIMARY KEY (Server_id,Module_id),
	CONSTRAINT FK_Module_OctopusServer FOREIGN KEY (Server_id) REFERENCES OctopusServer(Server_id))
GO
CREATE TABLE ProjectState(
	State_id INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_ProjectState PRIMARY KEY (State_id),CONSTRAINT UQ_ProjectState_Name UNIQUE ([Name]))
GO
CREATE TABLE Project (
	Server_id INT NOT NULL,
	Project_id INT NOT NULL,	
	[Name] NVARCHAR(50) NOT NULL,
	ZNName NVARCHAR(50) NOT NULL CONSTRAINT DT_Project_ZNName DEFAULT ('N\A'),
	State_id INT NOT NULL,
	CONSTRAINT PK_Project PRIMARY KEY (Project_id,Server_id),
	CONSTRAINT FK_Project_OctopusServer FOREIGN KEY (Server_id) REFERENCES OctopusServer(Server_id), 
	CONSTRAINT FK_Project_ProjectState FOREIGN KEY (State_id) REFERENCES ProjectState(State_id))
GO
CREATE TABLE TaskState(
		State_id INT NOT NULL IDENTITY (1,1),
		[Name] NVARCHAR(50) NOT NULL,
		CONSTRAINT PK_TaskState PRIMARY KEY (State_id))
GO
CREATE TABLE Task (
	Server_id INT NOT NULL,
	Project_id INT NOT NULL,
	Task_id INT NOT NULL,
	Module_id INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	State_id int NOT NULL,
	DateDel DATETIME,
	CONSTRAINT PK_Task PRIMARY KEY (Server_id,Project_id,Task_id),
	CONSTRAINT FK_Task_Project FOREIGN KEY (Project_id,Server_id) REFERENCES Project(Project_id,Server_id),
	CONSTRAINT FK_Task_Module FOREIGN KEY (Server_id,Module_id) REFERENCES Module(Server_id,Module_id),
	CONSTRAINT FK_Task_TaskState FOREIGN KEY (State_id) REFERENCES TaskState(State_id))
GO
CREATE TABLE DictionaryState(
	State_id INT IDENTITY (1,1) NOT NULL,
	[Name] NVARCHAR(20) NOT NULL CONSTRAINT UQ_DictionaryState_Name UNIQUE,
	CONSTRAINT PK_DictionaryState PRIMARY KEY (State_id))
GO
CREATE TABLE DictionaryCategory(
	Category_id INT NOT NULL IDENTITY (1,1),
	[Name] NVARCHAR(20) NOT NULL CONSTRAINT UQ_DictionaryCategory_Name UNIQUE,
	[Description] NVARCHAR(100) NOT NULL,
	CONSTRAINT PK_DictionaryCategory PRIMARY KEY (Category_id))
GO
CREATE TABLE Dictionary(
	Dictionary_id INT IDENTITY (1,1) NOT NULL ,
	[FileName] NVARCHAR(100) NOT NULL,
	[FriendlyName] NVARCHAR(100) NOT NULL,
	Category_id INT NOT NULL CONSTRAINT DT_Category_id DEFAULT (0),
	[Description] NVARCHAR(100) NOT NULL,
	State_id INT NOT NULL CONSTRAINT DT_Sate_id DEFAULT (0),
	PathToDict NVARCHAR(300) NOT NULL,
	DateDel DATETIME,
	ParentDict_id INT,CONSTRAINT PK_Dictionary PRIMARY KEY (Dictionary_id),
	CONSTRAINT FK_Dictionary_DictionaryCategory FOREIGN KEY (Category_id) REFERENCES DictionaryCategory(Category_id),
	CONSTRAINT FK_Dictionary_DictionaryState FOREIGN KEY (State_id) REFERENCES DictionaryState(State_id),
	CONSTRAINT UQ_Dictionary_Name_Category UNIQUE ([FriendlyName],Category_id),
	CONSTRAINT FK_Dictionary_ParentDictionary FOREIGN KEY (ParentDict_id) REFERENCES Dictionary(Dictionary_id))
GO
CREATE TABLE Version(
	Version_id INT IDENTITY (1,1) NOT NULL,
	Dictionary_id INT NOT NULL,
	[DateAdd] DATETIME CONSTRAINT DT_Version_DateAdd DEFAULT GETDATE(),
	DateDel DATETIME,
	PathToVersion NVARCHAR(300),CONSTRAINT PK_Version PRIMARY KEY (Version_id),
	CONSTRAINT PK_Version_Dictionary FOREIGN KEY (Dictionary_id) REFERENCES Dictionary(Dictionary_id))
	
	--ALTER TABLE DictionaryState ADD CONSTRAINT DT_DictionaryState_State_id  DEFAULT  0 FOR [DateAdState_id]
GO
CREATE TABLE DictionaryOnTask(
	DictionaryOnTask_id INT IDENTITY (1,1) NOT NULL,
    	Server_id INT NOT NULL,
	Project_id INT NOT NULL,	
	Task_id INT NOT NULL,
	Dictionary_id INT NOT NULL,
	DateAdded DATETIME NOT NULL CONSTRAINT DT_DictionaryOnTask_DateAdded DEFAULT GETDATE(),
	DateSync DATETIME NOT NULL CONSTRAINT DT_DictionaryOnTask_DateSync DEFAULT GETDATE(),
	DateDel DATETIME,
	CONSTRAINT PK_DictionaryOnTask PRIMARY KEY (DictionaryOnTask_id),
	CONSTRAINT FK_DictionaryOnTask_Task FOREIGN KEY (Server_id,Project_id,Task_id) REFERENCES Task(Server_id,Project_id,Task_id),
	CONSTRAINT FK_DictionaryOnTask_Dictionary FOREIGN KEY (Dictionary_id) REFERENCES Dictionary(Dictionary_id))
GO
CREATE TABLE ActionType (
	Action_id INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_ActionType PRIMARY KEY (Action_id),
	CONSTRAINT UQ_ActionType_Name UNIQUE ([Name]))
GO

CREATE TABLE [User] (
	[User_id] INT IDENTITY (1,1) NOT NULL,
	[Login] NVARCHAR(50) NOT NULL, 
	DateDel DATETIME,
	CONSTRAINT PK_User PRIMARY KEY ([User_id]),
	CONSTRAINT UQ_User_Login UNIQUE ([Login]))
GO
CREATE TABLE Permission (
	Permission_id INT IDENTITY (1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_Permission PRIMARY KEY (Permission_id),
	CONSTRAINT UQ_Permission_Name UNIQUE ([Name]))
GO
CREATE TABLE UserPermission(
	UserPermission_id INT IDENTITY (1,1) NOT NULL,
	[User_id] INT NOT NULL,
	Permission_id INT NOT NULL,
	Give_User_id INT NOT NULL,
	Remove_User_id INT,
	DateDel DATETIME,
	DateAdded DATETIME NOT NULL CONSTRAINT DT_UserPermission_DateAdded DEFAULT GETDATE(),
	CONSTRAINT PK_UserPermission PRIMARY KEY (UserPermission_id),
	CONSTRAINT FK_UserPermission_User FOREIGN KEY ([User_id]) REFERENCES [User]([User_id]),
	CONSTRAINT FK_UserPermission_Permission FOREIGN KEY (Permission_id) REFERENCES Permission(Permission_id),
     CONSTRAINT UQ_UserPermission_UserId_PermissionId_DateDel UNIQUE ([User_id],Permission_id,DateDel))-- Не дает выдать одному юзеру одно и тоже право 2 раза, если право не отобрано.
GO
CREATE TABLE UserChangeHistory(
	UserHistory_id INT IDENTITY (1,1) NOT NULL,
	[User_id] INT NOT NULL,
	Dictionary_id INT NOT NULL,
	Action_id INT NOT NULL,
	DateHistory DATETIME NOT Null,
	CONSTRAINT PK_UserChangeHistory PRIMARY KEY (UserHistory_id),
	CONSTRAINT FK_UserChangeHistory_User FOREIGN KEY ([User_id]) REFERENCES [User]([User_id]),
	CONSTRAINT FK_UserChangeHistory_Dictionary FOREIGN KEY (Dictionary_id) REFERENCES Dictionary(Dictionary_id),
	CONSTRAINT FK_UserChangeHistory_Action FOREIGN KEY (Action_id) REFERENCES [ActionType](Action_id))
GO
CREATE TABLE [Queue](
	Queue_id INT IDENTITY(1,1) NOT NULL,
	Server_id INT NOT NULL,
	Project_id INT NOT NULL,
	Task_id INT NOT NULL,
	Dictionary_id INT NOT NULL,
	Action_id INT NOT NULL,
	[Error] NVARCHAR(255),
	UserHistory_id INT , 
	CONSTRAINT PK_Queue PRIMARY KEY (Queue_id),
	CONSTRAINT FK_Queue_Task FOREIGN KEY (Server_id,Project_id,Task_id) REFERENCES Task(Server_id,Project_id,Task_id),
	CONSTRAINT FK_Queue_ActionType FOREIGN KEY (Action_id) REFERENCES ActionType(Action_id),
	CONSTRAINT FK_Queue_Dictionary FOREIGN KEY (Dictionary_id) REFERENCES Dictionary(Dictionary_id),
     CONSTRAINT FK_Queque_UserChangeHistory FOREIGN KEY (UserHistory_id) REFERENCES UserChangeHistory(UserHistory_id))
GO

CREATE TABLE ChangeType (
	Change_id INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_ChangeType PRIMARY KEY (Change_id),
	CONSTRAINT UQ_ChangeType_Name UNIQUE ([Name]))
Go
CREATE TABLE DictionaryChangeHistory (
	DictionaryHistory_id INT IDENTITY (1,1) NOT NULL,
	UserHistory_id INT NOT NULL,
	Dictionary_id INT NOT NULL,
	[Message] nvarchar(255),
	Change_id INT NOT NULL,
	CONSTRAINT PK_DictionaryChangeHistory PRIMARY KEY (DictionaryHistory_id),
	CONSTRAINT FK_DictionaryChangeHistory_UserChangeHistory FOREIGN KEY (UserHistory_id) REFERENCES UserChangeHistory(UserHistory_id),
	CONSTRAINT FK_DictionaryChangeHistory_ChangeType FOREIGN KEY (Change_id) REFERENCES ChangeType(Change_id),
	CONSTRAINT FK_DictionaryChangeHistory_Dictionary FOREIGN KEY (Dictionary_id) REFERENCES Dictionary(Dictionary_id))
GO
CREATE TABLE ActionTypePermission (
	Action_id INT NOT NULL,
	Permission_id INT NOT NULL,
	CONSTRAINT PK_ActionTypePermission PRIMARY KEY (Action_id,Permission_id),
	CONSTRAINT FK_ActionTypePermission_ActionType FOREIGN KEY (Action_id) REFERENCES ActionType(Action_id),
	CONSTRAINT FK_ActionTypePermission_Permission FOREIGN KEY (Permission_id) REFERENCES Permission(Permission_id))
GO
--CREATE TABLE [QueueUserChangeHistory](
--	Queue_id INT NOT NULL,
--	UserHistory_id INT NOT NULL,
--	CONSTRAINT PK_QueueUserChangeHistory PRIMARY KEY (Queue_id,UserHistory_id),
--	CONSTRAINT FK_QueueUserChangeHistory_Queue FOREIGN KEY (Queue_id) REFERENCES [Queue](Queue_id),
--	CONSTRAINT FK_QueueUserChangeHistory_UserChangeHistory FOREIGN KEY (UserHistory_id) REFERENCES [UserChangeHistory](UserHistory_id))
--GO

CREATE TYPE dbo.SyncTask as TABLE (
	Project_id INT NOT NULL,
	Task_id INT NOT NULL,
	Module_id INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL)
go
CREATE TYPE dbo.SyncProject AS TABLE (
	Project_id INT NOT NULL,	
	[Name] NVARCHAR(50) NOT NULL,
	ZNName NVARCHAR(50),
	State_id INT NOT NULL)
GO
CREATE TYPE dbo.SyncModule AS TABLE (
	Module_id INT NOT NULL,
	Name NVARCHAR(255) NOT NULL,
	FilePath NVARCHAR(255) NOT NULL)
GO

SET IDENTITY_INSERT dbo.ActionType ON
    INSERT INTO dbo.ActionType(Action_id, NAME) VALUES (0,N'Добавление словаря'),(1,N'Удаление словаря'),(2,N'Редактирование словаря'),(3,N'Редактирование привязок'),(4,N'Редактирование прав')
SET IDENTITY_INSERT dbo.ActionType OFF 
GO
SET IDENTITY_INSERT dbo.Permission ON
    INSERT INTO dbo.Permission(Permission_id, [Name]) VALUES (1,N'SuperUser')
SET IDENTITY_INSERT dbo.Permission OFF 
GO
    INSERT INTO dbo.ActionTypePermission(Action_id, Permission_id) VALUES (0,1),(1,1),(2,1),(3,1),(4,1)
GO
--SET IDENTITY_INSERT dbo.ProjectState ON
    INSERT INTO dbo.ProjectState(State_id, [Name]) VALUES (1,N'Доступен всем'),(2,N'Доступен только АУП'),(0,N'Не доступен')
--SET IDENTITY_INSERT dbo.ProjectState OFF 
GO
SET IDENTITY_INSERT dbo.ChangeType ON
    INSERT INTO dbo.ChangeType(Change_id, [Name]) VALUES (0,N'Добавление записи'),(1,N'Удаление записи'),(2,N'Добавление таблицы'),(3,N'Удаление таблицы'),(4,N'Добавление колонки'),(5,N'Удаление колонки')
SET IDENTITY_INSERT dbo.ChangeType OFF 
GO
SET IDENTITY_INSERT dbo.DictionaryState ON
    INSERT INTO dbo.DictionaryState(State_id, [Name]) VALUES (0,N'Доступен'),(1,N'Заблокирован'),(2,N'Обновляется'),(3,N'Удален')
SET IDENTITY_INSERT dbo.DictionaryState OFF 
GO
SET IDENTITY_INSERT dbo.TaskState ON 
INSERT INTO dbo.TaskState(State_id, NAME) VALUES (0,'Доступен'),(1,'Заблкирван')
SET IDENTITY_INSERT dbo.TaskState OFF
GO
------------------------------================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE SyncDB
@Server_id INT,
@Task dbo.SyncTask READONLY,
@Project dbo.SyncProject READONLY,
@Module dbo.SyncModule READONLY
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE p1
	SET p1.Name = p2.Name,
	 p1.ZNName = ISNULL(p2.ZNName,'N/A'),
	 p1.State_id = p2.State_id
	FROM Project AS p1
	JOIN @Project AS p2 ON p1.Project_id=p2.Project_id AND p1.Server_id=@Server_id
	
	UPDATE p
	SET p.State_id=0
	FROM dbo.Project p
	LEFT JOIN @Project AS p2 ON p2.Project_id=p.project_id AND p.Server_id=@Server_id
	WHERE p2.Project_id IS NULL
	
	INSERT INTO dbo.Project (Server_id, Project_id, Name, ZNName, State_id)
	SELECT @Server_id,p.project_id,p.Name,ISNULL(p.name,'N/A'),p.State_id FROM @Project p
	LEFT JOIN Project AS p2 ON p2.Project_id=p.project_id AND p2.Server_id=@Server_id
	WHERE p2.Project_id IS NULL
	
	UPDATE m
	SET m.Name = m2.Name,
	 m.FilePath = m2.FilePath
	FROM dbo.Module AS m
	JOIN @Module m2 ON m.Module_id=m.Module_id AND m.Server_id=@Server_id
	
	UPDATE m
	SET m.DateDel=GETDATE()
	FROM dbo.Module AS m
	LEFT JOIN @Module m2 ON m.Module_id=m.Module_id AND m.Server_id=@Server_id
	WHERE m2.Module_id IS null
	
	INSERT INTO dbo.Module(Server_id, Module_id, Name, FilePath)
	SELECT @Server_id, m.Module_id, m.Name, m.FilePath FROM @Module m
	LEFT JOIN dbo.Module AS m2 ON m2.Module_id=m.Module_id AND m2.Server_id=@Server_id
	WHERE m2.Module_id IS NULL
	
	UPDATE t
	SET t.Module_id = t2.Module_id,
	 t.Name = t2.Name 
	FROM dbo.Task t
	JOIN @Task AS t2 ON t2.Project_id=t.Project_id AND t2.Task_id=t.Task_id AND t.Server_id=@Server_id
	
	UPDATE t
	SET t.DateDel=GETDATE()
	FROM dbo.Task t
	JOIN @Task AS t2 ON t2.Project_id=t.Project_id AND t2.Task_id=t.Task_id AND t.Server_id=@Server_id
	WHERE t2.Project_id IS null 
	
	INSERT INTO Task (Server_id, Project_id, Task_id, Module_id, NAME)
	SELECT @Server_id AS Serv, t.Project_id, t.Task_id, t.Module_id, t.NAME FROM __tmp t
	LEFT JOIN dbo.Task AS t2 ON t2.Project_id=t.Project_id AND t2.Task_id=t.Task_id AND t2.Server_id=@Server_id
	WHERE t2.Project_id IS null 
END
GO
