use RecipeDatabase
go

create TABLE RecipeDatabase.dbo.Recipe
(
	RecipeId uniqueidentifier NOT NULL,
	Title varchar(250) not null,
	Description varchar(500) null,
	RecipeTypeId uniqueidentifier not null,
	Author varchar(250) null,
	AuthorWebsite varchar(250) null,
	
	CreateUserId uniqueidentifier NOT NULL,
	CreateDate datetime NOT NULL,
	ChangeUserId uniqueidentifier NOT NULL,
	ChangeDate datetime NOT NULL,
	DeleteDate datetime NULL
)

GO
alter table RecipeDatabase.dbo.Recipe add constraint PK_Recipe primary key nonclustered (RecipeId)
GO
alter table RecipeDatabase.dbo.Recipe add constraint FK_Recipe_RecipeTypeId foreign key (RecipeTypeId)
	references RecipeDatabase.dbo.RecipeType(RecipeTypeId)
GO
CREATE nonclustered INDEX IX_FK_Recipe_RecipeTypeId ON RecipeDatabase.dbo.Recipe(RecipeTypeId)
GO



--stored procedure
CREATE PROCEDURE spRecipeGetRecipe
@RecipeId uniqueidentifier = null,
@RecipeTypeId uniqueidentifier = null

as

IF @RecipeId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.Recipe with (nolock)
	 WHERE RecipeId = @RecipeId
END
ELSE IF @RecipeTypeId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.Recipe with (nolock)
	 Where RecipeTypeId = @RecipeTypeId
END
ELSE
BEGIN
	 SELECT *
	 FROM RecipeDatabase.dbo.Recipe with (nolock)
END	
GO


create PROCEDURE spRecipeSave
@RecipeId uniqueidentifier,
@Title varchar(250),
@Description varchar(500),
@RecipeTypeId uniqueidentifier,
@Author varchar(250),
@AuthorWebsite varchar(250),
@UserId uniqueidentifier

as

BEGIN
	IF EXISTS(SELECT * FROM RecipeDatabase.dbo.Recipe WHERE RecipeId = @RecipeId)
	BEGIN
		UPDATE RecipeDatabase.dbo.Recipe 
        SET Title = @Title, 
            Description = @Description,
			RecipeTypeId = @RecipeTypeId,
			Author = @Author,
			AuthorWebsite = @AuthorWebsite,
            ChangeUserId = @UserId, 
            ChangeDate = sysutcdatetime(), 
            DeleteDate = null
        WHERE RecipeId = @RecipeId
	END
	ELSE 
	BEGIN
		INSERT INTO RecipeDatabase.dbo.Recipe
			   ([RecipeId]
			   ,[Title]
			   ,[Description]
			   ,[RecipeTypeId]
			   ,[Author]
			   ,[AuthorWebsite]
			   ,[CreateUserId]
			   ,[CreateDate]
			   ,[ChangeUserId]
			   ,[ChangeDate]
			   ,[DeleteDate])
		 VALUES
			   (@RecipeId
			   ,@Title
			   ,@Description
			   ,@RecipeTypeId
			   ,@Author
			   ,@AuthorWebsite
			   ,@UserId
			   ,sysutcdatetime()
			   ,@UserId
			   ,sysutcdatetime()
			   ,null)
	END 
END
GO


create PROCEDURE spRecipeDelete
@RecipeId uniqueidentifier = null,
@UserId uniqueidentifier

as

BEGIN
	IF @RecipeId IS NOT NULL
	BEGIN
		UPDATE RecipeDatabase.dbo.Recipe 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE RecipeId = @RecipeId
	END
END	
GO

