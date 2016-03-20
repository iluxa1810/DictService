USE DictService
GO
SET IDENTITY_INSERT dbo.Permission ON
    INSERT INTO dbo.Permission (Permission_id,Name) values
 (1,N'Добавление словарей'),(2,N'Редактирование словарей'),(3,N'Удаление словарей'),(4,N'Редактирование привязок'),(5,N'Редактирование прав')
SET IDENTITY_INSERT dbo.Permission OFF
GO
SET IDENTITY_INSERT dbo.ActionType ON
    INSERT INTO dbo.ActionType (Action_id,Name)
    VALUES 
(1,N'Добавление словаря'),(2,N'Редактирование словаря'),(3,N'Удаление словаря'),(4,N'Редактирование приязок'),(5,'Редактирование юзеров')
SET IDENTITY_INSERT dbo.ActionType OFF

go
ALTER TRIGGER dbo.ChkPermiss
ON dbo.UserChangeHistrory
AFTER INSERT
AS 
BEGIN
	SELECT * FROM inserted
	IF NOT Exists(SELECT * FROM dbo.UserPermission AS up
	              INNER JOIN INSERTED i ON up.[User_id]=i.[user_id] AND up.UserPermission_id!=i.UserPermission_id
	              WHERE up.Permission_id=5 AND up.DateDel IS null)
    BEGIN
    	   RAISERROR('Отсутствуют права',1,1)
    	   ROLLBACK
    END
END
GO


ALTER TRIGGER dbo.ChkPermissOnEditPermiss
ON dbo.UserPermission
AFTER INSERT,Update
AS 
BEGIN
	SELECT * FROM inserted
	IF NOT Exists(SELECT * FROM dbo.UserPermission AS up
	              INNER JOIN INSERTED i ON up.[User_id]=i.[user_id] AND up.UserPermission_id!=i.UserPermission_id
	              WHERE up.Permission_id=5 AND up.DateDel IS null)
    BEGIN
    	   RAISERROR('Отсутствуют права',1,1)
    	   ROLLBACK
    END
END
GO
ALTER PROCEDURE AddPermission 
@TargetUser_id INT,
@Give_User_id INT,
@Permissons XML
AS
BEGIN
    INSERT INTO dbo.UserPermission ([User_id],Permission_id,Give_User_id)
    SELECT @TargetUser_id,a.value('(.)[1]','int') AS Permission_id,@Give_User_id FROM @Permissons.nodes('*/Permission_id') a(a) 
END
GO
--EXEC AddPrmisson @TargetUser_id=1, @Give_User_id=1, @Permissons=N'<Permissions> 
--<Permission_id>3</Permission_id>
--<Permission_id>4</Permission_id> 
--</Permissions>'

ALTER PROCEDURE RemovePermission 
@TargetUser_id INT,
@Remove_User_id INT,
@Permissons XML
AS
BEGIN
    	UPDATE up
    	SET up.Remove_User_id = @Remove_User_id,
    	 up.DateDel = GETDATE()
    	FROM dbo.UserPermission AS up
    	JOIN (SELECT a.value('(.)[1]','int') AS Permission_id 
    	      from @Permissons.nodes('*/Permission_id') a(a)) t ON t.Permission_id=up.Permission_id AND up.[User_id]=@TargetUser_id
END
Go
--EXEC RemovePrmisson @TargetUser_id=1, @Remove_User_id=1, @Permissons='<Permissions> 
--<Permission_id>3</Permission_id>
--<Permission_id>5</Permission_id> 
--</Permissions>'






DELETE dbo.UserPermission FROM dbo.UserPermission AS ups
DELETE up FROM dbo.UserPermission AS up

DISABLE TRIGGER dbo.CheckPermissionTrigger ON dbo.UserPermission


ENABLE TRIGGER dbo.CheckPermissionTrigger ON dbo.UserPermission

SELECT * FROM dbo.UserPermission AS up
WHERE up.UserPermission_id=14
--SELECT * FROM dbo.Permission 



