<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default"
    ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ejemplo WebApp Comunicación | Ciudadano Digital</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="Form1" runat="server">
    <img src="http://ciudadanodigital.cba.gov.ar/wp-content/themes/cidiresponsive/img/logo_ciudadanodigital.png"
        alt="Plataforma Provincial del Ciudadano Digital" width="100px" />
    <hr />
    <div style="text-align: center;">
        <h3>
            Ejemplo Envío SMS y Email</h3>
    </div>
    <asp:Label ID="lblSesionHash" runat="server" Visible="False"></asp:Label>
    <br />
    <div>
        <fieldset>
            <div>
                CUIL:
                <asp:TextBox ID="txtCuilEmail" runat="server"></asp:TextBox>
                <asp:Button ID="btnEmail" runat="server" Text="Enviar Email" OnClick="btnEmail_Click" />
            </div>
            <leyend>Resultado Envio Email</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblResultadoEmail" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblMensajeEmail" runat="server"></asp:Label></p>
            <p>
                Se envío al email:
                <asp:Label ID="lblEmail" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <br />
    <div>
        <fieldset>
            <div>
                CUIL:
                <asp:TextBox ID="txtCuilEmailHTML" runat="server"></asp:TextBox><br />
                <asp:TextBox ID="txtHTML" runat="server" TextMode="MultiLine"></asp:TextBox><br />
                <asp:Button ID="btnEmailHTML" runat="server" Text="Enviar Email HTML" OnClick="btnEmailHTML_Click" />
            </div>
            <leyend>Resultado Envio Email</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblResultadoEmailHTML" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblMensajeEmailHTML" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <br />
    <div>
        <fieldset>
            <div>
                Cuenta:
                <asp:TextBox ID="txtCuentaEmail" runat="server"></asp:TextBox>
                <asp:Button ID="btCuentaEmail" runat="server" Text="Enviar Cuenta Email" OnClick="btnCuentaEmail_Click" />
            </div>
            <leyend>Resultado Envio Email</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblCuentaEmailResultado" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblCuentaEmailMensaje" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <br />
    <div>
        <fieldset>
            <div>
                Cuenta:
                <asp:TextBox ID="txtCuentaEmailClob" runat="server"></asp:TextBox>
                <asp:Button ID="btnCuentaEmailClob" runat="server" Text="Enviar Cuenta Email Clob"
                    OnClick="btnCuentaEmailClob_Click" />
            </div>
            <asp:TextBox ID="txtCuentaEmailClobCuerpo" runat="server" TextMode="MultiLine"></asp:TextBox><br />
            <leyend>Resultado Envio Email</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblEmailClobResultado" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblEmailClobMensaje" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <br />
    <div>
        <fieldset>
            <div>
                Cuenta:
                <asp:TextBox ID="txtEmailMasivoCuenta" runat="server"></asp:TextBox>
                <asp:Button ID="btnEmailMasivo" runat="server" Text="Enviar Email Masivo" OnClick="btnEmailMasivo_Click" />
            </div>
            <asp:TextBox ID="txtEmailMasivoMensaje" runat="server" TextMode="MultiLine"></asp:TextBox><br />
            <leyend>Resultado Envio Email</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblEmailMasivoResultado" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblEmailMasivoMensaje" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <br />
    <div>
        <fieldset>
            <div>
                CUIL:
                <asp:TextBox ID="txtCuilSMS" runat="server"></asp:TextBox>
                <asp:Button ID="btnSMS" runat="server" Text="Enviar SMS" OnClick="btnSMS_Click" />
            </div>
            <leyend>Resultado Envio SMS</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblResultadoSMS" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblMensajeSMS" runat="server"></asp:Label></p>
            <p>
                Se envío al SMS:
                <asp:Label ID="lblSMS" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <div>
        <fieldset>
            <div>
                CUIL:
                <asp:TextBox ID="txtCuilNotificacion" runat="server"></asp:TextBox><asp:Button ID="btnNotificaciones"
                    runat="server" Text="Enviar Alerta" OnClick="btnNotificaciones_Click" />
                <br />
            </div>
            Texto:<asp:TextBox ID="txtTextoNotificacion" runat="server" Text="Prueba" TextMode="MultiLine"></asp:TextBox>
            <br />
            <leyend>Resultado Envio de Alerta</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblResultadoAlerta" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblMensajeAlerta" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <div>
        <fieldset>
            <div>
                CUIL:
                <asp:TextBox ID="txtCuilEnvioCuilDireccion" runat="server"></asp:TextBox><asp:Button
                    ID="btnEnvioCuilDireccion" runat="server" Text="Enviar Cuil Direccion" OnClick="btnEnvioCuilDireccion_Click" />
                <br />
                Email:
                <asp:TextBox ID="txtEmailDireccion" runat="server"></asp:TextBox>
                <br />
            </div>
            Texto:<asp:TextBox ID="txtCuilDireccion" runat="server" Text="Prueba" TextMode="MultiLine"></asp:TextBox>
            <br />
            <leyend>Resultado Cuil Direccion</leyend>
            <p>
                Resultado:
                <asp:Label ID="lblResCuilDireccion" runat="server"></asp:Label></p>
            <p>
                Mensaje:
                <asp:Label ID="lblMsjCuilDireccion" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    <div>
        <fieldset>
            <div>
                CUIL:
                <asp:TextBox ID="txtCuilHistorial" runat="server"></asp:TextBox>
                <asp:Button
                    ID="btnHistorial" runat="server" Text="Traer Historial" 
                    onclick="btnHistorial_Click" />
                <br />
            </div>
            <p>
                Resultado:
                <asp:Label ID="lblHistorialResultado" runat="server"></asp:Label></p>
        </fieldset>
    </div>
    </form>
</body>
</html>
