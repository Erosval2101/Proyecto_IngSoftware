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
    public partial class ModificarCliente : Form
    {
        public ModificarCliente()
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

            buscar.CommandText = ("select * from prouducto where IDProducto = '" + txtID.Text + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                conectar.Close();

                groupBoxCliente.Visible = true;
                ptbImagen.Visible = true;
                btnExaminar.Visible = true;
                btnLimpiar.Visible = true;
                btnModificar.Visible = true;

                MySqlConnection conectar_nuevo = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                conectar.Open();

                String query = "select * from cliente where IDCliente = '" + txtID.Text + "'";
                MySqlCommand comando = new MySqlCommand(query, conectar_nuevo);
                MySqlDataAdapter da = new MySqlDataAdapter(comando);
                DataTable tabla = new DataTable();
                da.Fill(tabla);
                txtNombre.Text = tabla.Rows[0][1].ToString();
                txtApPat.Text = tabla.Rows[0][2].ToString();
                txtApMat.Text = tabla.Rows[0][3].ToString();
                txtTelefono.Text = tabla.Rows[0][4].ToString();
                txtCorreo.Text = tabla.Rows[0][5].ToString();
                txtCredito.Text = tabla.Rows[0][6].ToString();

                byte[] img = (byte[])tabla.Rows[0][7];

                MemoryStream ms = new MemoryStream(img);

                ptbImagen.Image = Image.FromStream(ms);

                da.Dispose();
                conectar.Close();
            }
            else
            {
                conectar.Close();
                MessageBox.Show("No se encontró el cliente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                groupBoxCliente.Visible = false;
                ptbImagen.Visible = false;
                btnExaminar.Visible = false;
                btnLimpiar.Visible = false;
                btnModificar.Visible = false;
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
            txtTelefono.Text = "";
            txtCorreo.Text = "";
            txtCredito.Text = "";
            if (ptbImagen.Image != null)
            {
                ptbImagen.Image.Dispose();
                ptbImagen.Image = null;
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
            {
                if (txtApPat.Text != "")
                {
                    if (txtApMat.Text != "")
                    {
                        if (txtApPat.Text != "")
                        {
                            if (txtTelefono.Text != "")
                            {
                                if (txtCorreo.Text != "")
                                {
                                    if (txtCredito.Text != "")
                                    {
                                        MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                                        conectar.Open();

                                        MemoryStream ms = new MemoryStream();
                                        ptbImagen.Image.Save(ms, ptbImagen.Image.RawFormat);
                                        byte[] img = ms.ToArray();

                                        String query = "update cliente set NombreCliente=@nombre, Ap_PaternoCliente=@ap_pat, Ap_MaternoCliente=@ap_mat, Numero_Telefono=@telefono, CorreoElectronico=@correo, CreditoDisponible=@credito, ImagenCliente=@img where IDCliente='" + txtID.Text + "'";

                                        MySqlCommand registrar = new MySqlCommand(query, conectar);

                                        registrar.Parameters.Add("@nombre", MySqlDbType.VarChar, 50);
                                        registrar.Parameters.Add("@ap_pat", MySqlDbType.VarChar, 30);
                                        registrar.Parameters.Add("@ap_mat", MySqlDbType.VarChar, 30);
                                        registrar.Parameters.Add("@telefono", MySqlDbType.VarChar, 10);
                                        registrar.Parameters.Add("@correo", MySqlDbType.Text);
                                        registrar.Parameters.Add("@credito", MySqlDbType.Float);
                                        registrar.Parameters.Add("@img", MySqlDbType.Blob);

                                        registrar.Parameters["@nombre"].Value = txtNombre.Text;
                                        registrar.Parameters["@ap_pat"].Value = txtApPat.Text;
                                        registrar.Parameters["@ap_mat"].Value = txtApMat;
                                        registrar.Parameters["@telefono"].Value = txtTelefono.Text;
                                        registrar.Parameters["@correo"].Value = txtCorreo.Text;
                                        registrar.Parameters["@credito"].Value = txtCredito.Text;
                                        registrar.Parameters["@img"].Value = img;


                                        if (registrar.ExecuteNonQuery() == 1)
                                        {
                                            MessageBox.Show("Producto agregado exitosamente");
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MenuGeneral regresar = new MenuGeneral();
            regresar.Show();
            this.Hide();
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
