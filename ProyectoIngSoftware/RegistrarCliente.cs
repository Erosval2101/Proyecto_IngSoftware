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
    public partial class RegistrarCliente : Form
    {
        public RegistrarCliente()
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

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
            {
                if (txtApPat.Text != "")
                {
                    if (txtApMat.Text != "")
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

                                    String query = "insert into cliente(NombreCliente, Ap_PaternoCliente, Ap_MaternoCliente, Numero_Telefono, CorreoElectronico, CreditoDisponible, ImagenCliente) values (@nombre, @ap_pat, @ap_mat, @telefono, @correo, @credito, @img)";

                                    MySqlCommand registrar = new MySqlCommand(query, conectar);

                                    registrar.Parameters.Add("@nombre", MySqlDbType.VarChar, 50);
                                    registrar.Parameters.Add("@ap_pat", MySqlDbType.VarChar, 30);
                                    registrar.Parameters.Add("@ap_mat", MySqlDbType.VarChar, 30);
                                    registrar.Parameters.Add("@telefono", MySqlDbType.VarChar, 10);
                                    registrar.Parameters.Add("@correo", MySqlDbType.Text);
                                    registrar.Parameters.Add("@credito", MySqlDbType.Float);
                                    registrar.Parameters.Add("@img", MySqlDbType.LongBlob);

                                    registrar.Parameters["@nombre"].Value = txtNombre.Text;
                                    registrar.Parameters["@ap_pat"].Value = txtApPat.Text;
                                    registrar.Parameters["@ap_mat"].Value = txtApMat.Text;
                                    registrar.Parameters["@telefono"].Value = txtTelefono.Text;
                                    registrar.Parameters["@correo"].Value = txtCorreo.Text;
                                    registrar.Parameters["@credito"].Value = txtCredito.Text;
                                    registrar.Parameters["@img"].Value = img;


                                    if (registrar.ExecuteNonQuery() == 1)
                                    {
                                         MessageBox.Show("Cliente agregado exitosamente");
                                         int contID = Convert.ToInt32(txtID.Text) + 1;
                                         txtID.Text = contID.ToString();
                                         btnLimpiar_Click(null, e);
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

        private void RegistrarCliente_Load(object sender, EventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            String query = "select count(*) from cliente";
            MySqlCommand consultar = new MySqlCommand(query, conectar);
            int contID = Convert.ToInt32(consultar.ExecuteScalar()) + 1;
            txtID.Text = contID.ToString();
            conectar.Close();
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
