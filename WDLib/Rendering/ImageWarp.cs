/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Maths.Geometry;

//transform is in Image warp transform.pdf on google drive
//general warps here http://www.cs.princeton.edu/courses/archive/fall00/cs426/lectures/warp/warp.pdf
//this is ok http://www.coe.utah.edu/~cs6640/geometric_trans.pdf
//this is what I intend to work from http://www.ece.ucsb.edu/~manj/ece178-Fall2008/ImageWarping.pdf

/*
namespace WD_toolbox.Rendering
{
    public class ImageWarp
    {
        //1 Computation of the bounding box of the warped image (forward mapping).
        //2 Backward mapping of lattice points that sample the bounding box of the 
        //  warped image (avoid “holes”)
        //3 Validation of the backward mapped points (must belong to the domain
        //  of the source image)
        //4 Intensity transfer via resampling

        public class Quad
        {
            public Point2D TopL, TopR, LowL, LowR;
            public Quad(Point2D topL, Point2D topR, Point2D lowL, Point2D lowR)
            {
                TopL = topL;
                TopR = topR;
                LowL = lowL;
                LowR = lowR;
            }

            public Rectangle GetBoundingBox()
            {
                return Rectangle2D.BoundingBox(new Point2D[] { TopL, TopR, LowL, LowR}).BoundingRound();
            }
        }

        public static Bitmap Warp(Bitmap bmp, Quad dest, out Point renderPos)
        {
            //1 Computation of the bounding box of the warped image (forward mapping).
            Rectangle destBox = dest.GetBoundingBox();
            renderPos = destBox.Location;
            Bitmap warpBmp = new Bitmap(destBox.Width, destBox.Height, bmp.PixelFormat);
            
            //2 Backward mapping of lattice points that sample the bounding box of the 
            //  warped image (avoid “holes”)

            // GDI+ still lies to us - the return format is BGR, NOT RGB. 
            BitmapData srcBmp = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), 
            ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb); 
            int stride = srcBmp.Stride; 
            System.IntPtr Scan0 = srcBmp.Scan0; 
            unsafe
            {
                byte* p = (byte *)(void *)Scan0;
                int nOffset = stride - bmp.Width*3;
                byte red, green, blue;

                for (int y = 0; y < bmp.Height; ++y)
                {
                    for (int x = 0; x < bmp.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            } //end unsafe
            bmp.UnlockBits(srcBmp);

            return warpBmp;

        }



    }
}
*/