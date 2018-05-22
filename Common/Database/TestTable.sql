use TestDatabase
go

create TABLE TestDatabase.dbo.TestTable
(
	TestTableId uniqueidentifier NOT NULL,
	TestName varchar(100) NOT NULL,
	TestValue varchar(100) NOT NULL,
	
	CreateUserId uniqueidentifier NOT NULL,
	CreateDate datetime NOT NULL,
	ChangeUserId uniqueidentifier NOT NULL,
	ChangeDate datetime NOT NULL,
	DeleteDate datetime NULL
)
GO

alter table TestDatabase.dbo.TestTable add constraint PK_TestTable primary key nonclustered (TestTableId)
GO
CREATE nonclustered INDEX IX_TestTable_TestName ON TestDatabase.dbo.TestTable(TestName)
GO




--stored procedure
create PROCEDURE spTestTableGetTestTable
@TestTableId uniqueidentifier = null,
@TestName varchar(100) = null

as

IF @TestTableId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM TestDatabase.dbo.TestTable with (nolock)
	 WHERE TestTableId = @TestTableId
END
ELSE IF @TestName IS NOT NULL
BEGIN
	 SELECT * 
	 FROM TestDatabase.dbo.TestTable with (nolock)
	 Where TestName = @TestName
END
ELSE
BEGIN
	 SELECT *
	 FROM TestDatabase.dbo.TestTable with (nolock)
END	
GO


create PROCEDURE spTestTableSave
@TestTableId uniqueidentifier,
@TestName varchar(100),
@TestValue varchar(100),
@UserId uniqueidentifier

as

BEGIN
	IF EXISTS(SELECT * FROM TestDatabase.dbo.TestTable WHERE TestTableId = @TestTableId)
	BEGIN
		UPDATE TestDatabase.dbo.TestTable 
        SET TestName = @TestName, 
            TestValue = @TestValue,
            ChangeUserId = @UserId, 
            ChangeDate = sysutcdatetime(), 
            DeleteDate = null
        WHERE TestTableId = @TestTableId
	END
	ELSE 
	BEGIN
		INSERT INTO TestDatabase.dbo.TestTable
			   ([TestTableId]
			   ,[TestName]
			   ,[TestValue]
			   ,[CreateUserId]
			   ,[CreateDate]
			   ,[ChangeUserId]
			   ,[ChangeDate]
			   ,[DeleteDate])
		 VALUES
			   (@TestTableId
			   ,@TestName
			   ,@TestValue
			   ,@UserId
			   ,sysutcdatetime()
			   ,@UserId
			   ,sysutcdatetime()
			   ,null)
	END 
END
GO


create PROCEDURE spTestTableDelete
@TestTableId uniqueidentifier = null,
@UserId uniqueidentifier

as

BEGIN
	IF @TestTableId IS NOT NULL
	BEGIN
		UPDATE TestDatabase.dbo.TestTable 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE TestTableId = @TestTableId
	END
END	
GO

