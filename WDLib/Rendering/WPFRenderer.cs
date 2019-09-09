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

using System.Windows.Media;
using WD_toolbox.Maths.Geometry;

namespace WD_toolbox.Rendering
{
    class WPFRenderer : IRenderer
    {

        /*public WPFRenderer(System.Windows.Media. b)
        {
            graphics = Graphics.FromImage(b);
            bitmap = b;
            SetHighQuality(true);
        }*/
        public AngleTypes AngleType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HighQuality
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public RendererLineCapStyle LineEndCapStyle
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool SupportsMultiThreading
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public TransMatrix2D Transform
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void ApplySettings(IRendererSettings setting)
        {
            throw new NotImplementedException();
        }

        public void Clear(System.Drawing.Color c)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void DrawArc(System.Drawing.Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawBeziers(System.Drawing.Color col, int lineSize, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawCircle(System.Drawing.Color col, int lineSize, int cx, int cy, int radius)
        {
            throw new NotImplementedException();
        }

        public void DrawClosedCurve(System.Drawing.Color col, int lineSize, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawCurve(System.Drawing.Color col, int lineSize, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawEllipse(System.Drawing.Color col, int lineSize, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Point[] destPoints, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, int x, int y, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(Image image, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void DrawLine(System.Drawing.Color col, int lineSize, int x1, int y1, int x2, int y2)
        {
            throw new NotImplementedException();
        }

        public void DrawPie(System.Drawing.Color col, int lineSize, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void DrawPolygon(System.Drawing.Color col, int lineSize, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawPolyLine(System.Drawing.Color col, int lineSize, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void DrawRectangle(System.Drawing.Color col, int lineSize, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void DrawString(System.Drawing.Color col, string s, Font font, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void FillCircle(System.Drawing.Color fillCol, int cx, int cy, int radius)
        {
            throw new NotImplementedException();
        }

        public void FillClosedCurve(System.Drawing.Color fillCol, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void FillEllipse(System.Drawing.Color fillCol, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void FillPie(System.Drawing.Color fillCol, int x, int y, int width, int height, double startAngle, double sweepAngle)
        {
            throw new NotImplementedException();
        }

        public void FillPolygon(System.Drawing.Color fillCol, Point[] points)
        {
            throw new NotImplementedException();
        }

        public void FillRectangle(System.Drawing.Color fillCol, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            throw new NotImplementedException();
        }

        public System.Drawing.Color GetNearestColor(System.Drawing.Color color)
        {
            throw new NotImplementedException();
        }

        public IRendererSettings GetSettings()
        {
            throw new NotImplementedException();
        }

        public void PutPixel(System.Drawing.Color col, int x, int y)
        {
            throw new NotImplementedException();
        }

        public Bitmap RenderTargetAsGDIBitmap()
        {
            throw new NotImplementedException();
        }

        public void ResetTransform()
        {
            throw new NotImplementedException();
        }

        public void SetHighQuality(bool enale)
        {
            throw new NotImplementedException();
        }


        public Size MeasureString(string s, Font font)
        {
            throw new NotImplementedException();
        }
    }
}
