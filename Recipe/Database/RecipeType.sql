use RecipeDatabase
go

create TABLE RecipeDatabase.dbo.RecipeType
(
	RecipeTypeId uniqueidentifier NOT NULL,
	Name varchar(50) NOT NULL,

	DeleteDate datetime NULL
)

GO
alter table RecipeDatabase.dbo.RecipeType add constraint PK_RecipeType primary key nonclustered (RecipeTypeId)
GO
CREATE unique nonclustered INDEX UX_RecipeType ON RecipeDatabase.dbo.RecipeType(Name)
go

insert into RecipeDatabase.dbo.RecipeType(RecipeTypeId, Name)
values(newid(), 'Lunch')
go
insert into RecipeDatabase.dbo.RecipeType(RecipeTypeId, Name)
values(newid(), 'Dinner')
go
insert into RecipeDatabase.dbo.RecipeType(RecipeTypeId, Name)
values(newid(), 'Dessert')
go
insert into RecipeDatabase.dbo.RecipeType(RecipeTypeId, Name)
values(newid(), 'Drinks')
go
insert into RecipeDatabase.dbo.RecipeType(RecipeTypeId, Name)
values(newid(), 'Misc')
go


--stored procedure
CREATE PROCEDURE spRecipeTypeGetRecipeType
@RecipeTypeId uniqueidentifier = null,
@Name varchar(50) = null

as

IF @RecipeTypeId IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.RecipeType with (nolock)
	 WHERE RecipeTypeId = @RecipeTypeId
END
ELSE IF @Name IS NOT NULL
BEGIN
	 SELECT * 
	 FROM RecipeDatabase.dbo.RecipeType with (nolock)
	 Where Name = @Name
END
ELSE
BEGIN
	 SELECT *
	 FROM RecipeDatabase.dbo.RecipeType with (nolock)
END	
GO

