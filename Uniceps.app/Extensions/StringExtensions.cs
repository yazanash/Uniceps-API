namespace Uniceps.app.Extensions
{
    public static class StringExtensions
    {
        public static string ToUrlSlug(this string value)
        {
            if (string.IsNullOrEmpty(value)) return Guid.NewGuid().ToString();
           string slug = System.Text.RegularExpressions.Regex.Replace(value.ToLowerInvariant(), @"[^a-z0-9\s-_]", "")
                .Trim()
                .Replace(" ", "-");

            return !string.IsNullOrEmpty(slug) ? slug : Guid.NewGuid().ToString();
        }
    }
}
