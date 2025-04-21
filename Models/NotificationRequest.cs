using System.Text.Json;

namespace NotificationService.Models
{

    public class NotificationRequest
    {
        public string MessageId { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public string TemplateName { get; set; } = string.Empty;

        public Dictionary<string, string> Variables { get; set; } = new(); 
    }

}
