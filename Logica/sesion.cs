using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Bd;
using Modelo;


namespace Logica
{
    public class Sesion
    {
        private readonly BaseDatosEntities _conexion = new BaseDatosEntities();
        private readonly Seguridad _seguridad = new Seguridad();
        public static Usuario UsuarioLogueado = new Usuario();

        public Usuario IniciarSesion(string doc, string password)
        {
            try
            {
                var nPassword = _seguridad.SerializarPassword(password);
                var query = @"exec sp_login '" + doc + "', '" + nPassword + "'";
                return _conexion.Database.SqlQuery<Usuario>(query).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool CrearUsuario(string name, string lastname, string doc, string birthday, string email, string password, int roleId,
            int cursoId, bool activo = true)
        {
            try
            {
                var nPassword = _seguridad.SerializarPassword(password);
                var query = @"exec sp_createUsers '" + name + "', '" + lastname + "', '" + doc + "', '" + birthday + "', '" + email
                    + "', '" + nPassword + "', '" + activo + "', '" + roleId + "', '" + cursoId + "'";
                _conexion.Database.SqlQuery<object>(query).SingleOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool EditarUsuario(int idUsuario, string name, string lastname, string doc, DateTime birthday, string email, int roleId,
            int cursoId, bool activo = true)
        {
            try
            {
                var query = @"exec sp_createUsers '" + idUsuario + "', '" + name + "', '" + lastname + "', '" + doc + "', '" + birthday + "', '" + email
                            + "', '" + activo + "'";
                _conexion.Database.SqlQuery<object>(query).SingleOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Usuario> ListaEstudiantes()
        {
            try
            {
                var query = @"exec sp_getUsers";
                return _conexion.Database.SqlQuery<Usuario>(query).ToList();
            }
            catch (Exception e)
            {
                return new List<Usuario>();
            }
        }

        public Usuario BuscarUsuario(string doc)
        {
            try
            {
                var query = @"exec sp_searchUser '" + doc + "'";
                return _conexion.Database.SqlQuery<Usuario>(query).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<Usuario> ObtenerVotos()
        {
            try
            {
                var lista = new List<Usuario>();
                var usuarios = ListaEstudiantes();
                foreach (var estudiante in usuarios)
                {
                    var query = @"select count(*) from Candidatos where usuario_id = '" + estudiante.Id + "'";
                    if (_conexion.Database.SqlQuery<int>(query).FirstOrDefault() > 0) lista.Add(estudiante);
                }
                foreach (var candidato in lista)
                {
                    var query = @"select count(*) from Votos v join Candidatos c on c.candidato_id = v.candidato_id where c.usuario_id = '" + candidato.Id + "'";
                    candidato.Votos = _conexion.Database.SqlQuery<int>(query).FirstOrDefault();
                }
                return lista;
            }
            catch (Exception e)
            {
                return new List<Usuario>();
            }
        }

        public bool RevisarVotoEstudiante(int idUsuario)
        {
            try
            {
                var query = @"select count(*) from Votos where usuario_id = '" + idUsuario + "'";
                return _conexion.Database.SqlQuery<int>(query).FirstOrDefault() > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool RevisarEstudianteEsCandidato(int idUsuario)
        {
            try
            {
                var query = @"select count(*) from Candidatos where usuario_id = '" + idUsuario + "'";
                return _conexion.Database.SqlQuery<int>(query).FirstOrDefault() > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Usuario> GetCandidatos()
        {
            try
            {
                var lista = new List<Usuario>();
                var usuarios = ListaEstudiantes();
                foreach (var estudiante in usuarios)
                {
                    var query = @"select count(*) from Candidatos where usuario_id = '" + estudiante.Id + "'";
                    if (_conexion.Database.SqlQuery<int>(query).FirstOrDefault() > 0) lista.Add(estudiante);
                }
                return lista;
            }
            catch (Exception e)
            {
                return new List<Usuario>();
            }
        }

        public bool Votar(int idUsuario, int idCandidato)
        {
            try
            {
                var query = @"select candidato_id from Candidatos where usuario_id = '" + idCandidato + "'";
                query = @"INSERT INTO Votos VALUES ('" + _conexion.Database.SqlQuery<int>(query).FirstOrDefault() + "', '" + idUsuario + "', '" + DateTime.Now + "')";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void DeleteUser(int idUsuario)
        {
            var query = @"delete from Votos where usuario_id = " + idUsuario;
            _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
            query = @"delete from Propuestas where candidato_id = (select top 1 candidato_id from Candidatos where usuario_id = " + idUsuario + ")";
            _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
            query = @"delete from Candidatos where usuario_id = " + idUsuario;
            _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
            query = @"delete from Usuarios where usuario_id = " + idUsuario;
            _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
        }

        public void EditUser(int idUsuario, string name, string lastName, string doc, string course, string email)
        {
            try
            {
                var query = @"Update Usuarios set usuario_nombres = '" + name + "', usuario_apellidos = '" + lastName
                            + "', usuario_documento = '" + doc + "', curso_id = (select top 1 curso_id from Cursos where curso_nombre ='" +
                            course + "'), usuario_email = '" + email + "' where usuario_id = " + idUsuario;
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
            }
            catch (Exception e)
            {
                //
            }
        }
    }
}
