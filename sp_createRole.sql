CREATE PROCEDURE sp_createRole
    @Role nvarchar(50)
AS   

    SET NOCOUNT ON;  
    INSERT INTO ROLES VALUES (@Role)
GO 


