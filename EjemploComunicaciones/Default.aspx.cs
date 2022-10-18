using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using EjemploCiDi.Models;

public partial class _Default : System.Web.UI.Page
{
    #region Variables

    /// <summary>
    /// Variable para guardar el valor de la cookie
    /// </summary>
    public String _Hash;

    #endregion

    #region Eventos

    protected void Page_Load(object sender, EventArgs e)
    {
        ///// <summary>
        ///// Verificar si la cookie existe, para saber si el usuario ya esta logueado
        ///// </summary>
        //if (Request.Cookies["CiDi"] != null)Request.QueryString["cidi"]
        if (Request.QueryString["cidi"] != null)
        {
            /// <summary>
            /// Se guarda el valor de la cookie en una variable
            /// </summary>
            _Hash = Request.QueryString["cidi"].ToString();

            if (!IsPostBack)
            {
                /// <summary>
                /// Se optiene el SesionHash de la APP
                /// </summary>
                Entrada entrada = new Entrada();
                entrada.IdAplicacion = Config.CiDiIdAplicacion;
                entrada.Contrasenia = Config.CiDiPassAplicacion;
                entrada.HashCookie = Request.QueryString["cidi"].ToString();
                entrada.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                entrada.TokenValue = Config.ObtenerToken_SHA1(entrada.TimeStamp);

                Respuesta respuesta = Config.LlamarWebAPI<Entrada, Respuesta>(APICuenta.Aplicacion.Registrar_Aplicacion, entrada);

                if (respuesta.Resultado == Config.CiDi_OK)
                {
                    lblSesionHash.Text = respuesta.SesionHash;
                }
            }
        }
        else
        {
            /// <summary>
            /// Como el usuario no se logueo se redirecciona al login de CiDi
            /// </summary>
            //Response.Redirect(Config.CiDiUrl + "?url=" + Config.MyAppUrl + "&app=" + Config.CiDiIdAplicacion);
        }
    }

