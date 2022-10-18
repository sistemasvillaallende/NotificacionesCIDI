using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Respuesta
{
    public String Resultado { get; set; }
    public String CodigoError { get; set; }
    public String SesionHash { get; set; }
    public String Mensaje { get; set; }
}

public class ResultadoSMS : Respuesta
{
    public String Celular { get; set; }
}

public class ResultadoEmail : Respuesta
{
    public String Email { get; set; }
}

public class RespuestaHistorial : Respuesta
{
    public List<Historial> HistorialList { get; set; }

    public RespuestaHistorial()
    {
        HistorialList = new List<Historial>();
    }
}