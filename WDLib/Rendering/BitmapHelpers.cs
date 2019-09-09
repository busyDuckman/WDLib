/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox;
using WD_toolbox.Data.DataStructures;
using WD_toolbox.Maths.Range;
using WD_toolbox.Rendering.Colour;
using WD_toolbox.Rendering.FormattedText;
using WD_toolbox.Rendering.Patterns;

namespace WD_toolbox.Rendering
{
    public static class BitmapHelpers
    {
        public static Bitmap GenerateDebugImage(int width, int height, string message)
        {
            GradientPattern pattern = new GradientPattern();
            pattern.bottomLeft = (Color)QColor.generateRandomOpaque(128, 255);
            pattern.bottomRight = (Color)QColor.generateRandomOpaque(128, 255);
            pattern.topLeft=  Color.Fuchsia;
            pattern.topRight = Color.Fuchsia;

            Bitmap bmp = pattern.makeBitmap(width, height);
            GDIPlusRenderer r =new GDIPlusRenderer(bmp);
            float fontSize = Math.Min(width, height)/10.0f;
            fontSize = Range.clamp(fontSize, 10, 45);
            using (Font f = new Font("Arial", fontSize, FontStyle.Bold))
            {
                TextFormat tf = new TextFormat(f, Color.Black, true, Color.White, false);
                tf.render(r, message, 0, height / 2);
                bmp = r.RenderTargetAsGDIBitmap();
            }
            

            return bmp;
        }

        public static Bitmap GenerateMessageInCircle(int width, int height, Color foreCol, Color backCol, string message, Font font = null)
        {
            GDIPlusRenderer r = new GDIPlusRenderer(width, height);
            r.SetHighQuality(true);
            //circle
            int lineWidth = Range.clamp(Math.Min(width, height) / 128, 1, 10);
            int diamater = Math.Max(Math.Min(width, height) - lineWidth - 1, 1);
            int cX = width/2;
            int cY = height/2;
            r.FillCircle(backCol, cX, cY, diamater / 2);
            r.DrawCircle(foreCol, lineWidth, cX, cY, diamater / 2);

            //text
            if (!String.IsNullOrWhiteSpace(message))
            {
                
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                Font _font  = font;

                if (_font == null)
                {
                    float fontSize = Math.Min(width, height) / 10.0f;
                    fontSize = Range.clamp(fontSize, 10, 45);
                    _font = new Font("Arial", fontSize, FontStyle.Bold);
                }

                try
                {
                    r.DrawStringAligned(foreCol, message, _font, cX, cY, HorizontalAlignment.Centre, VerticalAlignment.Centre);
                }
                finally
                {
                    if(font == null)
                    {
                        //_font was created in this method, not supplied via font
                        _font.TryDispose();
                    }
                }                
            }

            return r.RenderTargetAsGDIBitmap();
        }

        public static Why Save(Bitmap b, BinaryWriter s)
        {
            return Why.FromTry(delegate()
            {
                bool notNull = b != null;
                
                s.Write(notNull);

                if (notNull)
                {
                    using(MemoryStream ms = new MemoryStream())
                    {
                        b.Save(ms, ImageFormat.Bmp);
                        byte[] data = ms.ToArray();
                        s.Write(data.Length);
                        s.Write(data);
                    }
                };
            });
        }

        public static Why Load(BinaryReader r, out Bitmap b)
        {
            try
            {
                bool notNull = r.ReadBoolean();
                if (notNull)
                {
                    int len = r.ReadInt32();
                    byte[] data = r.ReadBytes(len);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        b = new Bitmap(ms);
                        return true;
                    }
                }
                else
                {
                    b = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                b = null;
                return Why.FalseBecause(ex);
            }
        }
    }
}
