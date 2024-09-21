using System.Web.Http;

namespace MessageBoard.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Rotas da API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Configuração para retornar JSON por padrão
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        }
    }
}
