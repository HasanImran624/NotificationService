using MassTransit;
using NotificationService.Messages;
using NotificationService.Models;
using NotificationService.Services.Push;
using NotificationService.Services.Sms;
using NotificationService.Services.Templates;
using System.Text.Json;

namespace NotificationService.Services.Dispatcher
{
    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITemplateService _templateService;

        public NotificationDispatcher(IPublishEndpoint publishEndpoint, ITemplateService templateService)
        {
            _publishEndpoint = publishEndpoint;
            _templateService = templateService;
        }

        public async Task DispatchAsync(NotificationRequest request)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var template = _templateService.GetTemplate(request.TemplateName);

            if (template == null)
                throw new Exception("Template not found.");

            var body = _templateService.ApplyTemplate(template.Body, request.Variables);
            var subject = _templateService.ApplyTemplate(template.Subject ?? "", request.Variables);


            switch (request.Type.ToLower())
            {
                case "email":

                    var email = new EmailNotification
                    {
                        MessageId = request.MessageId,
                        To = request.Variables["to"],
                        Subject = subject,
                        Body = body
                    };
                    await _publishEndpoint.Publish(email);
                    break;

                case "sms":
                    var sms = new SmsNotification
                    {
                        MessageId = request.MessageId,
                        PhoneNumber = request.Variables["to"],
                        Body = body

                    };
                    if (request.TemplateName.ToLower().Contains("otp") || request.Variables.ContainsKey("Ttl"))
                    {
                        sms.IsOtp = true;
                        sms.TtlSeconds = int.Parse(request.Variables["Ttl"]);
                        sms.CreatedAt = DateTime.UtcNow;
                    }
                    await _publishEndpoint.Publish(sms);
                    break;

                case "push":
                    var push = new PushNotification
                    {
                        Title = subject,
                        Body = body
                    };
                    await _publishEndpoint.Publish(push);
                    break;

                default:
                    throw new ArgumentException("Unsupported notification type.");
            }
        }
    }

}
