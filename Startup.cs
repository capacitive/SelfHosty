using System.Web.Http;
using Owin;

namespace SelfHosty
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "MoviesApi",
                routeTemplate: "api/movies/{id}",
                defaults: new { controller = "movies", id = RouteParameter.Optional });

            appBuilder.UseWebApi(config);
        }
    }
}
