CREATE PROCEDURE sp_getRoles
	
AS
	SELECT rol_id as Id, rol_descripcion as Nombre
	FROM Roles
GO
