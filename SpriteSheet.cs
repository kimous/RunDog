using System.Drawing.Imaging;
using System.Reflection;

namespace RunDog
{
    /// <summary>
    /// Gère une feuille de sprites (Sprite Sheet) pour une animation.
    /// Charge une image, la découpe, et génère des icônes de haute qualité.
    /// </summary>
    public class SpriteSheet
    {
        public string Name { get; }
        public Icon[] Icons { get; }

        public SpriteSheet(string resourceName, int frameCount, string displayName)
        {
            Name = displayName;
            Icons = new Icon[frameCount];

            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)!)
            {
                if (stream == null)
                    throw new FileNotFoundException($"La ressource '{resourceName}' est introuvable.");

                using (var bitmap = new Bitmap(stream))
                {
                    int frameWidth = bitmap.Width / frameCount;
                    int frameHeight = bitmap.Height;

                    for (int i = 0; i < frameCount; i++)
                    {
                        var frameRect = new Rectangle(i * frameWidth, 0, frameWidth, frameHeight);
                        using (var frameBitmap = bitmap.Clone(frameRect, PixelFormat.Format32bppArgb))
                        {
                            // --- NOUVELLE LOGIQUE ---
                            // On trouve les limites réelles du contenu dessiné pour "rogner" la transparence.
                            Rectangle contentBounds = FindContentBounds(frameBitmap);

                            if (contentBounds.IsEmpty)
                            {
                                // Si l'image est vide, on crée une icône vide pour ne pas planter.
                                using(var emptyBitmap = new Bitmap(32,32))
                                {
                                    Icons[i] = Icon.FromHandle(emptyBitmap.GetHicon());
                                }
                                continue;
                            }
                            
                            // On crée un canvas final de 32x32.
                            using (var finalBitmap = new Bitmap(32, 32))
                            using (var g = Graphics.FromImage(finalBitmap))
                            {
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                                
                                // On dessine uniquement la partie contenant le chat (contentBounds)
                                // en l'étirant sur tout le canvas final de 32x32.
                                g.DrawImage(frameBitmap, new Rectangle(0, 0, 32, 32), contentBounds, GraphicsUnit.Pixel);

                                IntPtr hIcon = finalBitmap.GetHicon();
                                Icons[i] = Icon.FromHandle(hIcon);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Analyse un bitmap et retourne un rectangle qui entoure la zone non-transparente.
        /// </summary>
        private Rectangle FindContentBounds(Bitmap bmp)
        {
            int left = bmp.Width, right = 0, top = bmp.Height, bottom = 0;

            // Utiliser LockBits est beaucoup plus rapide que GetPixel.
            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* scan0 = (byte*)data.Scan0;
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        // L'alpha est le 4ème byte dans un pixel 32bpp ARGB.
                        byte alpha = scan0[(y * data.Stride) + (x * 4) + 3];
                        if (alpha > 10) // On considère tout alpha > 10 comme visible.
                        {
                            if (x < left) left = x;
                            if (x > right) right = x;
                            if (y < top) top = y;
                            if (y > bottom) bottom = y;
                        }
                    }
                }
            }
            bmp.UnlockBits(data);

            if (right < left || bottom < top)
                return Rectangle.Empty; // L'image est entièrement transparente.

            return new Rectangle(left, top, right - left + 1, bottom - top + 1);
        }
    }
}