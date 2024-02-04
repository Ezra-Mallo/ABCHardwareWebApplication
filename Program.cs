namespace ABCHardwareWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // containers
            builder.Services.AddRazorPages();
            builder.Services.AddSession();



            var app = builder.Build();
            // configure the HTTP request 
            if (!app.Environment.IsDevelopment())       //check for production environment
            {
                app.UseDeveloperExceptionPage();        // not for production, remove for final release
                //app.UseExceptionHandler("/Error");       //customized error page, use for final release
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();

            app.MapRazorPages();

            app.Run();
        }
    }
}