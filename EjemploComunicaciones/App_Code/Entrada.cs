using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

public class Entrada
{
    public int IdAplicacion { get; set; }
    public String Contrasenia { get; set; }
    public String HashCookie { get; set; }
    public String TokenValue { get; set; }
    public String TimeStamp { get; set; }
    public String SesionHash { get; set; }
}

public class EntradaNotificacion : Entrada
{
    public String Cuil { get; set; }
    public String Mensaje { get; set; }
    public String FechaDesde { get; set; }
    public String FechaHasta { get; set; }
}

public class EntradaCuilDireccion : Entrada
{
    public String Cuil { get; set; }
    public String Email { get; set; }
    public String Mensaje { get; set; }
    public String Asunto { get; set; }
}

public class EntradaHistorial : Entrada
{
    public String Cuil { get; set; }
    public String FechaDesde { get; set; }
    public String FechaHasta { get; set; }
}
