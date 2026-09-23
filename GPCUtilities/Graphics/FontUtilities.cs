using System;
using System.Drawing.Text;
using System.Linq;
using GPC.Utilities.Extensions;

namespace GPC.Utilities.Graphics
{
    public static class FontUtilities
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontName">The font name</param>
        /// <returns>True if font is installaed in the system</returns>
        /// <exception cref="ArgumentException">If string is null or empty</exception>
        public static bool IsFontInstalled(string fontName)
        {
            if (string.IsNullOrEmpty(fontName))
                throw new ArgumentException("Font is null or empty");

            using (var fontCollection = new InstalledFontCollection())
            {
                fontName = fontName.FirstLetterToUpperCase();
                return fontCollection.Families.Any(f => f.Name == fontName);
            }
        }
    }
}
