use Database
go

create TABLE Database.dbo.ExampleTable
(
	ExampleTableId uniqueidentifier NOT NULL,
	ForeignKeyId uniqueidentifier NOT NULL,
	ForeignKeyId2  uniqueidentifier NOT NULL,
	
	CreateUserId uniqueidentifier NOT NULL,
	CreateDate datetime NOT NULL,
	ChangeUserId uniqueidentifier NOT NULL,
	ChangeDate datetime NOT NULL,
	DeleteDate datetime NULL
)

GO
--primary key
alter table Database.dbo.ExampleTable add constraint PK_ExampleTable primary key nonclustered (ExampleTableId)
GO
--foreign key
alter table Database.dbo.ExampleTable add constraint FK_ExampleTable_ForeignKeyId foreign key (ForeignKeyId)
	references Database.dbo.ForeignKey(ForeignKeyId)
GO
--unique constraint
CREATE unique nonclustered INDEX UX_ExampleTable ON Database.dbo.ExampleTable(ForeignKeyId, ForeignKeyId2)
go

--just an index
CREATE nonclustered INDEX IX_ExampleTable_ForeignKeyId ON Database.dbo.ExampleTable(ForeignKeyId)
GO

--foreign key with index example
alter table Database.dbo.ExampleTable add constraint FK_ExampleTable_ForeignKeyId2 foreign key (ForeignKeyId2)
	references Database.dbo.ForeignKey(ForeignKeyId2)
GO
CREATE nonclustered INDEX IX_FK_ExampleTable_ForeignKeyId2 ON Database.dbo.ExampleTable(ForeignKeyId2)
GO



--stored procedure
CREATE PROCEDURE spExampleTableGetExampleTable
@ExampleTableId uniqueidentifier = null,
@ForeignKeyId uniqueidentifier = null,
@ForeignKeyId2 uniqueidentifier = null

as

IF @ExampleTableId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM Database.dbo.ExampleTable with (nolock)
	 WHERE ExampleTableId = @ExampleTableId
END
ELSE IF @ForeignKeyId IS NOT NULL and @ForeignKeyId2 IS NOT NULL
BEGIN
	 SELECT * 
	 FROM Database.dbo.ExampleTable with (nolock)
	 WHERE ForeignKeyId = @ForeignKeyId
		and ForeignKeyId2 = @ForeignKeyId2
END
ELSE IF @ForeignKeyId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM Database.dbo.ExampleTable with (nolock)
	 Where ForeignKeyId = @ForeignKeyId
END
ELSE
BEGIN
	 SELECT *
	 FROM Database.dbo.ExampleTable with (nolock)
END	
GO


create PROCEDURE spExampleTableSave
@ExampleTableId uniqueidentifier,
@ForeignKeyId uniqueidentifier,
@ForeignKeyId2 uniqueidentifier,
@UserId uniqueidentifier

as

BEGIN
	IF EXISTS(SELECT * FROM Database.dbo.ExampleTable WHERE ExampleTableId = @ExampleTableId)
	BEGIN
		UPDATE Database.dbo.ExampleTable 
        SET ForeignKeyId = @ForeignKeyId, 
            ForeignKeyId2 = @ForeignKeyId2,
            ChangeUserId = @UserId, 
            ChangeDate = sysutcdatetime(), 
            DeleteDate = null
        WHERE ExampleTableId = @ExampleTableId
	END
	ELSE 
	BEGIN
		INSERT INTO Database.dbo.ExampleTable
			   ([ExampleTableId]
			   ,[ForeignKeyId]
			   ,[ForeignKeyId2]
			   ,[CreateUserId]
			   ,[CreateDate]
			   ,[ChangeUserId]
			   ,[ChangeDate]
			   ,[DeleteDate])
		 VALUES
			   (@ExampleTableId
			   ,@ForeignKeyId
			   ,@ForeignKeyId2
			   ,@DirectoryName
			   ,@UserId
			   ,sysutcdatetime()
			   ,@UserId
			   ,sysutcdatetime()
			   ,null)
	END 
END
GO


create PROCEDURE spExampleTableDelete
@ExampleTableId uniqueidentifier = null,
@UserId uniqueidentifier

as

BEGIN
	IF @ExampleTableId IS NOT NULL
	BEGIN
		UPDATE Database.dbo.ExampleTable 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE ExampleTableId = @ExampleTableId
	END
END	
GO

