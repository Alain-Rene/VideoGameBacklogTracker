using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//cors
// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(
//         policy =>
//         {
//             //replace localhost with yours            //also add your deployed website
//             policy.WithOrigins("http://localhost:4200",
//                                 "https://MyChatRoom.com").AllowAnyMethod().AllowAnyHeader();
//         });
// });
// //WINDOWS

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<VideoGameDetailsService>();


builder.Services.AddCors(options =>
{
   options.AddPolicy ("AllowLocalhost4200",
       builder => builder.WithOrigins("http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader());
});
//ALAIN


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseCors("AllowLocalhost4200"); //ALAIN
//Cors
// app.UseCors(); //WINDOWS

app.MapControllers();

app.Run();
