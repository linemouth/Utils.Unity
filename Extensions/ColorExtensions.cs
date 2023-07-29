using Unity.VisualScripting;
using UnityEngine;
using Utils;

namespace Utils.Unity
{
    public static class ColorExtensions
    {
        public static UnityEngine.Color ToUnityColor(this IColor color)
        {
            Rgb rgb = color.ToRgb();
            return new UnityEngine.Color(rgb.r, rgb.g, rgb.b, rgb.a);
        }
        public static Argb ToArgb(this UnityEngine.Color color) => (Argb)color.ToRgb();
        public static Rgb ToRgb(this UnityEngine.Color color) => new Rgb(color.r, color.g, color.b, color.a);
        public static Hsl ToHsl(this UnityEngine.Color color) => (Hsl)color.ToRgb();
        public static Hsv ToHsv(this UnityEngine.Color color) => (Hsv)color.ToRgb();
        public static Cmyk ToCmyk(this UnityEngine.Color color) => (Cmyk)color.ToRgb();
        public static Xyl ToXyl(this UnityEngine.Color color) => (Xyl)color.ToRgb();
    }
}