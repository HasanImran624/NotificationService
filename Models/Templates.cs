namespace NotificationService.Models
{
    public class Template
    {
        public string? Subject { get; set; }
        public string Body { get; set; } = string.Empty;
    }

    public class TemplateMap : Dictionary<string, Template> { }
}
