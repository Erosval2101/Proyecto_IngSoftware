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
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace ProyectoIngSoftware
{
    public partial class RegistrarVenta : Form
    {
        String id_vendedor;

        double porcentaje_iva = 0.16;
        public RegistrarVenta(String id)
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

        private void RegistrarVenta_Load(object sender, EventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            String query = "select count(*) from venta";
            MySqlCommand consultar = new MySqlCommand(query, conectar);
            int contID = Convert.ToInt32(consultar.ExecuteScalar()) + 1;
            txtIDVenta.Text = contID.ToString();

            txtIDEmpleado.Text = id_vendedor;

            MySqlCommand consultar_empleado = new MySqlCommand();
            MySqlConnection conectarnos = new MySqlConnection();
            consultar_empleado.Connection = conectar;

            consultar_empleado.CommandText = ("select Nombre_Empleado, Ap_Paterno, Ap_Materno from empleado where IDEmpleado = '" + id_vendedor + "'");

            MySqlDataReader leerConsulta = consultar_empleado.ExecuteReader();
            String nombre_empleado;
            if (leerConsulta.Read())
            {
                nombre_empleado = leerConsulta.GetString(0) + " " + leerConsulta.GetString(1) + " " + leerConsulta.GetString(2);
                txtNombreEmpleado.Text = nombre_empleado;
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select IDProducto, Descripcion, Precio, NoProducto, Color, Modelo from producto where IDProducto = '" + txtIDProducto.Text + "' and Disponible = 'Si' and Existencias != '0'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                Boolean flag = false;
                foreach (DataGridViewRow r in dtgvProducto.Rows)
                {
                    String id_producto = Convert.ToString(r.Cells[0].Value);
                    if (leerConsulta.GetString(0) == id_producto)
                    {
                        int suma_cantidad = Convert.ToInt32(r.Cells[6].Value) + Convert.ToInt32(nupCantidad.Value);
                        r.Cells[6].Value = Convert.ToString(suma_cantidad);
                        decimal _importe = Convert.ToDecimal(suma_cantidad) * Convert.ToDecimal(leerConsulta.GetString(2));
                        r.Cells[7].Value = _importe.ToString();
                        flag = true;
                        txtSubtotal.Text = _importe.ToString();
                        decimal _iva = _importe * Convert.ToDecimal(porcentaje_iva);
                        txtIVA.Text = Convert.ToString(_iva);
                        txtTotal.Text = Convert.ToString(_iva + _importe);
                    }
                }
                if (flag == false)
                {
                    DataGridViewRow fila = new DataGridViewRow();
                    fila.CreateCells(dtgvProducto);
                    fila.Cells[0].Value = leerConsulta.GetString(0);
                    fila.Cells[1].Value = leerConsulta.GetString(1);
                    fila.Cells[2].Value = leerConsulta.GetString(2);
                    fila.Cells[3].Value = leerConsulta.GetString(3);
                    fila.Cells[4].Value = leerConsulta.GetString(4);
                    fila.Cells[5].Value = leerConsulta.GetString(5);
                    fila.Cells[6].Value = nupCantidad.Value.ToString();
                    decimal importe = nupCantidad.Value * Convert.ToDecimal(leerConsulta.GetString(2));
                    fila.Cells[7].Value = importe.ToString();

                    dtgvProducto.Rows.Add(fila);

                    decimal _importe = 0;
                    foreach (DataGridViewRow r in dtgvProducto.Rows)
                    {
                        _importe = _importe + Convert.ToDecimal(r.Cells[7].Value);
                    }
                    txtSubtotal.Text = _importe.ToString();
                    decimal _iva = _importe * Convert.ToDecimal(porcentaje_iva);
                    txtIVA.Text = Convert.ToString(_iva);
                    txtTotal.Text = Convert.ToString(_iva + _importe);
                }
            }
            else
            {
                MessageBox.Show("Hay problemas con el producto que quiere registrar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            conectar.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            dtgvProducto.Rows.Remove(dtgvProducto.CurrentRow);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtgvProducto.Rows.Clear();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MenuVendedor regresar = new MenuVendedor(id_vendedor);
            regresar.Show();
            this.Hide();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            String tipo_pago = cmbTipoPago.SelectedItem.ToString();
            Boolean comp_cantidad = false;
            if (tipo_pago == "Efectivo" || tipo_pago == "Tarjeta Bancaria")
            {
                foreach (DataGridViewRow r in dtgvProducto.Rows)
                {
                    Boolean comp = comprobar_cantidad(r.Cells[0].Value.ToString(), r.Cells[6].Value.ToString());
                    if(comp == false)
                    {
                        comp_cantidad = true;
                    }
                }

                if(comp_cantidad == false)
                {
                    MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                    conectar.Open();

                    String query = "insert into venta(ID_Empleado, ID_Cliente, FechaVenta, TipoPago, MontoPagar) values (@idempleado, @idcliente, @f_venta, @tipopago, @monto)";

                    MySqlCommand registrar = new MySqlCommand(query, conectar);

                    registrar.Parameters.Add("@idempleado", MySqlDbType.VarChar, 13);
                    registrar.Parameters.Add("@idcliente", MySqlDbType.Int32, 11);
                    registrar.Parameters.Add("@f_venta", MySqlDbType.DateTime);
                    registrar.Parameters.Add("@tipopago", MySqlDbType.Enum);
                    registrar.Parameters.Add("@monto", MySqlDbType.Float);

                    registrar.Parameters["@idempleado"].Value = id_vendedor;
                    registrar.Parameters["@idcliente"].Value = null;
                    registrar.Parameters["@f_venta"].Value = Convert.ToDateTime(txtFecha.Text);
                    registrar.Parameters["@tipopago"].Value = cmbTipoPago.SelectedItem.ToString();
                    registrar.Parameters["@monto"].Value = txtTotal.Text;

                    if (registrar.ExecuteNonQuery() == 1)
                    {
                        String query_detalle = "insert into detalle_venta(ID_Venta, Cantidad, Precio, ID_Producto) values (@idventa, @cantidad, @precio, @idprod)";

                        MySqlCommand registrar_detalle = new MySqlCommand(query_detalle, conectar);

                        foreach (DataGridViewRow r in dtgvProducto.Rows)
                        {
                            registrar_detalle.Parameters.Clear();
                            registrar_detalle.Parameters.Add("@idventa", MySqlDbType.Int32, 11);
                            registrar_detalle.Parameters.Add("@cantidad", MySqlDbType.Int32, 11);
                            registrar_detalle.Parameters.Add("@precio", MySqlDbType.Float);
                            registrar_detalle.Parameters.Add("@idprod", MySqlDbType.Int32, 11);

                            registrar_detalle.Parameters["@idventa"].Value = txtIDVenta.Text;
                            registrar_detalle.Parameters["@cantidad"].Value = r.Cells[6].Value;
                            registrar_detalle.Parameters["@precio"].Value = r.Cells[2].Value;
                            registrar_detalle.Parameters["@idprod"].Value = r.Cells[0].Value;
                            restar_existencias(r.Cells[0].Value.ToString(), r.Cells[6].Value.ToString());

                            registrar_detalle.ExecuteNonQuery();
                        }
                        MessageBox.Show("Venta generada exitosamente");
                        generarPDF();
                        int contID = Convert.ToInt32(txtIDVenta.Text) + 1;
                        txtIDVenta.Text = contID.ToString();
                        Limpiar();
                        btnLimpiar_Click(null, e);
                        conectar.Close();
                    }
                    else
                    {
                        MessageBox.Show("Error al momento de registrar la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        conectar.Close();
                    }
                }
                else
                {
                    MessageBox.Show("La cantidad de productos que ingresó es superior a la que se tiene en tienda", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (tipo_pago == "Crédito Tienda" && txtIDCliente.Text != "")
            {
                foreach (DataGridViewRow r in dtgvProducto.Rows)
                {
                    Boolean comp = comprobar_cantidad(r.Cells[0].Value.ToString(), r.Cells[6].Value.ToString());
                    if (comp == false)
                    {
                        comp_cantidad = true;
                    }
                }

                if (comp_cantidad == false)
                {
                    Boolean comp_credito = comprobar_credito(txtIDCliente.Text.ToString(), txtTotal.Text.ToString());

                    if (comp_credito == true)
                    {
                        MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
                        conectar.Open();

                        String query = "insert into venta(ID_Empleado, ID_Cliente, FechaVenta, TipoPago, MontoPagar) values (@idempleado, @idcliente, @f_venta, @tipopago, @monto)";

                        MySqlCommand registrar = new MySqlCommand(query, conectar);

                        registrar.Parameters.Add("@idempleado", MySqlDbType.VarChar, 13);
                        registrar.Parameters.Add("@idcliente", MySqlDbType.Int32, 11);
                        registrar.Parameters.Add("@f_venta", MySqlDbType.DateTime);
                        registrar.Parameters.Add("@tipopago", MySqlDbType.Enum);
                        registrar.Parameters.Add("@monto", MySqlDbType.Float);

                        registrar.Parameters["@idempleado"].Value = id_vendedor;
                        registrar.Parameters["@idcliente"].Value = txtIDCliente.Text;
                        registrar.Parameters["@f_venta"].Value = Convert.ToDateTime(txtFecha.Text);
                        registrar.Parameters["@tipopago"].Value = cmbTipoPago.SelectedItem.ToString();
                        registrar.Parameters["@monto"].Value = txtTotal.Text;

                        if (registrar.ExecuteNonQuery() == 1)
                        {
                            String query_detalle = "insert into detalle_venta(ID_Venta, Cantidad, Precio, ID_Producto) values (@idventa, @cantidad, @precio, @idprod)";

                            MySqlCommand registrar_detalle = new MySqlCommand(query_detalle, conectar);

                            foreach (DataGridViewRow r in dtgvProducto.Rows)
                            {
                                registrar_detalle.Parameters.Clear();
                                registrar_detalle.Parameters.Add("@idventa", MySqlDbType.Int32, 11);
                                registrar_detalle.Parameters.Add("@cantidad", MySqlDbType.Int32, 11);
                                registrar_detalle.Parameters.Add("@precio", MySqlDbType.Float);
                                registrar_detalle.Parameters.Add("@idprod", MySqlDbType.Int32, 11);

                                registrar_detalle.Parameters["@idventa"].Value = txtIDVenta.Text;
                                registrar_detalle.Parameters["@cantidad"].Value = r.Cells[6].Value;
                                registrar_detalle.Parameters["@precio"].Value = r.Cells[2].Value;
                                registrar_detalle.Parameters["@idprod"].Value = r.Cells[0].Value;
                                restar_existencias(r.Cells[0].Value.ToString(), r.Cells[6].Value.ToString());

                                registrar_detalle.ExecuteNonQuery();
                            }
                            MessageBox.Show("Venta generada exitosamente");
                            generarPDF();
                            registrar_pagoCredito();
                            int contID = Convert.ToInt32(txtIDVenta.Text) + 1;
                            txtIDVenta.Text = contID.ToString();
                            restar_credito(txtIDCliente.Text.ToString());
                            Limpiar();
                            btnLimpiar_Click(null, e);
                            conectar.Close();
                        }
                        else
                        {
                            MessageBox.Show("Error al momento de registrar la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            conectar.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Crédito insuficiente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show("La cantidad de productos que ingresó es superior a la que se tiene en tienda", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Limpiar()
        {
            cmbTipoPago.SelectedIndex = -1;
            txtIDCliente.Text = "";
            nupCantidad.Value = 1;
            txtIDProducto.Text = "";
        }


        public static Boolean comprobar_cantidad(String id_prod, String cantidad)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select Existencias from producto where IDProducto = '" + id_prod + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                if (Convert.ToInt32(leerConsulta.GetString(0)) < Convert.ToInt32(cantidad))
                {
                    conectar.Close();
                    return false;
                }
            }
            return true;
        }


        private void restar_existencias(String id_prod, String cantidad)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select Existencias from producto where IDProducto = '" + id_prod + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                int resta;
                resta = Convert.ToInt32(leerConsulta.GetString(0)) - Convert.ToInt32(cantidad);

                leerConsulta.Close();

                String query = "update producto set Existencias=@existencias where IDProducto='" + id_prod + "'";

                MySqlCommand modificar = new MySqlCommand(query, conectar);

                modificar.Parameters.Add("@existencias", MySqlDbType.Int32, 11);

                modificar.Parameters["@existencias"].Value = Convert.ToString(resta);


                if (modificar.ExecuteNonQuery() == 1)
                {
                    int i = 1;
                }
                conectar.Close();
            }
            else
            {
                MessageBox.Show("Error al momento de registrar la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conectar.Close();
            }
        }

        public static Boolean comprobar_credito(String id_cliente, String cantidad)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select CreditoDisponible from cliente where IDCliente = '" + id_cliente + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                if (Convert.ToDecimal(leerConsulta.GetString(0)) < Convert.ToDecimal(cantidad))
                {
                    conectar.Close();
                    return false;
                }
            }
            else
            {
                conectar.Close();
                return false;
            }
            return true;
        }

        private void restar_credito(String id_cliente)
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            MySqlCommand buscar = new MySqlCommand();
            buscar.Connection = conectar;

            buscar.CommandText = ("select CreditoDisponible from cliente where IDCliente = '" + id_cliente + "'");

            MySqlDataReader leerConsulta = buscar.ExecuteReader();
            if (leerConsulta.Read())
            {
                decimal resta;
                resta = Convert.ToDecimal(leerConsulta.GetString(0)) - Convert.ToDecimal(txtTotal.Text);

                leerConsulta.Close();

                String query = "update cliente set CreditoDisponible=@credito where IDCliente = '" + id_cliente + "'";

                MySqlCommand modificar = new MySqlCommand(query, conectar);

                modificar.Parameters.Add("@credito", MySqlDbType.Float);

                modificar.Parameters["@credito"].Value = Convert.ToString(resta);


                if (modificar.ExecuteNonQuery() == 1)
                {
                    int i = 1;
                }
                conectar.Close();
            }
            else
            {
                MessageBox.Show("Error al momento de registrar la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                conectar.Close();
            }
        }

        private void registrar_pagoCredito()
        {
            MySqlConnection conectar = new MySqlConnection("server=localhost; database=surtisistema; Uid=root; Pwd=; port=3306");
            conectar.Open();

            String query = "insert into pago_con_credito(IDCliente, Monto_a_pagar, FechaDePago) values (@idcliente, @monto, @f_venta)";

            MySqlCommand registrar = new MySqlCommand(query, conectar);

            registrar.Parameters.Add("@idcliente", MySqlDbType.Int32, 11);
            registrar.Parameters.Add("@monto", MySqlDbType.Float);
            registrar.Parameters.Add("@f_venta", MySqlDbType.DateTime);

            registrar.Parameters["@idcliente"].Value = txtIDCliente.Text;
            registrar.Parameters["@monto"].Value = txtTotal.Text;
            registrar.Parameters["@f_venta"].Value = Convert.ToDateTime(txtFecha.Text);

            if (registrar.ExecuteNonQuery() == 1)
            {
                int i = 1;
            }
        }


        public void generarPDF()
        {
            Document doc = new Document(PageSize.LETTER);
            PdfWriter.GetInstance(doc, new FileStream("cuenta" + txtIDVenta.Text + ".pdf", FileMode.Create));

            doc.Open();

            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            doc.Add(new Paragraph("Cuenta"));
            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("Folio de Venta: " + txtIDVenta.Text + "                                    RFC Empleado: " + txtIDEmpleado.Text));
            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("Nombre Empleado: " + txtNombreEmpleado.Text + "                                   ID Cliente: " + txtIDCliente.Text));
            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("Fecha Venta: " + txtFecha.Text + "                                    Tipo de Pago: " + cmbTipoPago.SelectedItem.ToString()));
            doc.Add(Chunk.NEWLINE);

            PdfPTable tblCuenta = new PdfPTable(8);
            tblCuenta.WidthPercentage = 100;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clID = new PdfPCell(new Phrase("ID", _standardFont));
            clID.BorderWidth = 0;
            clID.BorderWidthBottom = 0.30f;

            PdfPCell clDescp = new PdfPCell(new Phrase("Descripción", _standardFont));
            clDescp.BorderWidth = 0;
            clDescp.BorderWidthBottom = 1.10f;

            PdfPCell clPrecio = new PdfPCell(new Phrase("Precio", _standardFont));
            clPrecio.BorderWidth = 0;
            clPrecio.BorderWidthBottom = 0.30f;

            PdfPCell clNoProd = new PdfPCell(new Phrase("No. Producto", _standardFont));
            clNoProd.BorderWidth = 0;
            clNoProd.BorderWidthBottom = 0.50f;

            PdfPCell clColor = new PdfPCell(new Phrase("Color", _standardFont));
            clColor.BorderWidth = 0;
            clColor.BorderWidthBottom = 0.50f;

            PdfPCell clModelo = new PdfPCell(new Phrase("Modelo", _standardFont));
            clModelo.BorderWidth = 0;
            clModelo.BorderWidthBottom = 0.50f;

            PdfPCell clCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
            clCantidad.BorderWidth = 0;
            clCantidad.BorderWidthBottom = 0.50f;

            PdfPCell clImporte = new PdfPCell(new Phrase("Importe", _standardFont));
            clImporte.BorderWidth = 0;
            clImporte.BorderWidthBottom = 0.50f;

            // Añadimos las celdas a la tabla
            tblCuenta.AddCell(clID);
            tblCuenta.AddCell(clDescp);
            tblCuenta.AddCell(clPrecio);
            tblCuenta.AddCell(clNoProd);
            tblCuenta.AddCell(clColor);
            tblCuenta.AddCell(clModelo);
            tblCuenta.AddCell(clCantidad);
            tblCuenta.AddCell(clImporte);


            foreach (DataGridViewRow r in dtgvProducto.Rows)
            {
                clID = new PdfPCell(new Phrase(r.Cells[0].Value.ToString(), _standardFont));
                clID.BorderWidth = 0;

                clDescp = new PdfPCell(new Phrase(r.Cells[1].Value.ToString(), _standardFont));
                clDescp.BorderWidth = 0;

                clPrecio = new PdfPCell(new Phrase(r.Cells[2].Value.ToString(), _standardFont));
                clPrecio.BorderWidth = 0;

                clNoProd = new PdfPCell(new Phrase(r.Cells[3].Value.ToString(), _standardFont));
                clNoProd.BorderWidth = 0;

                clColor = new PdfPCell(new Phrase(r.Cells[4].Value.ToString(), _standardFont));
                clColor.BorderWidth = 0;

                clModelo = new PdfPCell(new Phrase(r.Cells[5].Value.ToString(), _standardFont));
                clModelo.BorderWidth = 0;

                clCantidad = new PdfPCell(new Phrase(r.Cells[6].Value.ToString(), _standardFont));
                clCantidad.BorderWidth = 0;

                clImporte = new PdfPCell(new Phrase(r.Cells[7].Value.ToString(), _standardFont));
                clImporte.BorderWidth = 0;

                tblCuenta.AddCell(clID);
                tblCuenta.AddCell(clDescp);
                tblCuenta.AddCell(clPrecio);
                tblCuenta.AddCell(clNoProd);
                tblCuenta.AddCell(clColor);
                tblCuenta.AddCell(clModelo);
                tblCuenta.AddCell(clCantidad);
                tblCuenta.AddCell(clImporte);
            }

            doc.Add(tblCuenta);

            doc.Add(new Paragraph("Subtotal: " + txtSubtotal.Text + "                IVA: " + txtIVA.Text + "                       Total: " + txtTotal.Text));
            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("La Surtidora le agradece por su compra, ¡Qué tenga un buen día!"));
            doc.Add(Chunk.NEWLINE);

            doc.Close();
        }

        private void fechaHora_Tick(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
