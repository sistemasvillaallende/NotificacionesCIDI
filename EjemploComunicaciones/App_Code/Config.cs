using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace EjemploCiDi.Models
{
    public class Config
    {
        #region Propiedades

        public static String CiDiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiUrl"].ToString();
            }
        }

        public static String MyAppUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["MyAppUrl"].ToString();
            }
        }

        public static int CiDiIdAplicacion
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CiDiIdAplicacion"].ToString());
            }
        }

        public static String CiDiPassAplicacion
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiPassAplicacion"].ToString();
            }
        }

        public static String CiDiKeyAplicacion
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiKeyAplicacion"].ToString();
            }
        }

        public static String APICuenta
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiUrlAPICuenta"].ToString();
            }
        }

        public static String APIComunicacion
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiUrlAPIComunicacion"].ToString();
            }
        }

        public static String APIDocumentacion
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiUrlAPIDocumentacion"].ToString();
            }
        }

        public static String APIMobile
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiUrlAPIMobile"].ToString();
            }
        }

        public static String CiDiUrlRelacion
        {
            get
            {
                return ConfigurationManager.AppSettings["CiDiUrlRelacion"].ToString();
            }
        }

        public static String CiDi_OK
        {
            get
            {
                return "OK";
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Metodo para obtener el token para enviar a la WebAPI. Este token consiste en un hash SHA512 del Timestamp + KEY de aplicación para validar la integridad y autenticidad de los parámetros utilizados.
        /// </summary>
        /// <param name="TimeStamp">Recibe un TimeStamp con formato debe ser yyyyMMddHHmmssfff Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")</param>
        /// <returns>String</returns>
        public static String ObtenerToken_SHA512(String TimeStamp)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(TimeStamp + CiDiKeyAplicacion);
            SHA512 SHA512M = new SHA512Managed();
            String token = BitConverter.ToString(SHA512M.ComputeHash(buffer)).Replace("-", "");

            return token;
        }

        /// <summary>
        /// Metodo para obtener el token para enviar a la WebAPI. Este token consiste en un hash SHA1 del Timestamp + KEY de aplicación para validar la integridad y autenticidad de los parámetros utilizados.
        /// </summary>
        /// <param name="TimeStamp">Recibe un TimeStamp con formato debe ser yyyyMMddHHmmssfff Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")</param>
        /// <returns>String</returns>
        public static String ObtenerToken_SHA1(String TimeStamp)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(TimeStamp + CiDiKeyAplicacion);
            SHA1 SHA1 = new SHA1Managed();
            String token = BitConverter.ToString(SHA1.ComputeHash(buffer)).Replace("-", "");

            return token;
        }

        /// <summary>
        /// Realiza la llamada a la WebAPI de Ciudadano Digital, serializa la Entrada y deserializa la Respuesta.
        /// </summary>
        /// <typeparam name="TEntrada">Declarar el objeto de Entrada al método.</typeparam>
        /// <typeparam name="TRespuesta">Declarar el objeto de Respuesta al método.</typeparam>
        /// <param name="Accion">Recibe la acción específica del controlador de la WebAPI.</param>
        /// <param name="tEntrada">Objeto de entrada de la WebAPI , especificado en TEntrada.</param>
        /// <returns>Objeto de salida de la WebAPI, especificado en TRespuesta.</returns>
        public static TRespuesta LlamarWebAPI<TEntrada, TRespuesta>(String Accion, TEntrada tEntrada)
        {
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Accion);
                httpWebRequest.ContentType = "application/json; charset=utf-8";

                String rawjson = JsonConvert.SerializeObject(tEntrada);
                httpWebRequest.Method = "POST";

                var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

                streamWriter.Write(rawjson);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();

                TRespuesta respuesta = JsonConvert.DeserializeObject<TRespuesta>(result);

                return respuesta;
            }
            catch (WebException ex)
            {
                var httpResponse = (HttpWebResponse)ex.Response;
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                var result = streamReader.ReadToEnd();
                TRespuesta respuesta = JsonConvert.DeserializeObject<TRespuesta>(result);

                return respuesta;
            }
        }

        /// <summary>
        /// Realiza la llamada a la WebAPI de Ciudadano Digital, serializa la Entrada y deserializa la Respuesta.
        /// </summary>
        /// <typeparam name="TEntrada">Declarar el objeto de Entrada al método.</typeparam>
        /// <typeparam name="TRespuesta">Declarar el objeto de Respuesta al método.</typeparam>
        /// <param name="Accion">Recibe la acción específica del controlador de la WebAPI.</param>
        /// <param name="tEntrada">Objeto de entrada de la WebAPI , especificado en TEntrada.</param>
        /// <returns>Objeto de salida de la WebAPI, especificado en TRespuesta.</returns>
        public static TRespuesta LlamarWebAPI_GET<TEntrada, TRespuesta>(String URI)
        {
            HttpWebRequest _Request = (HttpWebRequest)WebRequest.Create(URI);
            _Request.Method = "GET";

            WebResponse _Response = _Request.GetResponse();
            StreamReader _Reader = new StreamReader(_Response.GetResponseStream(), Encoding.UTF8);
            String _Result = _Reader.ReadToEnd();

            TRespuesta respuesta = JsonConvert.DeserializeObject<TRespuesta>(_Result);

            return respuesta;
        }

        #endregion
    }

    public class APICuenta
    {
        public class Usuario
        {
            /// <summary>
            /// Obtención del usuario Ciudadano Digital a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Aplicacion
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario_Aplicacion";
                }
            }

            /// <summary>
            /// Obtención de un usuario Ciudadano Digital a través del CUIL
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario";
                }
            }

            /// <summary>
            /// Obtención del usuario Ciudadano Digital con datos básicos a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Basicos
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario_Basicos";
                }
            }

            /// <summary>
            /// Obtención del usuario Ciudadano Digital con datos básicos a través del CUIL
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.CUIL" tipo="Alfanumérico" long="30">CUIL del usuario.
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Basicos_CUIL
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario_Basicos_CUIL";
                }
            }

            /// <summary>
            /// Obtención del usuario Ciudadano Digital con datos básicos y el domicilo a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Basicos_Domicilio
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario_Basicos_Domicilio";
                }
            }

            /// <summary>
            /// Obtención del usuario Ciudadano Digital con datos básicos y el domicilio a través del CUIL
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.CUIL" tipo="Alfanumérico" long="30">CUIL del usuario.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Basicos_Domicilio_CUIL
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario_Basicos_Domicilio_CUIL";
                }
            }

            /// <summary>
            /// Obtención del usuario Ciudadano Digital con datos básicos y el representado a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Basicos_Representado
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Usuario_Basicos_Representado";
                }
            }

            /// <summary>
            /// Obtención de los datos del representado a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Representado
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Obtener_Representado";
                }
            }

            /// <summary>
            /// Cierre de sesión a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Cerrar_Sesion_Usuario_Aplicacion
            {
                get
                {
                    return Config.APICuenta + "api/Usuario/Cerrar_Sesion_Usuario_Aplicacion";
                }
            }
        }

        public class Representado
        {
            /// <summary>
            /// Obtención de un usuario y sus representados a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene del query string de la cookie encriptada por 
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Y_Representados
            {
                get
                {
                    return Config.APICuenta + "api/Representado/Obtener_Usuario_Y_Representados";
                }
            }

            /// <summary>
            /// Obtención de un usuario y sus representados a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Lista_Representados
            {
                get
                {
                    return Config.APICuenta + "api/Representado/Obtener_Usuario_Lista_Representados";
                }
            }

            /// <summary>
            /// Obtención del representado y un listado de representantes a través del CUIL/CUIT
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.CuitCuil" tipo="Alfanumérico" long="30">CUIL del usuario o CUIT de la organización.
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Representado_Lista_Representantes
            {
                get
                {
                    return Config.APICuenta + "api/Representado/Obtener_Representado_Lista_Representantes";
                }
            }

            /// <summary>
            /// Obtención del usuario Ciudadano Digital con datos básicos y el representado a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Usuario_Basicos_Representado
            {
                get
                {
                    return Config.APICuenta + "api/Representado/Obtener_Usuario_Basicos_Representado";
                }
            }

            /// <summary>
            /// Obtención de los datos del representado a través de la cookie
            /// </summary>
            /// <param name="Entrada">
            /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Obtener_Representado
            {
                get
                {
                    return Config.APICuenta + "api/Representado/Obtener_Representado";
                }
            }
        }

        public class Aplicacion
        {
            /// <summary>
            /// Cambio de contraseña de la aplicación. Para uso interno del desarrollador
            /// </summary>
            /// <param name="IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación (provisto por la gestión del proyecto Ciudadano Digital).</param>
            /// <param name="ContraseniaAnterior">Contraseña actual de la aplicación.</param>
            /// <param name="ContraseniaNueva">Nueva contraseña a establecer para la aplicación.</param>
            /// <param name="TokenValue" tipo="Alfanumérico" long="40">Contraseña de Ciudadano Digital. Consiste en un hash SHA1 del timestamp+SECRET_KEY para validar la integridad y autenticidad de los parámetros utilizados. La SECRET_KEY será una clave acordada entre la Aplicación y el portal.</param>
            /// <param name="TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del mensaje. El formato debe ser yyyyMMddHHmmssfff. Ej: DateTime.Now.ToString("yyyyMMddHHmmssfff");</param>
            /// <returns></returns>
            public static String Cambiar_Contrasenia_Aplicacion
            {
                get
                {
                    return Config.APICuenta + "api/Representado/Cambiar_Contrasenia_Aplicacion";
                }
            }

            /// <summary>
            /// Login de la aplicación y obtención del SesioHash
            /// </summary>
            /// <param name="IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación (provisto por la gestión del proyecto Ciudadano Digital).</param>
            /// <param name="Contrasenia">Contraseña de la aplicación.</param>
            /// <param name="TokenValue" tipo="Alfanumérico" long="40">Contraseña de Ciudadano Digital. Consiste en un hash SHA1 del timestamp+SECRET_KEY para validar la integridad y autenticidad de los parámetros utilizados. La SECRET_KEY será una clave acordada entre la Aplicación y el portal.</param>
            /// <param name="TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del mensaje. El formato debe ser yyyyMMddHHmmssfff. Ej: DateTime.Now.ToString("yyyyMMddHHmmssfff");</param>
            /// <returns></returns>
            public static String Registrar_Aplicacion
            {
                get
                {
                    return Config.APICuenta + "api/Aplicacion/Registrar_Aplicacion";
                }
            }
        }
    }

    public class APIComunicacion
    {
        public class Notificacion
        {
            /// <summary>
            /// Envía notificaciones a ciudadanos a través del CUIL, que se muestran en el portal de Ciudadano Digital.
            /// </summary>
            /// <param name="entrada">EntradaNotificacion</param>
            /// <returns>Respuesta</returns>
            public static String Enviar_Alerta
            {
                get
                {
                    return Config.APIComunicacion + "api/Notificacion/Enviar_Alerta";
                }
            }
        }

        public class SMS
        {
            /// <summary>
            /// Envia un SMS a un usuario Ciudadano Digital a través del Cuil
            /// </summary>
            /// <param name="SMS">Objecto SMS</param>
            /// <returns>ResultadoSMS</returns>
            public static String Enviar
            {
                get
                {
                    return Config.APIComunicacion + "api/SMS/Enviar";
                }
            }

            /// <summary>
            /// Envia un SMS a un usuario Ciudadano Digital a través de la cookie
            /// </summary>
            /// <param name="SMS">Objecto SMS</param>
            /// <returns>ResultadoSMS</returns>
            public static String Enviar_Aplicacion
            {
                get
                {
                    return Config.APIComunicacion + "api/SMS/Enviar_Aplicacion";
                }
            }
        }

        public class Email
        {
            /// <summary>
            /// Envia un email a un usuario Ciudadano Digital a través del Cuil
            /// </summary>
            /// <param name="email">Objecto Email</param>
            /// <returns>ResultadoEmail</returns>
            public static String Enviar
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar";
                }
            }

            /// <summary>
            /// Envia un email a un usuario Ciudadano Digital a través de la cookie
            /// </summary>
            /// <param name="email">Objecto Email</param>
            /// <returns>ResultadoEmail</returns>
            public static String Enviar_Aplicacion
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar_Aplicacion";
                }
            }

            /// <summary>
            /// Envia un email con contenido HTML a un usuario Ciudadano Digital a través del Cuil
            /// </summary>
            /// <param name="email">
            /// email.Id_App tipo="Numérico" Identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// email.Pass_App tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// email.Cuil tipo="Alfanumérico" CUIL del usuario al que se quiere enviar el mail.
            /// email.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie del operador. Nombre de la cookie "CiDi".
            /// email.Asunto tipo="Alfanumérico" Compuesto por una parte fija, igual para todos los emails, con un texto corto.
            /// email.BodyHTML tipo="Alfanumérico" Información relativa al negocio de aplicación, lo que se que quiere notificar.
            /// email.Firma tipo="Alfanumérico" Dato obligatorio, la persona que firma el mail enviado.
            /// email.Ente tipo="Alfanumérico" Ente que efectúa la notificación.
            /// email.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// email.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Respuesta</returns>
            public static String Enviar_HTML
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar_HTML";
                }
            }

            /// <summary>
            /// Envia un email a un ciudadano a través de la dirección de correo 
            /// </summary>
            /// <param name="email">
            /// email.Id_App tipo="Numérico" Identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// email.SesionHash tipo="Alfanumérico" Valor que se obtiene al autenticar la aplicación (Registrar_Aplicacion).
            /// email.DireccionEmail tipo="Alfanumérico" Dirección de correo del usuario al que se quiere enviar el mail.
            /// email.Asunto tipo="Alfanumérico" Compuesto por una parte fija, igual para todos los emails, con un texto corto.
            /// email.Subtitulo tipo="Alfanumérico" Dato opcional, en caso de que la aplicación quiera tipificar sus mensajes, texto resaltado.
            /// email.Mensaje tipo="Alfanumérico" Información relativa al negocio de aplicación, lo que se que quiere notificar.
            /// email.InfoDesc tipo="Alfanumérico" Breve descripción sobre información adicional o de un link.
            /// email.InfoDato tipo="Alfanumérico" Información del dato adicional.
            /// email.InfoLink tipo="Alfanumérico" Dato opcional, hipervínculo de la aplicación.
            /// email.Firma tipo="Alfanumérico" Dato obligatorio, la persona que firma el mail enviado.
            /// email.Ente tipo="Alfanumérico" Ente que efectúa la notificación.
            /// email.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// email.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Respuesta</returns>
            public static String Enviar_Direccion
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar_Direccion";
                }
            }

            /// <summary>
            /// Envia un email a un ciudadano a través de la dirección de correo 
            /// </summary>
            /// <param name="EmailMasivo">
            /// email.IdAplicacion tipo="Numérico" Identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// email.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// email.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// email.SesionHash tipo="Alfanumérico" Valor que se obtiene al autenticar la aplicación (Registrar_Aplicacion).
            /// email.DireccionEmail tipo="Alfanumérico" Dirección de correo del usuario al que se quiere enviar el mail.
            /// email.Asunto tipo="Alfanumérico" Compuesto por una parte fija, igual para todos los emails, con un texto corto.
            /// email.Mensaje tipo="Alfanumérico" Información relativa al negocio de aplicación, lo que se que quiere notificar.
            /// email.Notificacion tipo="Alfanumérico" Ente que efectúa la notificación.
            /// </param>
            /// <returns>Respuesta</returns>
            public static String Enviar_Masivo
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar_Masivo";
                }
            }

            /// <summary>
            /// Envia un email a un ciudadano a través de la dirección de correo
            /// </summary>
            /// <param name="email">
            /// email.SesionHash tipo="Alfanumérico" Valor que se obtiene de la cookie del operador. Nombre de la cookie "CiDi".
            /// email.DireccionEmail tipo="Alfanumérico" Dirección de correo del usuario al que se quiere enviar el mail.
            /// email.Asunto tipo="Alfanumérico" Compuesto por una parte fija, igual para todos los emails, con un texto corto.
            /// email.BodyHTML tipo="Alfanumérico" Información relativa al negocio de aplicación, lo que se que quiere notificar.
            /// email.Firma tipo="Alfanumérico" Dato obligatorio, la persona que firma el mail enviado.
            /// email.Ente tipo="Alfanumérico" Ente que efectúa la notificación.
            /// email.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// email.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Respuesta</returns>
            public static String Enviar_Direccion_Email
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar_Direccion_Email";
                }
            }

            /// <summary>
            /// Envia un email a un ciudadano a través de la dirección de correo 
            /// </summary>
            /// <param name="email">
            /// email.Id_App tipo="Numérico" Identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// email.SesionHash tipo="Alfanumérico" Valor que se obtiene al autenticar la aplicación (Registrar_Aplicacion).
            /// email.Cuil tipo="Alfanumérico" CUIL del usuario al que se quiere enviar el mail.
            /// email.DireccionEmail tipo="Alfanumérico" Dirección de correo del usuario al que se quiere enviar el mail.
            /// email.Asunto tipo="Alfanumérico" Compuesto por una parte fija, igual para todos los emails, con un texto corto.
            /// email.Mensaje tipo="Alfanumérico" Información relativa al negocio de aplicación, lo que se que quiere notificar.
            /// email.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// email.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Respuesta</returns>
            public static String Enviar_Cuil_Direccion
            {
                get
                {
                    return Config.APIComunicacion + "api/Email/Enviar_Cuil_Direccion";
                }
            }
        }

        public class Historial
        {
            /// <summary>
            /// Trae las comunicaciones de una aplicación, para un usuario, en un rango de fechas
            /// </summary>
            /// <param name="entrada">
            /// entrada.SesionHash tipo="Alfanumérico" Valor que se obtiene al autenticar la aplicación (Registrar_Aplicacion).
            /// entrada.Cuil tipo="Alfanumérico" CUIL del usuario al que se quiere enviar el mail.
            /// entrada.FechaDesde tipo="Alfanumérico" CUIL del usuario al que se quiere enviar el mail.
            /// entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie del operador. Nombre de la cookie "CiDi".
            /// entrada.FechaHasta tipo="Alfanumérico" Compuesto por una parte fija, igual para todos los emails, con un texto corto.
            /// </param>
            /// <returns>RespuestaHistorial</returns>
            public static String Get
            {
                get
                {
                    return Config.APIComunicacion + "api/Historial/Get";
                }
            }
        }
    }

    public class APIDocumentacion
    {
        public class Documentacion
        {
            /// <summary>
            /// Obtención de los documentos del Ciudadano Digital a través del Cuil
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.CUIL tipo="Alfanumérico" CUIL del usuario.
            /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>RespuestaList</returns>
            public static String Obtener_Documentacion_Usuario
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Obtener_Documentacion_Usuario";
                }
            }

            /// <summary>
            /// Obtención de los documentos del Ciudadano Digital a través de la cookie
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>RespuestaList</returns>
            public static String Obtener_Documentacion_Sesion
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Obtener_Documentacion_Sesion";
                }
            }

            /// <summary>
            /// Obtención de un listado de tipos de documentos permitidos en la plataforma Ciudadano Digital
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>RespuestaTipoDoc</returns>
            public static String Obtener_Tipo_Documentos
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Obtener_Tipo_Documentos";
                }
            }

            /// <summary>
            /// Obtención de una vista previa del listado de documentos del Ciudadano Digital
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital
            /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital
            /// Entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")
            /// Entrada.Cuil tipo="Alfanumérico" CUIL del usuario al que se requiere obtener el documento.
            /// Entrada.DiccionarioDocumentos tipo="Numérico" Diccionario de datos con el identificador del documento y el tipo de documento IdDocumento;IdTipoDocumento
            /// </param>
            /// <returns>RespuestaVistaPrevia</returns>
            public static String Obtener_Vista_Previa
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Obtener_Vista_Previa";
                }
            }

            /// <summary>
            /// Obtención de un documento específico del Ciudadano Digital
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital
            /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital
            /// Entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff")
            /// Entrada.Documentacion.IdDocumento tipo="Numérico" Identificador del documento que se quiere obtener.
            /// Entrada.Cuil tipo="Alfanumérico" CUIL del usuario al que se requiere obtener el documento.
            /// </param>
            /// <returns>RespuestaDoc</returns>
            public static String Obtener_Documento
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Obtener_Documento";
                }
            }

            /// <summary>
            /// Obtención de la foto de perfil del Ciudadano Digital (Nivel 2)
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// Entrada.HashCookieOperador tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
            /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// Entrada.CUIL tipo="Alfanumérico" CUIL del usuario.
            /// </param>
            /// <returns>RespuestaList</returns>
            public static String Obtener_Foto_Perfil
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Obtener_Foto_Perfil";
                }
            }

            /// <summary>
            /// Incorporar un documento en la plataforma al Ciudadano Digital
            /// </summary>
            /// <param name="entrada">
            /// Entrada.IdAplicacion El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital
            /// Entrada.Contrasenia Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital
            /// Entrada.TokenValue Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// Entrada.TimeStamp TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff");
            /// Entrada.CUIL CUIL del usuario al que se le asocia el documento.
            /// Entrada.HashCookie Valor de la cookie del operador.
            /// Entrada.Documentacion.Imagen Array de byte del documento, cifrado por la librería CryptoManager.
            /// Entrada.Documentacion.Extension Extensión del documento.
            /// Entrada.Documentacion.FechaVencimiento Fecha vencimiento del documento.
            /// Entrada.Documentacion.IdTipo Identificador del tipo documento.
            /// Entrada.Documentacion.Descripcion Descripción del documento.
            /// </param>
            /// <returns>RespuestaDocInsercion</returns>
            public static String Guardar_Documento
            {
                get
                {
                    return Config.APIDocumentacion + "api/Documentacion/Guardar_Documento";
                }
            }
        }
    }

    public class APIMobile
    {
        public class Mobile
        {
            /// <summary>
            /// Inicio de sesión y obtención del usuario de la cuanta de Ciudadano Digital
            /// </summary>
            /// <param name="Entrada">
            /// name="EntradaLogin.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="EntradaLogin.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
            /// name="EntradaLogin.CUIL" tipo="Alfanumérico" long="30">CUIL del usuario.
            /// name="EntradaLogin.ContraseniaUsuario" tipo="Alfanumérico" long="30">Contraseña del usuario
            /// name="EntradaLogin.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. 
            /// Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
            /// name="EntradaLogin.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. 
            /// El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.ToString("yyyyMMddHHmmssfff").
            /// </param>
            /// <returns>Usuario</returns>
            public static String Login_Usuario
            {
                get
                {
                    return Config.APIMobile + "api/Mobile/Login_Usuario";
                }
            }
        }
    }
}