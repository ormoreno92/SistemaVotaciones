CREATE PROCEDURE sp_createUsers
    @Name nvarchar(50),
	@LastName nvarchar(50),
	@Doc nvarchar(50),
	@Birthday date,
	@Email nvarchar(50),
	@Password nvarchar(50),
	@Active bit,
	@RoleId int,
	@CourseId int
AS   

    SET NOCOUNT ON;  
    INSERT INTO USUARIOS VALUES (@Name, @LastName, @Doc, @Birthday, @Email, @Password, @Active, @RoleId, @CourseId)
GO 