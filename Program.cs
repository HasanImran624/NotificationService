using MassTransit;
using NotificationService.Consumers;
using NotificationService.Services.Dispatcher;
using NotificationService.Services.Email;
using NotificationService.Services.Idempotency;
using NotificationService.Services.Push;
using NotificationService.Services.Sms;
using NotificationService.Services.Templates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailConsumer>();
    x.AddConsumer<SmsConsumer>();
    x.AddConsumer<PushConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("email_queue", e => e.ConfigureConsumer<EmailConsumer>(context));
        cfg.ReceiveEndpoint("sms_queue", e => e.ConfigureConsumer<SmsConsumer>(context));
        cfg.ReceiveEndpoint("push_queue", e => e.ConfigureConsumer<PushConsumer>(context));
    });
});
// Email Providers
builder.Services.AddScoped<IEmailProvider, SendGridEmailProvider>();
builder.Services.AddScoped<IEmailProvider, MailgunEmailProvider>();

// SMS Providers
builder.Services.AddScoped<ISmsProvider, TwilioSmsProvider>();
builder.Services.AddScoped<ISmsProvider, NexmoSmsProvider>();

// Push Providers
builder.Services.AddScoped<IPushProvider, FirebasePushProvider>();
builder.Services.AddScoped<IPushProvider, OneSignalPushProvider>();

builder.Services.AddScoped<EmailProviderManager>();
builder.Services.AddScoped<PushProviderManager>();
builder.Services.AddScoped<SmsProviderManager>();

builder.Services.AddSingleton<IIdempotencyService, InMemoryIdempotencyService>();
builder.Services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
builder.Services.AddSingleton<ITemplateService, TemplateService>();


builder.Services.AddMassTransitHostedService();

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