    protected void btnEmail_Click(object sender, EventArgs e)
    {
        try
        {
            Email email = new Email();
            email.Cuil = txtCuilEmail.Text;
            email.Asunto = "Prueba";
            email.Subtitulo = "Subtitulo";
            email.Mensaje = "<a href='http://www.w3schools.com/html/'>Visit our HTML tutorial</a>";
            email.InfoDesc = "InfoDesc";
            email.InfoDato = "InfoDato";
            email.InfoLink = "http://google.com";
            email.Firma = "Firma";
            email.Ente = "Gobierno de Córdoba";
            email.Id_App = Config.CiDiIdAplicacion;
            email.Pass_App = Config.CiDiPassAplicacion;
            email.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            email.TokenValue = Config.ObtenerToken_SHA512(email.TimeStamp);

            ResultadoEmail respuesta = Config.LlamarWebAPI<Email, ResultadoEmail>(APIComunicacion.Email.Enviar, email);

            if (respuesta.Resultado == Config.CiDi_OK)
            {
                lblResultadoEmail.Text = respuesta.Resultado;
                lblMensajeEmail.Text = respuesta.Mensaje;
                lblEmail.Text = respuesta.Email;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnCuentaEmail_Click(object sender, EventArgs e)
    {
        try
        {
            Email email = new Email();
            email.DireccionEmail = txtCuentaEmail.Text;
            email.Asunto = "Prueba";
            //email.Subtitulo = "Subtitulo";
            email.Mensaje = "<HTML><HEAD><TITLE>Un Titulo para el Browser de turno </TITLE></HEAD><BODY><H1>Otro t&iacute;tulo, esta vez m&aacute;s largo. </H1><P>Hoola.<P>Esto es un parrafo con informacion super importante. Notese que las lineas salen pegadas aun dejando espacios, saltos de linea, etc. <BR> &#161 Si pongo esto si <STRONG>cambia </STRONG> de linea!<P>Otro parrafo, esto ya es un poco rollo.<H3>Pongamos un subtítulo<H3><P>Por cierto, &#191 que paso con las <A HREF= '#pepe '>anclas</A>?<HR><UL><LI> Esto es una lista no ordenada.<LI> Las listas quedan mejor si tienen varios elementos.</UL>Me voy al <A HREF= 'http://www.iac.es/home.html '>IAC</A>.<P>Vamos a crear un <EM>ancla </EM>, o lo que es lo mismo, un <A NAME='pepe'>anchor.</A>..........................................................................................................</BODY></HTML>Tu visualizador lo vería así.El HTML se basa en una serie de etiquetas (<tags>), que casi siempre hay una al principio y otra al final. Lo que si es común a todas es que están englobadas en los símbolos em mayor que y menor que, de forma: <etiqueta>. Veamos las del ejemplo en detalle:";
            email.InfoDesc = "InfoDesc";
            email.InfoDato = "InfoDato";
            email.InfoLink = "http://google.com";
            email.Firma = "Firma";
            email.Ente = "Gobierno de Córdoba";

            //email.Id_App = Config.CiDiIdAplicacion;
            //email.Pass_App = Config.CiDiPassAplicacion;
            //email.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //email.TokenValue = ObtenerTokenSHA512(email.TimeStamp);
            email.SesionHash = lblSesionHash.Text;

            ResultadoEmail respuesta = Config.LlamarWebAPI<Email, ResultadoEmail>(APIComunicacion.Email.Enviar_Direccion_Email, email);

            if (respuesta.Resultado == Config.CiDi_OK)
            {
                lblCuentaEmailResultado.Text = respuesta.Resultado;
                lblCuentaEmailMensaje.Text = respuesta.Mensaje;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnCuentaEmailClob_Click(object sender, EventArgs e)
    {
        try
        {
            Email email = new Email();
            email.DireccionEmail = txtCuentaEmailClob.Text;
            email.Asunto = "Prueba";
            //email.Subtitulo = "Subtitulo";
            email.Mensaje = txtCuentaEmailClobCuerpo.Text;
            email.InfoDesc = "InfoDesc";
            email.InfoDato = "InfoDato";
            email.InfoLink = "http://google.com";
            email.Firma = "Firma";
            email.Ente = "Gobierno de Córdoba";

            email.Id_App = Config.CiDiIdAplicacion;
            //email.Pass_App = Config.CiDiPassAplicacion;
            email.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            email.TokenValue = Config.ObtenerToken_SHA512(email.TimeStamp);
            email.SesionHash = lblSesionHash.Text;

            ResultadoEmail respuesta = Config.LlamarWebAPI<Email, ResultadoEmail>(APIComunicacion.Email.Enviar_Direccion_Email, email);

            lblEmailClobResultado.Text = respuesta.Resultado;
            lblEmailClobMensaje.Text = respuesta.Mensaje;
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnEmailMasivo_Click(object sender, EventArgs e)
    {
        try
        {
            EmailMasivo email = new EmailMasivo();

            email.IdAplicacion = Config.CiDiIdAplicacion;
            email.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            email.TokenValue = Config.ObtenerToken_SHA1(email.TimeStamp);
            email.SesionHash = lblSesionHash.Text;

            email.DireccionEmail = txtEmailMasivoCuenta.Text;
            email.Asunto = "Prueba";
            email.Mensaje = txtEmailMasivoMensaje.Text;
            email.Notificacion = "Notificacion de prueba";

            ResultadoEmail respuesta = Config.LlamarWebAPI<EmailMasivo, ResultadoEmail>(APIComunicacion.Email.Enviar_Masivo, email);

            lblEmailMasivoResultado.Text = respuesta.Resultado;
            lblEmailMasivoMensaje.Text = respuesta.Mensaje;
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnSMS_Click(object sender, EventArgs e)
    {
        try
        {
            SMS SMS = new SMS();
            SMS.Cuil = txtCuilSMS.Text;
            SMS.Mensaje = String.IsNullOrEmpty(txtTextoNotificacion.Text) ? "Prueba" : txtTextoNotificacion.Text;
            SMS.Id_App = Config.CiDiIdAplicacion;
            SMS.Pass_App = Config.CiDiPassAplicacion;
            SMS.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            SMS.TokenValue = Config.ObtenerToken_SHA1(SMS.TimeStamp);

            ResultadoSMS respuesta = Config.LlamarWebAPI<SMS, ResultadoSMS>(APIComunicacion.SMS.Enviar, SMS);

            if (respuesta.Resultado == Config.CiDi_OK)
            {
                lblResultadoSMS.Text = respuesta.Resultado;
                lblMensajeSMS.Text = respuesta.Mensaje;
                lblSMS.Text = respuesta.Celular;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnNotificaciones_Click(object sender, EventArgs e)
    {
        try
        {
            EntradaNotificacion entrada = new EntradaNotificacion();
            entrada.IdAplicacion = Config.CiDiIdAplicacion;
            entrada.Contrasenia = Config.CiDiPassAplicacion;
            entrada.HashCookie = _Hash;
            entrada.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            entrada.TokenValue = Config.ObtenerToken_SHA512(entrada.TimeStamp);

            entrada.Cuil = txtCuilNotificacion.Text;
            entrada.FechaDesde = DateTime.Now.ToShortDateString();
            entrada.FechaHasta = DateTime.Now.AddDays(7).ToShortDateString();

            entrada.Mensaje = txtTextoNotificacion.Text;

            ResultadoSMS respuesta = Config.LlamarWebAPI<EntradaNotificacion, ResultadoSMS>(APIComunicacion.Notificacion.Enviar_Alerta, entrada);

            if (respuesta.Resultado == Config.CiDi_OK)
            {
                lblResultadoAlerta.Text = respuesta.Resultado;
                lblMensajeAlerta.Text = respuesta.Mensaje;
            }
            else
            {
                lblResultadoAlerta.Text = respuesta.CodigoError;
                lblMensajeAlerta.Text = respuesta.Resultado;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnEmailHTML_Click(object sender, EventArgs e)
    {
        try
        {
            EmailHTML email = new EmailHTML();
            email.Cuil = txtCuilEmailHTML.Text;
            email.HashCookie = _Hash;
            email.Asunto = "Prueba";
            email.BodyHTML = txtHTML.Text;
            email.Firma = "Firma";
            email.Ente = "Gobierno de Córdoba";
            email.Id_App = Config.CiDiIdAplicacion;
            email.Pass_App = Config.CiDiPassAplicacion;
            email.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            email.TokenValue = Config.ObtenerToken_SHA1(email.TimeStamp);

            ResultadoEmail respuesta = Config.LlamarWebAPI<EmailHTML, ResultadoEmail>(APIComunicacion.Email.Enviar_HTML, email);

            lblResultadoEmailHTML.Text = respuesta.Resultado;
            lblMensajeEmailHTML.Text = respuesta.Mensaje;
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnEnvioCuilDireccion_Click(object sender, EventArgs e)
    {
        try
        {
            EntradaCuilDireccion entrada = new EntradaCuilDireccion();
            entrada.IdAplicacion = Config.CiDiIdAplicacion;
            entrada.Contrasenia = Config.CiDiPassAplicacion;
            //TESTING
            //entrada.SesionHash = "396447367759556E2B766E61332F76425441686448792B734F57773D";
            //PRODUCCION
            entrada.SesionHash = lblSesionHash.Text;
            entrada.HashCookie = _Hash;
            entrada.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            entrada.TokenValue = Config.ObtenerToken_SHA512(entrada.TimeStamp);

            entrada.Cuil = txtCuilEnvioCuilDireccion.Text;
            entrada.Email = txtEmailDireccion.Text;
            entrada.Mensaje = txtCuilDireccion.Text;
            entrada.Asunto = "Asunto";

            Respuesta respuesta = Config.LlamarWebAPI<EntradaCuilDireccion, ResultadoEmail>(APIComunicacion.Email.Enviar_Cuil_Direccion, entrada);

            if (respuesta.Resultado == Config.CiDi_OK)
            {
                lblResCuilDireccion.Text = respuesta.Resultado;
                lblMsjCuilDireccion.Text = respuesta.Mensaje;
            }
            else
            {
                lblResCuilDireccion.Text = respuesta.CodigoError;
                lblMsjCuilDireccion.Text = respuesta.Resultado;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnHistorial_Click(object sender, EventArgs e)
    {
        try
        {
            EntradaHistorial entrada = new EntradaHistorial();

            entrada.IdAplicacion = Config.CiDiIdAplicacion;
            entrada.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            entrada.TokenValue = Config.ObtenerToken_SHA512(entrada.TimeStamp);
            entrada.SesionHash = lblSesionHash.Text;

            entrada.Cuil = txtCuilHistorial.Text;

            RespuestaHistorial respuesta = Config.LlamarWebAPI<EntradaHistorial, RespuestaHistorial>(APIComunicacion.Historial.Get, entrada);

            if (respuesta.Resultado == Config.CiDi_OK)
            {
                lblHistorialResultado.Text = respuesta.Resultado;
            }
            else
            {
                lblHistorialResultado.Text = respuesta.CodigoError + respuesta.Resultado;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }


    #endregion

    #region Metodos


    #endregion
}