using System.Text.RegularExpressions;
using System.Text;

namespace Games.Services.Common
{
    public static class AppConst
    {
        public static readonly List<string> colors = new List<string>() {
            "rgba(255, 0, 0, 1.0)",     // Red
            //"rgba(0, 255, 0, 1.0)",     // Green
            "rgba(0, 0, 255, 1.0)",     // Blue
            //"rgba(255, 255, 0, 1.0)",   // Yellow
            "rgba(0, 255, 255, 1.0)",   // Cyan
            "rgba(255, 0, 255, 1.0)",   // Magenta
            "rgba(255, 165, 0, 1.0)",   // Orange
            "rgba(128, 0, 128, 1.0)",   // Purple
            "rgba(0, 255, 127, 1.0)",   // Lime
            "rgba(255, 192, 203, 1.0)", // Pink
            "rgba(0, 128, 128, 1.0)",   // Teal
            "rgba(165, 42, 42, 1.0)",   // Brown
            "rgba(75, 0, 130, 1.0)",    // Indigo
            "rgba(128, 128, 0, 1.0)",   // Olive
            "rgba(255, 99, 71, 1.0)",   // Tomato
            "rgba(255, 69, 0, 1.0)",    // OrangeRed
            "rgba(210, 105, 30, 1.0)",  // Chocolate
            "rgba(0, 100, 0, 1.0)",     // DarkGreen
            "rgba(75, 83, 32, 1.0)",    // Moss
            "rgba(138, 43, 226, 1.0)",  // BlueViolet
            "rgba(0, 206, 209, 1.0)",   // DarkTurquoise
            "rgba(46, 139, 87, 1.0)",   // SeaGreen
            "rgba(255, 215, 0, 1.0)",   // Gold
            "rgba(64, 224, 208, 1.0)",  // Turquoise
            "rgba(199, 21, 133, 1.0)",  // MediumVioletRed
            "rgba(100, 149, 237, 1.0)", // CornflowerBlue
            "rgba(233, 150, 122, 1.0)", // DarkSalmon
            "rgba(244, 164, 96, 1.0)",  // SandyBrown
            "rgba(219, 112, 147, 1.0)", // PaleVioletRed
            "rgba(255, 228, 181, 1.0)"  // Moccasin
        };

        public static string GetSlug(string text)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string slug = text.Normalize(NormalizationForm.FormD).Trim().ToLower();

            slug = regex.Replace(slug, String.Empty)
              .Replace('\u0111', 'd').Replace('\u0110', 'D')
              .Replace(",", "-").Replace(".", "-").Replace("!", "").Replace(": ", "-")
              .Replace("(", "").Replace(")", "").Replace(";", "-")
              .Replace("/", "-").Replace("%", "").Replace(" &", "")
              .Replace("?", "").Replace('"', '-').Replace(' ', '-');

            return slug;
        }
    }
}
