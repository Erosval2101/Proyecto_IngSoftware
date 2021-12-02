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
    public partial class MenuVendedor : Form
    {
        String id_vendedor;
        public MenuVendedor(String id)
        {
            InitializeComponent();
            id_vendedor = id;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Inicio_sesion regresar = new Inicio_sesion();
                regresar.Show();
                this.Hide();
            }
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

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;

            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MenuVendedor_Load(object sender, EventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            String query = "select Nombre_Empleado,Ap_Paterno,Ap_Materno,ImagenEmpleado from empleado where IDEmpleado = '" + id_vendedor + "'";
            MySqlCommand comando = new MySqlCommand(query, conectar);
            MySqlDataAdapter da = new MySqlDataAdapter(comando);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            lblNombre.Text = tabla.Rows[0][0].ToString() + " " + tabla.Rows[0][1].ToString() + " " + tabla.Rows[0][2].ToString(); ;

            byte[] img = (byte[])tabla.Rows[0][3];

            MemoryStream ms = new MemoryStream(img);

            ptbUsuario.Image = Image.FromStream(ms);

            da.Dispose();
        }

        private void registrarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrarVenta ver = new RegistrarVenta(id_vendedor);
            ver.Show();
            this.Hide();
        }

        private void buscarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsultarVenta ver = new ConsultarVenta(id_vendedor);
            ver.Show();
            this.Hide();
        }

        private void registrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReporteVentas ver = new ReporteVentas(id_vendedor);
            ver.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Inicio_sesion regresar = new Inicio_sesion();
                regresar.Show();
                this.Hide();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Inicio_sesion regresar = new Inicio_sesion();
                regresar.Show();
                this.Hide();
            }
        }
    }
}
