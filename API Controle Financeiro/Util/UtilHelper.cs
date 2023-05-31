using System.Security.Cryptography;
using System.Text;

namespace ControleFinanceiroAPI.Util;

public class UtilHelper
{
    public static int SO { get; set; }

    public static void myLogTxtRequest(string texto, IConfiguration _config, HttpContext httpContext)
    {
        try
        {
            if (SO == 1)
            {
                string pathLogAPI = _config["LogApi:UNIX"];
                StreamWriter logFile = File.AppendText(pathLogAPI);
                logFile.WriteLine(DateTime.Now.ToString("yyyy'-'MM'-'dd HH:mm:ss") + " ======> " + texto + " - IP Client: (" + httpContext.Connection.RemoteIpAddress?.ToString().Replace("::ffff:", "") + ")");
                logFile.Close();
            }
            else if (SO == 0)
            {
                string pathLogAPI = _config["LogApi:WINDOWS"];
                StreamWriter logFile = File.AppendText(pathLogAPI);
                logFile.WriteLine(DateTime.Now.ToString("yyyy'-'MM'-'dd HH:mm:ss") + " ======> " + texto + " - IP Client: (" + httpContext.Connection.RemoteIpAddress?.ToString().Replace("::ffff:", "") + ")");
                logFile.Close();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public static void myLogTxtSimple(string texto, IConfiguration _config)
    {
        try
        {
            if (SO == 1)
            {
                string pathLogAPI = _config["LogApi:UNIX"];
                StreamWriter logFile = File.AppendText(pathLogAPI);
                logFile.WriteLine(DateTime.Now.ToString("yyyy'-'MM'-'dd HH:mm:ss") + " ======> " + texto);
                logFile.Close();
            }
            else if (SO == 0)
            {
                string pathLogAPI = _config["LogApi:WINDOWS"];
                StreamWriter logFile = File.AppendText(pathLogAPI);
                logFile.WriteLine(DateTime.Now.ToString("yyyy'-'MM'-'dd HH:mm:ss") + " ======> " + texto);
                logFile.Close();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public static string MyToMD5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
