use RecipeDatabase
go

create TABLE RecipeDatabase.dbo.RecipeInstruction
(
	RecipeInstructionId uniqueidentifier NOT NULL,
	RecipeId uniqueidentifier NOT NULL,
	Sequence int NOT NULL,
	Instruction varchar(max) NOT NULL,
	
	CreateUserId uniqueidentifier NOT NULL,
	CreateDate datetime NOT NULL,
	ChangeUserId uniqueidentifier NOT NULL,
	ChangeDate datetime NOT NULL,
	DeleteDate datetime NULL
)

GO
alter table RecipeDatabase.dbo.RecipeInstruction add constraint PK_RecipeInstruction primary key nonclustered (RecipeInstructionId)
GO

alter table RecipeDatabase.dbo.RecipeInstruction add constraint FK_RecipeInstruction_RecipeId foreign key (RecipeId)
	references RecipeDatabase.dbo.Recipe(RecipeId)
GO
CREATE nonclustered INDEX IX_FK_RecipeInstruction_RecipeId ON RecipeDatabase.dbo.RecipeInstruction(RecipeId)
GO



--stored procedure
CREATE PROCEDURE spRecipeInstructionGetRecipeInstruction
@RecipeInstructionId uniqueidentifier = null,
@RecipeId uniqueidentifier = null

as

IF @RecipeInstructionId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.RecipeInstruction with (nolock)
	 WHERE RecipeInstructionId = @RecipeInstructionId
END
ELSE IF @RecipeId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.RecipeInstruction with (nolock)
	 Where RecipeId = @RecipeId
END
ELSE
BEGIN
	 SELECT *
	 FROM RecipeDatabase.dbo.RecipeInstruction with (nolock)
END	
GO


create PROCEDURE spRecipeInstructionSave
@RecipeInstructionId uniqueidentifier,
@RecipeId uniqueidentifier,
@Sequence int,
@Instruction varchar(max),
@UserId uniqueidentifier

as

BEGIN
	IF EXISTS(SELECT * FROM RecipeDatabase.dbo.RecipeInstruction WHERE RecipeInstructionId = @RecipeInstructionId)
	BEGIN
		UPDATE RecipeDatabase.dbo.RecipeInstruction 
        SET Sequence = @Sequence,
			Instruction = @Instruction,
            ChangeUserId = @UserId, 
            ChangeDate = sysutcdatetime(), 
            DeleteDate = null
        WHERE RecipeInstructionId = @RecipeInstructionId
	END
	ELSE 
	BEGIN
		INSERT INTO RecipeDatabase.dbo.RecipeInstruction
			   ([RecipeInstructionId]
			   ,[RecipeId]
			   ,[Sequence]
			   ,[Instruction]
			   ,[CreateUserId]
			   ,[CreateDate]
			   ,[ChangeUserId]
			   ,[ChangeDate]
			   ,[DeleteDate])
		 VALUES
			   (@RecipeInstructionId
			   ,@RecipeId
			   ,@Sequence
			   ,@Instruction
			   ,@UserId
			   ,sysutcdatetime()
			   ,@UserId
			   ,sysutcdatetime()
			   ,null)
	END 
END
GO


create PROCEDURE spRecipeInstructionDelete
@RecipeInstructionId uniqueidentifier = null,
@RecipeId uniqueidentifier = null,
@UserId uniqueidentifier

as

BEGIN
	IF @RecipeInstructionId IS NOT NULL
	BEGIN
		UPDATE RecipeDatabase.dbo.RecipeInstruction 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE RecipeInstructionId = @RecipeInstructionId
	END
	ELSE IF @RecipeId IS NOT NULL
	BEGIN
		UPDATE RecipeDatabase.dbo.RecipeInstruction 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE RecipeId = @RecipeId
			AND DeleteDate is null
	END
END	
GO

