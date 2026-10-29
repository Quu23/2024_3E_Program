namespace ScoreServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/score", (string name) => "Hello " + name);

            app.Run("http://*:5000");
        }
    }
}
