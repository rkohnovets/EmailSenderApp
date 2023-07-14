using EmailSenderApp.Configuration;
using EmailSenderApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// here is what I added, other lines are from standard template
# region Added
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddTransient<IMailSender, EmailSender>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddTransient<IMailsRepository, MailsRepository>(provider => new MailsRepository(connectionString!));
# endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
