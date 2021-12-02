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

namespace ProyectoIngSoftware
{
    public partial class MenuGeneral : Form
    {
        public MenuGeneral()
        {
            InitializeComponent();
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

        private void registrarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrarEmpleado ver = new RegistrarEmpleado();
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

        private void buscarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuscarEmpleado ver = new BuscarEmpleado();
            ver.Show();
            this.Hide();
        }

        private void modificarEmpleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModificarEmpleado ver = new ModificarEmpleado();
            ver.Show();
            this.Hide();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportesEmpleado ver = new ReportesEmpleado();
            ver.Show();
            this.Hide();
        }

        private void registrarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrarProducto ver = new RegistrarProducto();
            ver.Show();
            this.Hide();
        }

        private void buscarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuscarProducto ver = new BuscarProducto();
            ver.Show();
            this.Hide();
        }

        private void modificarProductoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModificarProducto ver = new ModificarProducto();
            ver.Show();
            this.Hide();
        }

        private void reportesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ReportesProducto ver = new ReportesProducto();
            ver.Show();
            this.Hide();
        }

        private void registrarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistrarCliente ver = new RegistrarCliente();
            ver.Show();
            this.Hide();
        }

        private void buscarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BuscarCliente ver = new BuscarCliente();
            ver.Show();
            this.Hide();
        }

        private void reportesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ReportesCliente ver = new ReportesCliente();
            ver.Show();
            this.Hide();
        }

        private void modificarClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModificarCliente ver = new ModificarCliente();
            ver.Show();
            this.Hide();
        }

        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportesVentasGerente ver = new ReportesVentasGerente();
            ver.Show();
            this.Hide();
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
    }
}
