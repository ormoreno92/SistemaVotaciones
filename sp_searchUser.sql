CREATE PROCEDURE sp_searchUser
	@doc nvarchar(50)
AS
	SET NOCOUNT ON;  
    SELECT u.usuario_id as Id, u.usuario_nombres as Nombres, u.usuario_apellidos as Apellidos,u.usuario_documento as Documento,u.usuario_fec_nac as FechaNacimiento,
	u.usuario_email as Email, u.usuario_activo as Activo, r.rol_descripcion as Rol, c.curso_nombre as Curso
    FROM Usuarios u
	join Roles r on u.rol_id = r.rol_id
	join Cursos c on u.curso_id = c.curso_id
    WHERE u.usuario_documento = @doc