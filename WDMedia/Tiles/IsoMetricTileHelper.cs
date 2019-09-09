/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox;
using WD_toolbox.Data;
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Rendering;

namespace WDMedia.Tiles
{
    public class IsoMetricTileGenerationSettings
    {
        public enum IsoMetricMaskType
        {
            SinglePixelCornerRoundUp, 
            //SinglePixelCornerRoundDown, 
            TwoPixelCorner
        };


        public enum IsoMetricMapping
        {
            RotateLeftAndScale, RotateRightAndScale
        };

        public enum IsoMetricBorderProtection
        {
            RepeatEdge,        ///edge pixel is repeted
            MirrorAboutEdge,   ///pixels before edge repeat after edge
            ToroidalWrap       ///teselate
        };

        public IsoMetricMaskType MaskType { get; set; }
        public IsoMetricMapping Mapping { get; set; }
        public IsoMetricBorderProtection BorderProtection { get; set; }
        public int BorderSize { get; set; }
        public double HeightRatio { get; set; }

        public IsoMetricTileGenerationSettings(IsoMetricMaskType maskType, IsoMetricMapping mapping, IsoMetricBorderProtection borderProtection)
        {
            this.MaskType = maskType;
            this.Mapping = mapping;
            this.BorderProtection = borderProtection;
            BorderSize = 1;
            HeightRatio = 0.5;
        }

        /// <summary>
        /// Function to generate an alpha chanel.
        /// </summary>
        /// <returns></returns>
        public Func<int, int, Size, byte> GenerateMaskMethod()
        {
            switch (MaskType)
            {
                case IsoMetricMaskType.SinglePixelCornerRoundUp:
                    return (x, y, size) => { return (flipAbout(x, size.Width / 2) > flipAbout(y, size.Height / 2)) ? (byte)0 : (byte)255; };

           //     case IsoMetricMaskType.SinglePixelCornerRoundDown:
             //       return (x, y, size) => { return (flipAbout(x, size.Width / 2) > flipAbout(y, size.Height / 2)) ? 0 : 255; };

                case IsoMetricMaskType.TwoPixelCorner:
                    return (x, y, size) => { return (flipAbout(x, size.Width / 2) >= flipAbout(y, size.Height / 2)) ? (byte)0 : (byte)255; };
                default:
                    return null;
            }
        }

        private int flipAbout(int x, int middle)
        {
            return (middle - x) + middle + 1;
        }
    }

    public static class IsoMetricTileHelper
    {
        

        public static Bitmap makeIsometric(Bitmap b, IsoMetricTileGenerationSettings settings)
        {
            Bitmap iso = addBorder(b, settings);
            iso = mapToIsoMetric(iso, settings);
            iso = removeBorder(iso, settings);
            //iso = applyMask(iso, settings);
            return iso;
        }

        private static Bitmap applyMask(Bitmap b, IsoMetricTileGenerationSettings settings)
        {
            //get mask
            var maskFunc = settings.GenerateMaskMethod();

            //apply mask
            byte[] bytes = b.GetCopyOfBytesARGB32();
            for(int y=0; y<b.Height; y++)
            {
                int linePos = y*b.Width;
                for (int x = 0; x < b.Width; x++)
                {
                    int alphaPos = (linePos + x) * 4;

                    bytes[alphaPos] = maskFunc(x, y, b.Size);
                }
            }

            //return new image
            Bitmap r = b.GetBlankClone();
            r.SetPixelsFromBytesARGB32(bytes);
            return r;
        }

        private static Bitmap removeBorder(Bitmap b, IsoMetricTileGenerationSettings settings)
        {
            int margin = settings.BorderSize;
            int newWidth = b.Width-(margin*2);
            int newHeight = b.Width-(margin*2);
            return b.GetSubImageClipped(margin, margin, newWidth, newHeight);
        }

