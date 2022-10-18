using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Email
{
    public int Id_App { get; set; }
    public String Pass_App { get; set; }
    public String Cuil { get; set; }
    public String HashCookie { get; set; }
    public String Asunto { get; set; }
    public String Subtitulo { get; set; }
    public String Mensaje { get; set; }
    public String InfoDesc { get; set; }
    public String InfoDato { get; set; }
    public String InfoLink { get; set; }
    public String Firma { get; set; }
    public String Ente { get; set; }
    public String TokenValue { get; set; }
    public String TimeStamp { get; set; }
    public String DireccionEmail { get; set; }
    public String SesionHash { get; set; }
}

public class EmailHTML
{
    public int Id_App { get; set; }
    public String Pass_App { get; set; }
    public String Cuil { get; set; }
    public String HashCookie { get; set; }
    public String Asunto { get; set; }
    public String BodyHTML { get; set; }
    public String Firma { get; set; }
    public String Ente { get; set; }
    public String TokenValue { get; set; }
    public String TimeStamp { get; set; }
}

public class EmailMasivo : Entrada
{
    public String DireccionEmail { get; set; }
    public String Asunto { get; set; }
    public String Mensaje { get; set; }
    public String Notificacion { get; set; }
}