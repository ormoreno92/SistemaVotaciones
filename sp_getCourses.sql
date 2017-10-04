CREATE PROCEDURE sp_getCourses
	
AS
	SELECT curso_id as Id, curso_nombre as Nombre, curso_valido_votacion as Valido
	FROM Cursos
GO
