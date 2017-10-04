using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bd;
using Modelo;

namespace Logica
{
    public class General
    {
        private readonly BaseDatosEntities _conexion = new BaseDatosEntities();
        public bool CrearRol(string roleName)
        {
            try
            {
                var query = @"exec sp_createRole '" + roleName + "'";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CrearCandidato(int idCandidato)
        {
            try
            {
                var query = @"exec sp_createCandidate '" + DateTime.Now + "', '" + idCandidato + "'";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CrearCurso(string courseName, bool valid)
        {
            try
            {
                var query = @"exec sp_createCourse '" + courseName + "', '" + valid + "'";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertarVotacion(int idCandidato, int idVotante)
        {
            try
            {
                var query = @"exec sp_insertvotations '" + idCandidato + "', '" + idVotante + "', '" + DateTime.Now + "'";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool InsertarPropuesta(int idCandidato, string propuesta, string descripcion)
        {
            try
            {
                var query = @"exec sp_insertproposals '" + idCandidato + "', '" + propuesta + "', '" + descripcion + "'";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public List<Rol> ObtenerRoles()
        {
            try
            {
                var query = @"exec sp_getRoles";
                return _conexion.Database.SqlQuery<Rol>(query).ToList();
            }
            catch (Exception e)
            {
                return new List<Rol>();
            }
        }

        public List<Curso> ObtenerCursos()
        {
            try
            {
                var query = @"exec sp_getCourses";
                return _conexion.Database.SqlQuery<Curso>(query).ToList();
            }
            catch (Exception e)
            {
                return new List<Curso>();
            }
        }

        public Curso ObtenerCurso(string nombre)
        {
            try
            {
                var query = @"exec sp_getCourse '" + nombre + "'";
                return _conexion.Database.SqlQuery<Curso>(query).FirstOrDefault();
            }
            catch (Exception e)
            {
                return new Curso();
            }
        }

        public bool CrearPropuesta(int idCandidato, string nombre, string descripcion)
        {
            try
            {
                var query = @"select candidato_id from Candidatos where usuario_id = '" + idCandidato + "'";
                query = @"exec sp_insertProposals '" + _conexion.Database.SqlQuery<int>(query).FirstOrDefault() + "', '" + nombre + "', '" + descripcion + "'";
                _conexion.Database.SqlQuery<object>(query).FirstOrDefault();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<Propuesta> ObtenerPropuestas(int idCandidato)
        {
            try
            {
                var query = @"select propuesta_id as Id, propuesta_nombre as NombrePropuesta, propuesta_descripcion as Descripcion FROM Propuestas join Candidatos on Propuestas.candidato_id = Candidatos.candidato_id where Candidatos.usuario_id = '" + idCandidato + "'";
                return _conexion.Database.SqlQuery<Propuesta>(query).ToList();
            }
            catch (Exception e)
            {
                return new List<Propuesta>();
            }
        }
    }
}
