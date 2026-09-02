using System.Text.Json;
using Uniceps.app.Helpers.UniFileDtos;

namespace Uniceps.app.Services
{
    public interface IUniFileParserService
    {
        (UniFile<RoutineExportDto> ParsedFile, string RawDataJson) ParseRoutineJson(string jsonContent);
        Task<(UniFile<RoutineExportDto> ParsedFile, string RawDataJson)> ParseRoutineUniFileAsync(Stream fileStream);
    }
    public class UniFileParserService : IUniFileParserService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public (UniFile<RoutineExportDto> ParsedFile, string RawDataJson) ParseRoutineJson(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                throw new ArgumentException("نص الـ JSON فارغ أو غير صالح.");
            }

            using var jsonDoc = JsonDocument.Parse(jsonContent);
            var root = jsonDoc.RootElement;

            if (!root.TryGetProperty("Data", out var dataElement))
            {
                throw new InvalidDataException("صيغة الملف غير صالحة: عنصر Data مفقود.");
            }

            string rawDataJson = root.GetRawText();

            var uniFile = JsonSerializer.Deserialize<UniFile<RoutineExportDto>>(jsonContent, _jsonOptions);

            if (uniFile == null || uniFile.Data == null)
            {
                throw new InvalidDataException("فشل قراءة بيانات البرنامج التدريبي من ملف الـ JSON.");
            }

            return (uniFile, rawDataJson);
        }
        public async Task<(UniFile<RoutineExportDto> ParsedFile, string RawDataJson)> ParseRoutineUniFileAsync(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            string jsonContent = await reader.ReadToEndAsync();

            return ParseRoutineJson(jsonContent);
        }
    }
}
