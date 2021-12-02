using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;
using System.IO;

namespace ProyectoIngSoftware
{
    public partial class ModificarEmpleado : Form
    {
        public ModificarEmpleado()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            MenuGeneral regresar = new MenuGeneral();
            regresar.Show();
            this.Hide();
        }

        int lx, ly;
        int sw, sh;
        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;

            btnRestaurar.Visible = true;
            btnMaximizar.Visible = false;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;

            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Length > 13 || txtID.Text.Length < 13)
            {
                MessageBox.Show("RFC no válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                conectar.Open();

                MySqlCommand buscar = new MySqlCommand();
                MySqlConnection conectarnos = new MySqlConnection();
                buscar.Connection = conectar;

                buscar.CommandText = ("select * from empleado where IDEmpleado = '" + txtID.Text + "'");

                MySqlDataReader leerConsulta = buscar.ExecuteReader();
                if (leerConsulta.Read())
                {
                    conectar.Close();

                    groupBoxEmpleado.Visible = true;
                    ptbImagen.Visible = true;
                    btnExaminar.Visible = true;
                    btnLimpiar.Visible = true;
                    btnModificar.Visible = true;

                    MySqlConnection conectar_nuevo = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                    conectar.Open();

                    String query = "select * from empleado where IDEmpleado = '" + txtID.Text + "'";
                    MySqlCommand comando = new MySqlCommand(query, conectar_nuevo);
                    MySqlDataAdapter da = new MySqlDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    da.Fill(tabla);
                    txtNombre.Text = tabla.Rows[0][1].ToString();
                    txtApPat.Text = tabla.Rows[0][2].ToString();
                    txtApMat.Text = tabla.Rows[0][3].ToString();
                    txtPuesto.Text = tabla.Rows[0][4].ToString();
                    dateTimePicker1.Text = tabla.Rows[0][5].ToString();
                    dateTimePicker2.Text = tabla.Rows[0][6].ToString();
                    txtTelefono.Text = tabla.Rows[0][7].ToString();
                    txtCorreo.Text = tabla.Rows[0][8].ToString();
                    txtContrasena.Text = tabla.Rows[0][9].ToString();
                    txtSalario.Text = tabla.Rows[0][10].ToString();

                    byte[] img = (byte[])tabla.Rows[0][11];

                    MemoryStream ms = new MemoryStream(img);

                    ptbImagen.Image = Image.FromStream(ms);

                    da.Dispose();
                }
                else
                {
                    conectar.Close();
                    MessageBox.Show("No se encontró el empleado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    groupBoxEmpleado.Visible = false;
                    ptbImagen.Visible = false;
                    btnExaminar.Visible = false;
                    btnLimpiar.Visible = false;
                    btnModificar.Visible = false;
                }
            }
        }

        public OpenFileDialog examinar = new OpenFileDialog();
        private void btnExaminar_Click(object sender, EventArgs e)
        {
            examinar.Filter = "Archivos de Imagen |*.jpg; *.png;";
            DialogResult r = examinar.ShowDialog();
            if (r == DialogResult.Abort)
            {
                return;
            }
            if (r == DialogResult.Cancel)
            {
                return;
            }
            ptbImagen.Image = Image.FromFile(examinar.FileName);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtApPat.Text = "";
            txtApMat.Text = "";
            txtPuesto.Text = "";
            dateTimePicker1.Text = "";
            dateTimePicker2.Text = "";
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtContrasena.Text = "";
            txtSalario.Text = "";
            if (ptbImagen.Image != null)
            {
                ptbImagen.Image.Dispose();
                ptbImagen.Image = null;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtID.Text != "" && txtID.Text.Length == 13)
            {
                if (txtNombre.Text != "")
                {
                    if (txtApPat.Text != "")
                    {
                        if (txtApMat.Text != "")
                        {
                            if (txtPuesto.Text != "")
                            {
                                if (txtTelefono.Text != "")
                                {
                                    if (txtCorreo.Text != "")
                                    {
                                        if (txtContrasena.Text != "")
                                        {
                                            if (txtSalario.Text != "")
                                            {

                                                MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                                                conectar.Open();

                                                MemoryStream ms = new MemoryStream();
                                                ptbImagen.Image.Save(ms, ptbImagen.Image.RawFormat);
                                                byte[] img = ms.ToArray();

                                                String query = "update empleado set @id=IDEmpleado, Nombre_Empleado=@nombre, Ap_Paterno=@ap_pat, Ap_Materno=@ap_mat, Puesto=@puesto, H_Entrada=@h_ent, H_Salida=@h_sal, Numero_Telefono=@num_tel, CorreoElectronico=@correo, Contrasena=@contrasena, Salario=@salario, ImagenEmpleado=@img where IDEmpleado=@id";

                                                MySqlCommand registrar = new MySqlCommand(query, conectar);

                                                registrar.Parameters.Add("@id", MySqlDbType.VarChar, 13);
                                                registrar.Parameters.Add("@nombre", MySqlDbType.VarChar, 50);
                                                registrar.Parameters.Add("@ap_pat", MySqlDbType.VarChar, 30);
                                                registrar.Parameters.Add("@ap_mat", MySqlDbType.VarChar, 30);
                                                registrar.Parameters.Add("@puesto", MySqlDbType.VarChar, 30);
                                                registrar.Parameters.Add("@h_ent", MySqlDbType.Time);
                                                registrar.Parameters.Add("@h_sal", MySqlDbType.Time);
                                                registrar.Parameters.Add("@num_tel", MySqlDbType.VarChar, 10);
                                                registrar.Parameters.Add("@correo", MySqlDbType.Text);
                                                registrar.Parameters.Add("@contrasena", MySqlDbType.VarChar, 15);
                                                registrar.Parameters.Add("@salario", MySqlDbType.Int32, 11);
                                                registrar.Parameters.Add("@img", MySqlDbType.Blob);

                                                registrar.Parameters["@id"].Value = txtID.Text;
                                                registrar.Parameters["@nombre"].Value = txtNombre.Text;
                                                registrar.Parameters["@ap_pat"].Value = txtApPat.Text;
                                                registrar.Parameters["@ap_mat"].Value = txtApMat.Text;
                                                registrar.Parameters["@puesto"].Value = txtPuesto.Text;
                                                registrar.Parameters["@h_ent"].Value = TimeSpan.Parse(dateTimePicker1.Text);
                                                registrar.Parameters["@h_sal"].Value = TimeSpan.Parse(dateTimePicker2.Text);
                                                registrar.Parameters["@num_tel"].Value = txtTelefono.Text;
                                                registrar.Parameters["@correo"].Value = txtCorreo.Text;
                                                registrar.Parameters["@contrasena"].Value = txtContrasena.Text;
                                                registrar.Parameters["@salario"].Value = txtSalario.Text;
                                                registrar.Parameters["@img"].Value = img;


                                                if (registrar.ExecuteNonQuery() == 1)
                                                {
                                                    MessageBox.Show("Empleado agregado exitosamente");
                                                    groupBoxEmpleado.Visible = true;
                                                    ptbImagen.Visible = false;
                                                    btnExaminar.Visible = false;
                                                    btnLimpiar.Visible = false;
                                                    btnModificar.Visible = false;
                                                }
                                                conectar.Close();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Llene adecuadamente los campos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MenuGeneral regresar = new MenuGeneral();
            regresar.Show();
            this.Hide();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            MenuGeneral regresar = new MenuGeneral();
            regresar.Show();
            this.Hide();
        }

        private void ModificarEmpleado_Load(object sender, EventArgs e)
        {

        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
