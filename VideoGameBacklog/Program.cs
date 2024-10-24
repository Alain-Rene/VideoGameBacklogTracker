using Services;
using VideoGameBacklog.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            //replace localhost with yours            //also add your deployed website
            policy.WithOrigins("http://localhost:4200",
                                "https://red-mud-08705140f.5.azurestaticapps.net").AllowAnyMethod().AllowAnyHeader();
        });
});
//WINDOWS

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<VideoGameDetailsService>();
// Register the VideoGameBacklogDbContext if you haven't already
builder.Services.AddScoped<VideoGameBacklogDbContext>();
builder.Services.AddScoped<UserService>();


//builder.Services.AddCors(options =>
//{
//   options.AddPolicy("AllowLocalhost4200",
//       builder => builder.WithOrigins("http://localhost:4200")
//                         .AllowAnyMethod()
//                         .AllowAnyHeader());
//});
//////ALAIN


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


//app.UseCors("AllowLocalhost4200"); //ALAIN
                                   //Cors
app.UseCors(); //WINDOWS

app.MapControllers();

app.Run();
