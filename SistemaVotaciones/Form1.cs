using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;

namespace SistemaVotaciones
{
    public partial class Form1 : Form
    {
        private readonly Sesion _sesion = new Sesion();
        private readonly General _generales = new General();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetInitials();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, EventArgs e)
        {

        }
        private void logo_Paint(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void SetInitials()
        {
            SetHiddens();
            menu.Hide();
            title.Width = Width;
            label1.Left = (ClientSize.Width - label1.Size.Width) / 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uId.Text) || string.IsNullOrEmpty(psswd.Text) ||
                uId.Text.Equals("Documento de identidad") || psswd.Text.Equals("Contraseña"))
            {
                MessageBox.Show(@"Ingrese su documento de identidad y contraseña.");
                return;
            }
            var usuario = _sesion.IniciarSesion(uId.Text, psswd.Text);
            if (usuario != null)
            {
                if (!usuario.Activo)
                {
                    MessageBox.Show(@"Usuario inactivo, contacte al administrador.");
                    return;
                }
                login.Hide();
                menu.Show();
                switch (usuario.Rol)
                {
                    case "Administrador":
                        admin_panel.Show();
                        break;
                    default:
                        estudiante_panel.Show();
                        if (!_sesion.RevisarVotoEstudiante(usuario.Id)) votar.Show();
                        if (_sesion.RevisarEstudianteEsCandidato(usuario.Id)) agregarPropuesta.Show();
                        break;
                }
                Sesion.UsuarioLogueado = usuario;
                return;
            }
            MessageBox.Show(@"Documento de identidad y/o contraseña invalidos.");
        }

        private void crear_usuario_Click(object sender, EventArgs e)
        {
            SetHiddens();
            c_cursos.DataSource = _generales.ObtenerCursos().Select(x => string.Concat(x.Id, ". ", x.Nombre)).ToList();
            c_roles.DataSource = _generales.ObtenerRoles().Select(x => string.Concat(x.Id, ". ", x.Nombre)).ToList();
            crear_usuario_panel.Show();
        }

        private void crearUsuarioBtn_Click(object sender, EventArgs e)
        {
            var valid = true;
            foreach (Control c in crear_usuario_panel.Controls)
            {
                if (!(c is TextBox)) continue;
                var textBox = c as TextBox;
                if (string.IsNullOrEmpty(textBox.Text)) valid = false;
            }
            if (!valid)
            {
                MessageBox.Show(@"Llene todos los campos.");
                return;
            }
            var regex = new Regex(@"/[a-zA-Z]+/");
            if (!regex.Match(c_name.Text).Success || !regex.Match(c_last.Text).Success || !regex.Match(c_email.Text).Success)
            {
                MessageBox.Show(@"Error en el formato de los campos de texto.");
                return;
            }
            regex = new Regex(@"^\d$");
            if (!regex.Match(c_doc.Text).Success)
            {
                MessageBox.Show(@"Error en el formato del documento.");
                return;
            }
            var crear = _sesion.CrearUsuario(c_name.Text, c_last.Text, c_doc.Text, c_date.Value.ToString("yyyy-MM-dd"), c_email.Text, c_pass.Text,
                Convert.ToInt32(c_roles.Text.Split('.')[0]), Convert.ToInt32(c_cursos.Text.Split('.')[0]));
            if (crear)
            {
                MessageBox.Show(@"usuario creado satisfactoriamente.");
                foreach (Control c in crear_usuario_panel.Controls)
                {
                    if (!(c is TextBox)) continue;
                    var textBox = c as TextBox;
                    textBox.Text = string.Empty;
                }
                return;
            }
            MessageBox.Show(@"Error creando el usuario, intente nuevamente.");
        }

        private void crear_usuario_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bc_buscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bc_doc.Text))
            {
                MessageBox.Show(@"Llene todos los campos.");
                return;
            }
            var usuario = _sesion.BuscarUsuario(bc_doc.Text);
            if (usuario == null)
            {
                MessageBox.Show(@"El usuario no existe.");
                return;
            }
            var valid = _generales.ObtenerCurso(usuario.Curso).Valido;
            if (!valid)
            {
                MessageBox.Show(@"El usuario no pertenece a un curso valido para votación.");
                return;
            }
            bd_grid.Rows.Add(usuario.Id, usuario.Nombres, usuario.Apellidos, usuario.Curso, usuario.Documento, usuario.Email);
            bd_grid.Show();
            bc_inscribir.Show();
        }

        private void insertarCandidato_Click(object sender, EventArgs e)
        {
            SetHiddens();
            buscar_candidato_panel.Show();
        }

        private void bc_inscribir_Click(object sender, EventArgs e)
        {
            var user = bd_grid.Rows[0].Cells[0].Value;
            var inscribir = _generales.CrearCandidato(Convert.ToInt32(user));
            if (inscribir)
            {
                MessageBox.Show(@"usuario inscrito.");
                bd_grid.Hide();
                bc_inscribir.Hide();
                bc_doc.Text = string.Empty;
                return;
            }
            MessageBox.Show(@"Error en la inscripción.");
        }

        private void verResultados_Click(object sender, EventArgs e)
        {
            SetHiddens();
            var results = _sesion.ObtenerVotos();
            foreach (var result in results)
            {
                votos_grid.Rows.Add(result.Nombres, result.Apellidos, result.Curso, result.Documento, result.Email,
                    result.Votos);
            }
            resultados_panel.Show();
        }

        private void votar_Click(object sender, EventArgs e)
        {
            SetHiddens();
            es_vt_listacandidatos.DataSource = _sesion.GetCandidatos().Select(x => string.Concat(x.Id, ". ", x.Nombres, " ", x.Apellidos)).ToList();
            Votar_panel.Show();
        }

        private void resultados_est_Click(object sender, EventArgs e)
        {
            SetHiddens();
            var results = _sesion.ObtenerVotos();
            foreach (var result in results)
            {
                votos_grid.Rows.Add(result.Nombres, result.Apellidos, result.Curso, result.Documento, result.Email,
                    result.Votos);
            }
            resultados_panel.Show();
        }

        private void es_propuesta_crear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(es_propuesta.Text) || string.IsNullOrEmpty(es_propuesta_descripcion.Text))
            {
                MessageBox.Show(@"Llene todos los campos.");
                return;
            }
            var proposal = _generales.CrearPropuesta(Sesion.UsuarioLogueado.Id, es_propuesta.Text, es_propuesta_descripcion.Text);
            if (proposal)
            {
                MessageBox.Show(@"Propuesta agregada correctamente");
                es_propuesta.Text = "";
                es_propuesta_descripcion.Text = "";
                return;
            }
            MessageBox.Show(@"Error ingresando la propuesta.");
        }

        private void agregarPropuesta_Click(object sender, EventArgs e)
        {
            SetHiddens();
            agregarPropuesta_panel.Show();
        }

        private void verCandidatos_Click(object sender, EventArgs e)
        {
            SetHiddens();
            es_listaCandidatos.DataSource = _sesion.GetCandidatos().Select(x => string.Concat(x.Id, ". ", x.Nombres, " ", x.Apellidos)).ToList();
            verCandidatos_panel.Show();
        }

        private void verPropuestas_Click(object sender, EventArgs e)
        {
            SetHiddens();

            var candidato = Convert.ToInt32(es_listaCandidatos.Text.Split('.')[0]);
            var propuestas = _generales.ObtenerPropuestas(candidato);
            foreach (var propuesta in propuestas)
            {
                propuestasGrid.Rows.Add(propuesta.NombrePropuesta, propuesta.Descripcion);
            }
            Propuestas_panel.Show();
        }

        private void es_votar_Click(object sender, EventArgs e)
        {
            var candidato = Convert.ToInt32(es_vt_listacandidatos.Text.Split('.')[0]);
            var voto = _sesion.Votar(Sesion.UsuarioLogueado.Id, candidato);
            if (voto)
            {
                MessageBox.Show(@"Voto válido, gracias por participar");
                SetHiddens();
                votar.Hide();
                return;
            }
            MessageBox.Show(@"Error relizando el voto, vuelva a intentar.");
        }

        private void SetHiddens()
        {
            verCandidatos_panel.Hide();
            crear_usuario_panel.Hide();
            Votar_panel.Hide();
            Propuestas_panel.Hide();
            agregarPropuesta_panel.Hide();
            buscar_candidato_panel.Hide();
            resultados_panel.Hide();
        }

        private void editarUsuario_Click(object sender, EventArgs e)
        {
            SetHiddens();
            UsersGeneral.Show();
            var usr = _sesion.ListaEstudiantes();
            foreach (var est in usr)
            {
                allEst.Rows.Add(est.Id, est.Nombres, est.Apellidos, est.Curso, est.Documento, est.Email);
            }
        }

        private void elmSaveGen_Click(object sender, EventArgs e)
        {
            var rows = allEst.Rows;
            for (var i = 0; i < rows.Count; i++)
            {
                try
                {
                    var slt = rows[i].Cells[0].Value;
                    var name = rows[i].Cells[1].Value.ToString();
                    var lname = rows[i].Cells[2].Value.ToString();
                    var doc = rows[i].Cells[4].Value.ToString();
                    var course = rows[i].Cells[3].Value.ToString();
                    var email = rows[i].Cells[5].Value.ToString();
                    if (!string.IsNullOrEmpty(slt.ToString()) &&
                        !string.IsNullOrEmpty(name) &&
                        !string.IsNullOrEmpty(lname) &&
                        !string.IsNullOrEmpty(doc) &&
                        !string.IsNullOrEmpty(course) &&
                        !string.IsNullOrEmpty(email))
                        Task.Factory.StartNew(() => _sesion.EditUser((int)slt, name, lname, doc, course, email));
                    else MessageBox.Show(@"Todos los datos deben estar llenos.");
                }
                catch (Exception exception)
                {
                    //
                }
            }
        }

        private void elmUserGen_Click(object sender, EventArgs e)
        {
            var rows = allEst.SelectedRows;
            for (var i = 0; i < rows.Count; i++)
            {
                try
                {
                    var slt = rows[i].Cells[0].Value;
                    Task.Factory.StartNew(() => _sesion.DeleteUser((int)slt));
                }
                catch (Exception exception)
                {
                    //
                }
            }
        }

        private void logo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sessionOut_Click(object sender, EventArgs e)
        {
            SetHiddens();
            menu.Hide();
            login.Show();
        }
    }
}