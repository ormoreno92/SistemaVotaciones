CREATE PROCEDURE sp_createCourse
    @Course nvarchar(50),
	@Valid bit
AS   

    SET NOCOUNT ON;  
    INSERT INTO CURSOS VALUES (@Course, @Valid)
GO 