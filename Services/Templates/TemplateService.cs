using NotificationService.Models;
using System.Text.Json;

namespace NotificationService.Services.Templates
{
    public interface ITemplateService
    {
        Template? GetTemplate(string templateName);
        string ApplyTemplate(string content, Dictionary<string, string> data);
    }


    public class TemplateService : ITemplateService
    {
        private readonly TemplateMap _templates;

        public TemplateService()
        {
            var json = File.ReadAllText("Templates/templates.json");
            _templates = JsonSerializer.Deserialize<TemplateMap>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new TemplateMap();
        }

        public Template? GetTemplate(string templateName)
        {
            _templates.TryGetValue(templateName, out var template);
            return template;
        }

        public string ApplyTemplate(string content, Dictionary<string, string> data)
        {
            foreach (var kvp in data)
            {
                content = content.Replace("{" + kvp.Key + "}", kvp.Value);
            }

            return content;
        }
    }
}

