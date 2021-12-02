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
    public partial class ModificarProducto : Form
    {
        public ModificarProducto()
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
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            MySqlConnection conectarnos = new MySqlConnection();
            buscar.Connection = conectar;

            buscar.CommandText = ("select * from producto where IDProducto = '" + txtIDBuscar.Text + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {

                groupBoxProducto.Visible = true;
                ptbImagen.Visible = true;
                btnExaminar.Visible = true;
                btnLimpiar.Visible = true;
                btnModificar.Visible = true;

                inyectar_datos();
            }
            else
            {
                conectar.Close();
                MessageBox.Show("No se encontró el producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                groupBoxProducto.Visible = false;
                ptbImagen.Visible = false;
                btnExaminar.Visible = false;
                btnLimpiar.Visible = false;
                btnModificar.Visible = false;
            }
        }

        private void inyectar_datos()
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            String query = "select * from producto where IDProducto = '" + txtIDBuscar.Text + "'";
            MySqlCommand comando = new MySqlCommand(query, conectar);
            MySqlDataAdapter da = new MySqlDataAdapter(comando);
            DataTable tabla = new DataTable();
            da.Fill(tabla);
            txtDescripcion.Text = tabla.Rows[0][1].ToString();
            txtPrecio.Text = tabla.Rows[0][2].ToString();
            cmbDisp.Text = tabla.Rows[0][3].ToString();
            txtExistencias.Text = tabla.Rows[0][4].ToString();
            txtNoProd.Text = tabla.Rows[0][6].ToString();
            txtColor.Text = tabla.Rows[0][7].ToString();
            txtModelo.Text = tabla.Rows[0][8].ToString();

            byte[] img = (byte[])tabla.Rows[0][9];

            MemoryStream ms = new MemoryStream(img);

            ptbImagen.Image = Image.FromStream(ms);

            da.Dispose();
            conectar.Close();
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
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
            cmbDep.SelectedIndex = -1;
            cmbDisp.SelectedIndex = -1;
            txtExistencias.Text = "";
            txtNoProd.Text = "";
            txtColor.Text = "";
            txtModelo.Text = "";
            if (ptbImagen.Image != null)
            {
                ptbImagen.Image.Dispose();
                ptbImagen.Image = null;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtDescripcion.Text != "")
            {
                if (txtPrecio.Text != "")
                {
                    if (cmbDisp.SelectedIndex != -1)
                    {
                        if (txtExistencias.Text != "")
                        {
                            if (cmbDep.SelectedIndex != -1)
                            {
                                if (txtNoProd.Text != "")
                                {
                                    if (txtColor.Text != "")
                                    {
                                        if (txtModelo.Text != "")
                                        {
                                            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                                            conectar.Open();

                                            MemoryStream ms = new MemoryStream();
                                            ptbImagen.Image.Save(ms, ptbImagen.Image.RawFormat);
                                            byte[] img = ms.ToArray();

                                            String query = "update producto set Descripcion=@descp, Precio=@precio, Disponible=@disponible, Existencias=@existencias, IDDepartamento=@iddep, NoProducto=@noprod, Color=@color, Modelo=@modelo, ImagenProducto=@img where IDProducto='"+ txtID.Text +"'";

                                            MySqlCommand registrar = new MySqlCommand(query, conectar);

                                            registrar.Parameters.Add("@descp", MySqlDbType.Text);
                                            registrar.Parameters.Add("@precio", MySqlDbType.Float);
                                            registrar.Parameters.Add("@disponible", MySqlDbType.Enum);
                                            registrar.Parameters.Add("@existencias", MySqlDbType.Int32, 11);
                                            registrar.Parameters.Add("@iddep", MySqlDbType.Int32, 11);
                                            registrar.Parameters.Add("@noprod", MySqlDbType.Int32, 11);
                                            registrar.Parameters.Add("@color", MySqlDbType.VarChar, 30);
                                            registrar.Parameters.Add("@modelo", MySqlDbType.VarChar, 30);
                                            registrar.Parameters.Add("@img", MySqlDbType.Blob);

                                            int id_dep = cmbDep.SelectedIndex + 1;
                                            registrar.Parameters["@descp"].Value = txtDescripcion.Text;
                                            registrar.Parameters["@precio"].Value = txtPrecio.Text;
                                            registrar.Parameters["@disponible"].Value = cmbDisp.SelectedItem.ToString();
                                            registrar.Parameters["@existencias"].Value = txtExistencias.Text;
                                            registrar.Parameters["@iddep"].Value = id_dep.ToString();
                                            registrar.Parameters["@noprod"].Value = txtNoProd.Text;
                                            registrar.Parameters["@color"].Value = txtColor.Text;
                                            registrar.Parameters["@modelo"].Value = txtModelo.Text;
                                            registrar.Parameters["@img"].Value = img;


                                            if (registrar.ExecuteNonQuery() == 1)
                                            {
                                                MessageBox.Show("Producto agregado exitosamente");
                                            }
                                            conectar.Close();
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
