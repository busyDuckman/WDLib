#define COLOR_ORDER_ARGB
//#define COLOR_ORDER_BGRA

using System;

#if !NET_MICRO_FRAMEWORK
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Maths.Geometry;
#else
using Microsoft.SPOT;
using System.Runtime.InteropServices;
//using Microsoft.SPOT.Presentation.Media;
#endif

namespace WD_toolbox.Rendering
{
#if NET_MICRO_FRAMEWORK
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
        }
    }

    public struct Rectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Right { get { return X + Width; } }
        public int Bottom { get { return Y + Height; } }

        public Rectangle(int x, int y, int width, int height) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        
        }
    }

#if COLOR_ORDER_ARGB
    //const int RED_MASK =0xff0000, GREEN_MASK=0xff00, BLUE_MASK=0xff;
    //const int RED_LSR =16, GREEN_LSR=8, BLUE_LSR=0; //LSR = logical shift right (old asm code)
#endif
#if  COLOR_ORDER_BGRA
    //const int RED_MASK =0xff00, GREEN_MASK=0xff0000, BLUE_MASK=0xff000000;
    //const int RED_LSR =8, GREEN_LSR=16, BLUE_LSR=24; //LSR = logical shift right (old asm code)
#endif
    //[StructLayout(LayoutKind.Explicit)]
    public struct Color
    {
        public static readonly Color Alice_Blue = new Color(240, 248, 255);
        public static readonly Color Antique_White = new Color(250, 235, 215);
        public static readonly Color Aqua = new Color(0, 255, 255);
        public static readonly Color Aquamarine = new Color(127, 255, 212);
        public static readonly Color Azure = new Color(240, 255, 255);
        public static readonly Color Beige = new Color(245, 245, 220);
        public static readonly Color Bisque = new Color(255, 228, 196);
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color Blanched_Almond = new Color(255, 235, 205);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color Blue_Violet = new Color(138, 43, 226);
        public static readonly Color Brown = new Color(165, 42, 42);
        public static readonly Color Burlywood = new Color(222, 184, 135);
        public static readonly Color Cadet_Blue = new Color(95, 158, 160);
        public static readonly Color Chartreuse = new Color(127, 255, 0);
        public static readonly Color Chocolate = new Color(210, 105, 30);
        public static readonly Color Coral = new Color(255, 127, 80);
        public static readonly Color Cornflower_Blue = new Color(100, 149, 237);
        public static readonly Color Cornsilk = new Color(255, 248, 220);
        public static readonly Color Cyan = new Color(0, 255, 255);
        public static readonly Color Dark_Blue = new Color(0, 0, 139);
        public static readonly Color Dark_Cyan = new Color(0, 139, 139);
        public static readonly Color Dark_Goldenrod = new Color(184, 134, 11);
        public static readonly Color Dark_Gray = new Color(169, 169, 169);
        public static readonly Color Dark_Green = new Color(0, 100, 0);
        public static readonly Color Dark_Khaki = new Color(189, 183, 107);
        public static readonly Color Dark_Magenta = new Color(139, 0, 139);
        public static readonly Color Dark_Olive_Green = new Color(85, 107, 47);
        public static readonly Color Dark_Orange = new Color(255, 140, 0);
        public static readonly Color Dark_Orchid = new Color(153, 50, 204);
        public static readonly Color Dark_Red = new Color(139, 0, 0);
        public static readonly Color Dark_Salmon = new Color(233, 150, 122);
        public static readonly Color Dark_Sea_Green = new Color(143, 188, 143);
        public static readonly Color Dark_Slate_Blue = new Color(72, 61, 139);
        public static readonly Color Dark_Slate_Gray = new Color(47, 79, 79);
        public static readonly Color Dark_Turquoise = new Color(0, 206, 209);
        public static readonly Color Dark_Violet = new Color(148, 0, 211);
        public static readonly Color Deep_Pink = new Color(255, 20, 147);
        public static readonly Color Deep_Sky_Blue = new Color(0, 191, 255);
        public static readonly Color Dim_Gray = new Color(105, 105, 105);
        public static readonly Color Dodger_Blue = new Color(30, 144, 255);
        public static readonly Color Firebrick = new Color(178, 34, 34);
        public static readonly Color Floral_White = new Color(255, 250, 240);
        public static readonly Color Forest_Green = new Color(34, 139, 34);
        public static readonly Color Fuschia = new Color(255, 0, 255);
        public static readonly Color Gainsboro = new Color(220, 220, 220);
        public static readonly Color Ghost_White = new Color(255, 250, 250);
        public static readonly Color Gold = new Color(255, 215, 0);
        public static readonly Color Goldenrod = new Color(218, 165, 32);
        public static readonly Color Gray = new Color(128, 128, 128);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Green_Yellow = new Color(173, 255, 47);
        public static readonly Color Honeydew = new Color(240, 255, 240);
        public static readonly Color Hot_Pink = new Color(255, 105, 180);
        public static readonly Color Indian_Red = new Color(205, 92, 92);
        public static readonly Color Ivory = new Color(255, 255, 240);
        public static readonly Color Khaki = new Color(240, 230, 140);
        public static readonly Color Lavender = new Color(230, 230, 250);
        public static readonly Color Lavender_Blush = new Color(255, 240, 245);
        public static readonly Color Lawn_Green = new Color(124, 252, 0);
        public static readonly Color Lemon_Chiffon = new Color(255, 250, 205);
        public static readonly Color Light_Blue = new Color(173, 216, 230);
        public static readonly Color Light_Coral = new Color(240, 128, 128);
        public static readonly Color Light_Cyan = new Color(224, 255, 255);
        public static readonly Color Light_Goldenrod = new Color(238, 221, 130);
        public static readonly Color Light_Goldenrod_Yellow = new Color(250, 250, 210);
        public static readonly Color Light_Gray = new Color(211, 211, 211);
        public static readonly Color Light_Green = new Color(144, 238, 144);
        public static readonly Color Light_Pink = new Color(255, 182, 193);
        public static readonly Color Light_Salmon = new Color(255, 160, 122);
        public static readonly Color Light_Sea_Green = new Color(32, 178, 170);
        public static readonly Color Light_Sky_Blue = new Color(135, 206, 250);
        public static readonly Color Light_Slate_Blue = new Color(132, 112, 255);
        public static readonly Color Light_Slate_Gray = new Color(119, 136, 153);
        public static readonly Color Light_Steel_Blue = new Color(176, 196, 222);
        public static readonly Color Light_Yellow = new Color(255, 255, 224);
        public static readonly Color Lime = new Color(0, 255, 0);
        public static readonly Color Lime_Green = new Color(50, 205, 50);
        public static readonly Color Linen = new Color(250, 240, 230);
        public static readonly Color Magenta = new Color(255, 0, 255);
        public static readonly Color Maroon = new Color(128, 0, 0);
        public static readonly Color Medium_Aquamarine = new Color(102, 205, 170);
        public static readonly Color Medium_Blue = new Color(0, 0, 205);
        public static readonly Color Medium_Orchid = new Color(186, 85, 211);
        public static readonly Color Medium_Purple = new Color(147, 112, 219);
        public static readonly Color Medium_Sea_Green = new Color(60, 179, 113);
        public static readonly Color Medium_Slate_Blue = new Color(123, 104, 238);
        public static readonly Color Medium_Spring_Green = new Color(0, 250, 154);
        public static readonly Color Medium_Turquoise = new Color(72, 209, 204);
        public static readonly Color Medium_Violet_Red = new Color(199, 21, 133);
        public static readonly Color Midnight_Blue = new Color(25, 25, 112);
        public static readonly Color Mint_Cream = new Color(245, 255, 250);
        public static readonly Color Misty_Rose = new Color(255, 228, 225);
        public static readonly Color Moccasin = new Color(255, 228, 181);
        public static readonly Color Navajo_White = new Color(255, 222, 173);
        public static readonly Color Navy = new Color(0, 0, 128);
        public static readonly Color Old_Lace = new Color(253, 245, 230);
        public static readonly Color Olive = new Color(128, 128, 0);
        public static readonly Color Olive_Drab = new Color(107, 142, 35);
        public static readonly Color Orange = new Color(255, 165, 0);
        public static readonly Color Orange_Red = new Color(255, 69, 0);
        public static readonly Color Orchid = new Color(218, 112, 214);
        public static readonly Color Pale_Goldenrod = new Color(238, 232, 170);
        public static readonly Color Pale_Green = new Color(152, 251, 152);
        public static readonly Color Pale_Turquoise = new Color(175, 238, 238);
        public static readonly Color Pale_Violet_Red = new Color(219, 112, 147);
        public static readonly Color Papaya_Whip = new Color(255, 239, 213);
        public static readonly Color Peach_Puff = new Color(255, 218, 185);
        public static readonly Color Peru = new Color(205, 133, 63);
        public static readonly Color Pink = new Color(255, 192, 203);
        public static readonly Color Plum = new Color(221, 160, 221);
        public static readonly Color Powder_Blue = new Color(176, 224, 230);
        public static readonly Color Purple = new Color(128, 0, 128);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Rosy_Brown = new Color(188, 143, 143);
        public static readonly Color Royal_Blue = new Color(65, 105, 225);
        public static readonly Color Saddle_Brown = new Color(139, 69, 19);
        public static readonly Color Salmon = new Color(250, 128, 114);
        public static readonly Color Sandy_Brown = new Color(244, 164, 96);
        public static readonly Color Sea_Green = new Color(46, 139, 87);
        public static readonly Color Seashell = new Color(255, 245, 238);
        public static readonly Color Sienna = new Color(160, 82, 45);
        public static readonly Color Silver = new Color(192, 192, 192);
        public static readonly Color Sky_Blue = new Color(135, 206, 235);
        public static readonly Color Slate_Blue = new Color(106, 90, 205);
        public static readonly Color Slate_Gray = new Color(112, 128, 144);
        public static readonly Color Snow = new Color(255, 250, 250);
        public static readonly Color Spring_Green = new Color(0, 255, 127);
        public static readonly Color Steel_Blue = new Color(70, 130, 180);
        public static readonly Color Tan = new Color(210, 180, 140);
        public static readonly Color Teal = new Color(0, 128, 128);
        public static readonly Color Thistle = new Color(216, 191, 216);
        public static readonly Color Tomato = new Color(255, 99, 71);
        public static readonly Color Turquoise = new Color(64, 224, 208);
        public static readonly Color Violet = new Color(238, 130, 238);
        public static readonly Color Violet_Red = new Color(208, 32, 144);
        public static readonly Color Wheat = new Color(245, 222, 179);
        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color White_Smoke = new Color(245, 245, 245);
        public static readonly Color Yellow = new Color(255, 255, 0);
        public static readonly Color Yellow_Green = new Color(154, 205, 50);

        //#if COLOR_ORDER_ARGB
        //[FieldOffset(0)]
        //int value; //32bit int
        public byte R,G,B;// { get {return value & RED_MASK}; set{}; }
        public Color(byte red, byte green, byte blue)
        {
            this.R = red;
            this.G = green;
            this.B = blue;
        }

    }

    public class Image
    {
        public byte[] data;
    }
