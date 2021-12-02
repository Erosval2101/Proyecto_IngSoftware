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

namespace ProyectoIngSoftware
{
    public partial class Inicio_sesion : Form
    {
        public Inicio_sesion()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Inicio_sesion_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void txtUsuario_Enter(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "RFC del Empleado")
            {
                txtUsuario.Text = "";
            }
        }

        private void txtUsuario_Leave(object sender, EventArgs e)
        {
            if (txtUsuario.Text == "")
            {
                txtUsuario.Text = "RFC del Empleado";
            }
        }

        private void txtContrasena_Enter(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "Contraseña")
            {
                txtContrasena.Text = "";
                txtContrasena.UseSystemPasswordChar = true;
            }
        }

        private void txtContrasena_Leave(object sender, EventArgs e)
        {
            if (txtContrasena.Text == "")
            {
                txtContrasena.Text = "Contraseña";
                txtContrasena.UseSystemPasswordChar = false;
            }
        }
        private void mensaje_Error(string msg)
        {
            lblError.Text = msg;
            lblError.Visible = true;
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            if (txtUsuario.Text != "Usuario")
            {
                if (txtContrasena.Text != "Contraseña")
                {
                    MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                    conectar.Open();

                    MySqlCommand inicio_sesion = new MySqlCommand();
                    MySqlConnection conectarnos = new MySqlConnection();
                    inicio_sesion.Connection = conectar;

                    inicio_sesion.CommandText = ("select * from empleado where IDEmpleado = '" + txtUsuario.Text + "'and Contrasena = '" + txtContrasena.Text + "'");

                    MySqlDataReader leerConsulta = inicio_sesion.ExecuteReader();
                    String puesto;
                    if (leerConsulta.Read())
                    {
                        puesto = leerConsulta.GetString(4);
                        if (puesto == "Gerente")
                        {
                            MenuGeneral llamar = new MenuGeneral();
                            llamar.Show();
                            this.Hide();
                        }
                        if (puesto == "Vendedor")
                        {
                            MenuVendedor llamar = new MenuVendedor(leerConsulta.GetString(0));
                            llamar.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        mensaje_Error("Usuario o contraseña incorrecta");
                    }
                    conectar.Close();
                }
                else
                {
                    mensaje_Error("Por favor, ingrese contraseña");
                }
            }
            else
            {
                mensaje_Error("Por favor, ingrese usuario");
            }
        }
    }
}
