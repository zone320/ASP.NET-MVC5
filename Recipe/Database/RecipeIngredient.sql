use RecipeDatabase
go

create TABLE RecipeDatabase.dbo.RecipeIngredient
(
	RecipeIngredientId uniqueidentifier NOT NULL,
	RecipeId uniqueidentifier NOT NULL,
	Sequence int NOT NULL,
	Ingredient varchar(max) NOT NULL,
	
	CreateUserId uniqueidentifier NOT NULL,
	CreateDate datetime NOT NULL,
	ChangeUserId uniqueidentifier NOT NULL,
	ChangeDate datetime NOT NULL,
	DeleteDate datetime NULL
)

GO
alter table RecipeDatabase.dbo.RecipeIngredient add constraint PK_RecipeIngredient primary key nonclustered (RecipeIngredientId)
GO

alter table RecipeDatabase.dbo.RecipeIngredient add constraint FK_RecipeIngredient_RecipeId foreign key (RecipeId)
	references RecipeDatabase.dbo.Recipe(RecipeId)
GO
CREATE nonclustered INDEX IX_FK_RecipeIngredient_RecipeId ON RecipeDatabase.dbo.RecipeIngredient(RecipeId)
GO



--stored procedure
CREATE PROCEDURE spRecipeIngredientGetRecipeIngredient
@RecipeIngredientId uniqueidentifier = null,
@RecipeId uniqueidentifier = null

as

IF @RecipeIngredientId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.RecipeIngredient with (nolock)
	 WHERE RecipeIngredientId = @RecipeIngredientId
END
ELSE IF @RecipeId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.RecipeIngredient with (nolock)
	 Where RecipeId = @RecipeId
END
ELSE
BEGIN
	 SELECT *
	 FROM RecipeDatabase.dbo.RecipeIngredient with (nolock)
END	
GO


create PROCEDURE spRecipeIngredientSave
@RecipeIngredientId uniqueidentifier,
@RecipeId uniqueidentifier,
@Sequence int,
@Ingredient varchar(max),
@UserId uniqueidentifier

as

BEGIN
	IF EXISTS(SELECT * FROM RecipeDatabase.dbo.RecipeIngredient WHERE RecipeIngredientId = @RecipeIngredientId)
	BEGIN
		UPDATE RecipeDatabase.dbo.RecipeIngredient 
        SET Sequence = @Sequence,
			Ingredient = @Ingredient,
            ChangeUserId = @UserId, 
            ChangeDate = sysutcdatetime(), 
            DeleteDate = null
        WHERE RecipeIngredientId = @RecipeIngredientId
	END
	ELSE 
	BEGIN
		INSERT INTO RecipeDatabase.dbo.RecipeIngredient
			   ([RecipeIngredientId]
			   ,[RecipeId]
			   ,[Sequence]
			   ,[Ingredient]
			   ,[CreateUserId]
			   ,[CreateDate]
			   ,[ChangeUserId]
			   ,[ChangeDate]
			   ,[DeleteDate])
		 VALUES
			   (@RecipeIngredientId
			   ,@RecipeId
			   ,@Sequence
			   ,@Ingredient
			   ,@UserId
			   ,sysutcdatetime()
			   ,@UserId
			   ,sysutcdatetime()
			   ,null)
	END 
END
GO


create PROCEDURE spRecipeIngredientDelete
@RecipeIngredientId uniqueidentifier = null,
@RecipeId uniqueidentifier = null,
@UserId uniqueidentifier

as

BEGIN
	IF @RecipeIngredientId IS NOT NULL
	BEGIN
		UPDATE RecipeDatabase.dbo.RecipeIngredient 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE RecipeIngredientId = @RecipeIngredientId
	END
	ELSE IF @RecipeId IS NOT NULL
	BEGIN
		UPDATE RecipeDatabase.dbo.RecipeIngredient 
        SET ChangeUserId = @UserId,
            ChangeDate = sysutcdatetime(),
            DeleteDate = sysutcdatetime()
        WHERE RecipeId = @RecipeId
			AND DeleteDate is null
	END
END	
GO