#endif
    public enum RendererLineCapStyle
    {
        Flat, Round
    }
    /// <summary>
    /// An Interface to common rendering API's
    /// Importantly this reduces dependencies on things like GDI+
    /// It is expected the IRenderer will be in high quality mode when created.
    /// </summary>
    public interface IRenderer : IDisposable
    {
        void Close();
        void Flush();
        Bitmap RenderTargetAsGDIBitmap();

        void SetHighQuality(bool enale);
        void SetLineEndCap(RendererLineCapStyle capStyle);
        bool SupportsMultiThreading { get; }

        void Clear(Color color);

#if !NET_MICRO_FRAMEWORK
        void SetTransform(TransMatrix2D t);
        void ResetTransform();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="destPoints">3 points for a parallelogram, [ul,ur,ll] </param>
        void DrawImage(Image image, Point[] destPoints);
        void DrawImage(Image image, int x, int y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="destPoints">3 points for a parallelogram, [ul,ur,ll] </param>
        /// <param name="srcRect"></param>
        void DrawImage(Image image, Point[] destPoints, Rectangle srcRect);
        void DrawImage(Image image, Rectangle destRect, Rectangle srcRect);
        void DrawImage(Image image, int x, int y, int width, int height);
        void DrawImage(Image image, int x, int y, Rectangle srcRect);

        void DrawString(Color col, string s, Font font, int x, int y);

        void PutPixel(Color col, int x, int y);

        void DrawArc(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle);
        void DrawBeziers(Color col, int lineSize, Point[] points);
        void DrawLine(Color col, int lineSize, int x1, int y1, int x2, int y2);
        void DrawPolyLine(Color col, int lineSize, Point[] points);
        void DrawCurve(Color col, int lineSize, Point[] points);

        void DrawClosedCurve(Color col, int lineSize, Point[] points);
        void DrawEllipse(Color col, int lineSize, int x, int y, int width, int height);
        void DrawCircle(Color col, int lineSize, int cx, int cy, int radius);
        void DrawPie(Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle);
        void DrawPolygon(Color col, int lineSize, Point[] points);
        void DrawRectangle(Color col, int lineSize, int x, int y, int width, int height);

        void FillClosedCurve(Color fillCol, Point[] points);
        void FillEllipse(Color fillCol, int x, int y, int width, int height);
        void FillCircle(Color fillCol, int cx, int cy, int radius);
        void FillPie(Color fillCol, int x, int y, int width, int height, double startAngle, double sweepAngle);
        void FillPolygon(Color fillCol, Point[] points);
        void FillRectangle(Color fillCol, int x, int y, int width, int height);
      
        Color GetNearestColor(Color color);
    }

    public static class IRendererFactory
    {
        /// <summary>
        /// Gets the renderer that is best supported by the system.
        /// </summary>
        /// <param name="b">An Image to draw onto.</param>
        /// <returns>A new IRenderer instance.</returns>
        public static IRenderer GetPreferredRenderer(Bitmap b)
        {
            
#if !NET_MICRO_FRAMEWORK
            return new GDIPlusRenderer(b);
#else
            return null;
#endif
        }

        /// <summary>
        /// Gets the renderer that is best supported by the system.
        /// The renderer will be for an ARGB based colour scheme.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns>A new IRenderer instance.</returns>
        public static IRenderer GetPreferredRenderer(int width, int height)
        {
#if !NET_MICRO_FRAMEWORK
            return new GDIPlusRenderer(width, height);
#else
            return null;
#endif
        }

    }

#if !NET_MICRO_FRAMEWORK
    /// <summary>
    /// Function overloads for IRenderer
    /// </summary>
    public static class IRendererExtension
    {
        public static void DrawImage(this IRenderer renderer, Image image, Point point)
        {
            renderer.DrawImage(image, point.X, point.Y);
        }

        public static void DrawImage(this IRenderer renderer, Image image, Rectangle rect)
        {
            renderer.DrawImage(image, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void DrawLine(this IRenderer renderer, Color col, int lineSize, Point pt1, Point pt2)
        {
            renderer.DrawLine(col, lineSize, pt1.X, pt1.Y, pt2.X, pt2.Y);
        }
        public static void DrawArc(this IRenderer renderer, Color col, int lineSize, Rectangle rect, double startAngle, double sweepAngle)
        {
            renderer.DrawArc(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height,
                             startAngle, sweepAngle);
        }
        public static void DrawBezier(this IRenderer renderer, Color col, int lineSize, Point pt1, Point pt2, Point pt3, Point pt4)
        {
            renderer.DrawBeziers(col, lineSize, new Point[] { pt1, pt2, pt3, pt4 });
        }

        public static void DrawString(this IRenderer renderer, Color col, string s, Font font, Point point)
        {
            renderer.DrawString(col, s, font, point.X, point.Y);
        }

        public static void DrawEllipse(this IRenderer renderer, Color col, int lineSize, Rectangle rect)
        {
            renderer.DrawEllipse(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public static void DrawPie(this IRenderer renderer, Color col, int lineSize, Rectangle rect, double startAngle, double sweepAngle)
        {
            renderer.DrawPie(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }
        public static void DrawRectangle(this IRenderer renderer, Color col, int lineSize, Rectangle rect)
        {
            renderer.DrawRectangle(col, lineSize, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void FillEllipse(this IRenderer renderer, Color fillCol, Rectangle rect)
        {
            renderer.FillEllipse(fillCol, rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static void FillPie(this IRenderer renderer, Color fillCol, Rectangle rect, double startAngle, double sweepAngle)
        {
            renderer.FillPie(fillCol, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
        }

        public static void FillRectangle(this IRenderer renderer, Color fillCol, Rectangle rect)
        {
            renderer.FillRectangle(fillCol, rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
#endif    
}
