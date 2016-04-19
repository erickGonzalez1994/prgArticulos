using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Llamado de las referencias propias del proyecto
using System.Data.SqlClient;
using Modelo;
using Controlador;


namespace prgArticulos
{
    public partial class frmAcceso : Form
    {
        #region Atributos
        clsConexionSQL conexion;
        clsEntidadUsuario pEntidadUsuario;
        clsUsuario usuario;
        SqlDataReader dtrUsuario;//para el retorno de las tuplas
        int intContador = 0;
        #endregion

        //Inicializamos los atributos que utilizaremos en toda la ventana
        public frmAcceso()
        {
            conexion = new clsConexionSQL();
            pEntidadUsuario = new clsEntidadUsuario();
            usuario = new clsUsuario();
            InitializeComponent();
        }

        private void frmAcceso_Load(object sender, EventArgs e)
        {

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            //Accion para salir del sistema
            Application.Exit();
        }

        private void txtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
                //El evento focus permite trasladar el cursor del mouse al objeto indicado
                this.txtClave.Focus();
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.Enter))
            {
                if (mValidarDatos() == true)
                {
                    this.btnIngresar.Enabled = true;
                }
            }
        }//fin del keypress de clave



        #region Metodos
        //Este metodo permite verificar la existencia del usuario segun el codigo y la  clave digitada
        private Boolean mValidarDatos()
        {
            if (intContador <= 2)
            {
                // Llenado de los atributos de la BD
                conexion.setCodigo("admEstudiante");
                conexion.setClave("123");

                // Lenado de los atributos de la clase EntidadUsuario
                pEntidadUsuario.setCodigo(this.txtCodigo.Text.Trim());
                pEntidadUsuario.setClave(this.txtClave.Text.Trim());

                //Consultamos si el usuario existe
                dtrUsuario = usuario.mConsultarUsuario(conexion, pEntidadUsuario);


                //Evaluo si retorna tuplas o datos
                if (dtrUsuario != null)
                {
                    if (dtrUsuario.Read())
                    {
                        pEntidadUsuario.setPerfil(dtrUsuario.GetString(2));
                        pEntidadUsuario.setEstado(dtrUsuario.GetInt32(3));
                        if (pEntidadUsuario.getEstado() == 0)
                        {
                            this.btnIngresar.Enabled = true;
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("El usuario esta bloqueado", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }//fin del pentidadUsuario
                    }
                    else {
                        MessageBox.Show("El usuario no existe", "Atencion", MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return false;
                    }//Fin del if del read

                }
                else {
                    MessageBox.Show("El usuario no existe", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }//fin del if del null


            }else {
                MessageBox.Show("Usted digito 3 veces su usuario de forma erronea", "Usuario Boqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }//Fin del if del contador
           
        }// fin de metodo mValidar
        #endregion

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            this.SetVisibleCore(false);
            mdiMenu menu = new mdiMenu(conexion);
            menu.Show();
        }
    }
}