        private static Bitmap mapToIsoMetric(Bitmap b, IsoMetricTileGenerationSettings settings)
        {
            int newWidth = (int)Math.Ceiling(Math.Sqrt(2*(Math.Pow(Math.Max(b.Width, b.Height), 2))));
            //keep the width even
            if((newWidth % 2) != 0)
            {
                newWidth++;
            }
            int newHeight = newWidth;
            int halfNewWidth = newWidth/2;
            int halfNewHeight = newWidth/2;
            IRenderer r = IRendererFactory.GetPreferredRenderer(newWidth, newHeight);
            TransMatrix2D  rot = TransMatrix2D.FromRotation(Math.PI/4.0, new Point2D(halfNewWidth, halfNewHeight));
            r.SetTransform(rot);
            r.DrawImage(b,
                        halfNewWidth - (b.Width / 2) + 1,
                        halfNewHeight - (b.Height / 2) + 1);

            Bitmap rotated =  r.RenderTargetAsGDIBitmap();

            //apply squash
            if (Math.Abs(settings.HeightRatio) > 0.0001)
            {
                //return new Bitmap(rotated, rotated.Width, rotated.Height / 2);
                return new Bitmap(rotated, b.Width, b.Height / 2);
            }
            return rotated;
        }

        private static Bitmap addBorder(Bitmap b, IsoMetricTileGenerationSettings settings)
        {
            int margin = settings.BorderSize;
            int newWidth = b.Width + (margin * 2);
            int newHeight = b.Width + (margin * 2);
            IRenderer r = IRendererFactory.GetPreferredRenderer(newWidth, newHeight);

            switch (settings.BorderProtection)
            {
                case IsoMetricTileGenerationSettings.IsoMetricBorderProtection.RepeatEdge:
                    //draw top edge streched across top margin
                    Rectangle srcEdge = new Rectangle(0, 0, b.Width, 1);
                    Rectangle destRect = new Rectangle(margin, 0, b.Width, margin);
                    r.DrawImage(b, destRect, srcEdge);
                    //r.FillRectangle(Color.Red, destRect);

                    //draw bottom edge
                    srcEdge = new Rectangle(0, b.Height-1, b.Width, 1);
                    destRect = new Rectangle(margin, b.Height+margin, b.Width, margin);
                    r.DrawImage(b, destRect, srcEdge);
                    //r.FillRectangle(Color.Red, destRect);

                    //draw left edge
                    srcEdge = new Rectangle(0, 0, 1, b.Height);
                    destRect = new Rectangle(0, margin, margin, b.Height);
                    r.DrawImage(b, destRect, srcEdge);
                    //r.FillRectangle(Color.Red, destRect);

                    //draw right edge
                    srcEdge = new Rectangle(b.Width-1, 0, 1, b.Height);
                    destRect = new Rectangle(b.Width+margin, margin, margin, b.Height);
                    r.DrawImage(b, destRect, srcEdge);
                    //r.FillRectangle(Color.Red, destRect);

                    //draw corners
                    int xPos = margin + b.Width;
                    int yPos = margin + b.Height;
                    r.FillRectangle(b.GetPixel(0, 0), new Rectangle(0, 0, margin, margin));
                    r.FillRectangle(b.GetPixel(b.Width-1, 0), new Rectangle(xPos, 0, margin, margin));
                    r.FillRectangle(b.GetPixel(0, b.Height-1), new Rectangle(0, yPos, margin, margin));
                    r.FillRectangle(b.GetPixel(b.Width-1, b.Height-1), new Rectangle(xPos, yPos, margin, margin));

                    //draw original image
                    r.DrawImage(b, margin, margin);
                    break;

                case IsoMetricTileGenerationSettings.IsoMetricBorderProtection.MirrorAboutEdge:
                    break;
                case IsoMetricTileGenerationSettings.IsoMetricBorderProtection.ToroidalWrap:
                    break;
            }
            return r.RenderTargetAsGDIBitmap();
        }
    }
}
