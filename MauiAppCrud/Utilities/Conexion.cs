
namespace MauiAppCrud.Utilties
{
    public static class Conexion
    {
        public static string ObtenerRuta(string nombreDB)
        {
            string ruta = string.Empty;
            string env = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                env = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                ruta = Path.Combine(env, nombreDB);
            } else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                env = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                ruta = Path.Combine (env, "..","Library", nombreDB);
            }

            return ruta;
        }
    }
}
