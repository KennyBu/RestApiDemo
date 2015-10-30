using Ninject.Modules;
using RestSharp;

namespace RestApiDemoConsoleApplication.Bootstrap
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
            var config = new Config();

            Bind<IRestClient>().To<RestClient>().WithConstructorArgument(config.GetBaseUrl());   
        }
    }
}