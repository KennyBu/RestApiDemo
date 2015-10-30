namespace RestApiDemoConsoleApplication
{
    public class Config : IConfig
    {
        private const string BaseUrl = "http://kkirtland.pythonanywhere.com";

        public string GetBaseUrl()
        {
            return BaseUrl;
        }
    }
}
