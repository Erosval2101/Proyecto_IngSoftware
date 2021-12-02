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
    public partial class ReporteVentas : Form
    {
        String id_vendedor;
        public ReporteVentas(String id)
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
            MenuVendedor regresar = new MenuVendedor(id_vendedor);
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

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MenuVendedor regresar = new MenuVendedor(id_vendedor);
            regresar.Show();
            this.Hide();
        }

        private void ReporteVentas_Load(object sender, EventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select * from venta where ID_Empleado = '" + id_vendedor + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            while (leerConsulta.Read())
            {
                String nombre_vendedor, nombre_cliente, tipo_pago;
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dtgvVenta);
                fila.Cells[0].Value = leerConsulta.GetString(0); //ID Venta
                fila.Cells[1].Value = leerConsulta.GetString(1); // ID Vendedor
                nombre_vendedor = buscar_NombreVendedor(leerConsulta.GetString(1));
                fila.Cells[2].Value = nombre_vendedor; // Nombre Vendedor
                fila.Cells[3].Value = leerConsulta.GetString(3); //Fecha Venta
                fila.Cells[4].Value = leerConsulta.GetString(4); //Tipo Pago
                fila.Cells[5].Value = leerConsulta.GetString(5); //Monto
                tipo_pago = leerConsulta.GetString(4);
                if (tipo_pago == "Efectivo" || tipo_pago == "Tarjeta Bancaria")
                {
                    fila.Cells[6].Value = ""; //ID Cliente
                    fila.Cells[7].Value = ""; //Nombre Cliente
                }
                else
                {
                    fila.Cells[6].Value = leerConsulta.GetString(2); //ID Cliente
                    nombre_cliente = buscar_NombreCliente(leerConsulta.GetString(2));
                    fila.Cells[7].Value = nombre_cliente; //Nombre Cliente
                }

                dtgvVenta.Rows.Add(fila);
            }
            conectar.Close();
        }

        private void buscar_Detalle(String id)
        {
            dtgvDetalle.Rows.Clear();
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select * from detalle_venta where ID_Venta = '" + id + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            while (leerConsulta.Read())
            {
                String descripcion;
                DataGridViewRow fila = new DataGridViewRow();
                fila.CreateCells(dtgvDetalle);
                fila.Cells[0].Value = leerConsulta.GetString(0); //ID Venta
                fila.Cells[1].Value = leerConsulta.GetString(1); //Cantidad
                fila.Cells[2].Value = leerConsulta.GetString(2); //Precio
                fila.Cells[3].Value = leerConsulta.GetString(3); //ID Producto
                descripcion = buscar_DescripcionProducto(leerConsulta.GetString(3));
                fila.Cells[4].Value = descripcion;

                dtgvDetalle.Rows.Add(fila);
            }
            conectar.Close();
        }

        public static String buscar_NombreVendedor(String id)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select Nombre_Empleado, Ap_Paterno from empleado where IDEmpleado = '" + id + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                String nombre_vendedor;
                nombre_vendedor = leerConsulta.GetString(0) + " " + leerConsulta.GetString(1);
                return nombre_vendedor;
            }
            else
            {
                String nombre_vendedor = "";
                return nombre_vendedor;
            }
            conectar.Close();
        }

        private void dtgvVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            String id_venta = this.dtgvVenta.CurrentRow.Cells[0].Value.ToString();
            buscar_Detalle(id_venta);
        }

        public static String buscar_DescripcionProducto(String id)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select Descripcion from producto where IDProducto = '" + id + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                String descp;
                descp = leerConsulta.GetString(0);
                return descp;
            }
            else
            {
                String descp = "";
                return descp;
            }
            conectar.Close();
        }

        public static String buscar_NombreCliente(String id)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select NombreCliente, Ap_PaternoCliente from cliente where IDCliente = '" + id + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                String nombre_cliente;
                nombre_cliente = leerConsulta.GetString(0) + " " + leerConsulta.GetString(1);
                return nombre_cliente;
            }
            else
            {
                String nombre_cliente = "";
                return nombre_cliente;
            }
            conectar.Close();
        }
    }
}
