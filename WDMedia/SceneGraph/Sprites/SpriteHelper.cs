/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox;
using WD_toolbox.AplicationFramework;

namespace WDMedia.SceneGraph.Sprites
{
    public static class SpriteHelper
    {
        static int maxImageDimension = 2048;

        public class CombinedSpriteSheet
        {
            public Bitmap Image;
            public Dictionary<int, int>[] Remap;

            public CombinedSpriteSheet(Bitmap image, Dictionary<int, int>[] remap)
            {
                this.Image = image;
                this.Remap = remap;
            }

            static CombinedSpriteSheet fromFiles(string imagePath, string settingsPath)
            {
                Bitmap image = ((Bitmap)Bitmap.FromFile(imagePath)).GetUnFuckedVersion();
                Dictionary<int, int>[] remap;
                bool settingsOk = loadSettings(settingsPath, out remap);

                if((image != null) && (settingsOk))
                {
                    CombinedSpriteSheet cs = new CombinedSpriteSheet(image, remap);
                    return cs;
                }

                //error
                return null;
            }

            public bool saveSettings(string settingsPath)
            {
                try
                {
                    List<int> nums = new List<int>();
                    nums.Add(Remap.Length);
                    foreach(Dictionary<int, int> map in Remap)
                    {
                        nums.Add(map.Count);
                        foreach(var mapping in map)
                        {
                            nums.Add(mapping.Key);
                            nums.Add(mapping.Value);
                        }
                    }

                    string data = string.Join(",", nums);
                    File.WriteAllText(settingsPath, data);
                    return true;
	            }
	            catch (Exception)
	            {
                    return false;
	            }
            }

            private static bool loadSettings(string settingsPath, out Dictionary<int, int>[] remap)
            {
                try
                {
                    string text = File.ReadAllText(settingsPath);
                    int[] nums = text.ParseAllIntegers();

                    int pos=0;
                    remap = new Dictionary<int, int>[nums[pos]]; 
                    pos++;

                    for (int m = 0; m < remap.Length; m++)
                    {
                        //create map
                        remap[m] = new Dictionary<int, int>();
                        int count = nums[pos];
                        pos++;

                        //load map
                        for (int c = 0;  c < count; c++)
                        {
                            //load map
                            int k = nums[pos];
                            pos++;
                            int v = nums[pos];
                            pos++;
                            remap[m].Add(k, v);
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    remap = null;
                    return false;
                }
            }
        }

        public static CombinedSpriteSheet CombineSpritesSheets(  IList<string> inputFiles, 
                                                    Size spriteSize, 
                                                    string outputFile = null,
                                                    int[][] spritesToKeep = null)
        {
            //accumulate images that will be used
            Dictionary<int, int>[] remap;
            List<Bitmap> subImages = getAllSprites(inputFiles, spriteSize, spritesToKeep, out remap);

            Bitmap b = MakeImageAtlas(spriteSize, subImages);

            //save image
            if(outputFile != null)
            {
                //make sure the precompile dir is present
                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                }

                b.Save(outputFile);
            }

            //return
            CombinedSpriteSheet res = new CombinedSpriteSheet(b, remap);
            return res;
        }

        public static Bitmap MakeImageAtlas(Size spriteSize, IList<Bitmap> subImages)
        {
            //find the optimal new image size
            int width, height;
            getBestSheetSize(subImages, out width, out height);

            Bitmap b = new Bitmap(width * spriteSize.Width, height * spriteSize.Height);

            //create single image
            SetAllSpritesInImage(b, spriteSize, subImages);
            return b;
        }

        private static List<Bitmap> getAllSprites(IList<string> inputFiles, 
                                              Size spriteSize, 
                                              int[][] spritesToKeep, 
                                              out Dictionary<int, int>[] remap)
        {
            List<Bitmap> subImages = new List<Bitmap>();
            remap = new Dictionary<int, int>[inputFiles.Count];
            int finalPos = 0;
            int i = 0;
            foreach (string file in inputFiles)
            {
                IList<Bitmap> frames = GetAllSpritesInFile(file, spriteSize);

                remap[i] = new Dictionary<int, int>();
                List<Bitmap> toRemove = new List<Bitmap>();

                //visit every frame
                int k = 0;
                foreach (Bitmap frame in frames)
                {
                    if (spritesToKeep != null)
                    {
                        if (spritesToKeep[i].Contains(k))
                        {
                            remap[i].Add(k, finalPos);
                            finalPos++;
                        }
                        else
                        {
                            //don't keep sprite
                            toRemove.Add(frame);
                        }
                    }
                    else
                    {
                        remap[i].Add(k, finalPos);
                        finalPos++;
                    }
                    k++;
                }

                //remove flaged images
                foreach (Bitmap r in toRemove)
                {
                    frames.Remove(r);
                }

                //add images
                subImages.AddRange(frames);
                i++;
            }

            return subImages;
        }

        /// <summary>
        /// Finds the best size sprite sheet for a set of frames
        /// </summary>
        /// <param name="subImages"></param>
        /// <param name="widthInTiles"></param>
        /// <param name="heightInTiles"></param>
        /// <param name="acceptableWaste">How many frames, in the resulting sheet, we are willing to leave unused 
        /// for a squarer sprite sheet.
        /// </param>
        private static void getBestSheetSize(IList<Bitmap> subImages, out int widthInTiles, out int heightInTiles, int acceptableWaste = 5)
        {
            widthInTiles = subImages.Count;
            heightInTiles = 1;

            //some cases are obvious strait lines
            if ((new int[] { 1, 2, 3, 5 }).Contains(subImages.Count))
            {
                return;
            }

            //iterate for the best solution
            int bestWaste = subImages.Count;  //unused tile positions
            int bestSquare = subImages.Count;  //abs(w-h)
            int bestWidth = 1;
            for (int width = 2; width < (subImages.Count / 2); width++)
            {
                int height = (int)Math.Ceiling(subImages.Count / (double)width);
                int waste = (width * height) - subImages.Count;
                int square = Math.Abs(width-height);

                if (waste < (bestWaste + acceptableWaste))
                {
                    if (square < bestSquare)
                    {
                        bestWidth = width;
                        bestSquare = square;
                        bestWaste = waste;
                    }
                }
            }

            widthInTiles = bestWidth;
            heightInTiles = (int)Math.Ceiling(subImages.Count / (double)bestWidth);
            return;
        }

        public static IList<Bitmap> GetAllSpritesInFile(string file, Size spriteSize)
        {
            Bitmap img = Bitmap.FromFile(file) as Bitmap;
            if (img != null)
            {
                img = img.GetUnFuckedVersion();
                return GetAllSpritesInImage(img, spriteSize);
            }
            WDAppLog.logFileOpenError(ErrorLevel.Error, file);
            return new List<Bitmap>();
        }

        public static IList<Bitmap> GetAllSpritesInImage(Bitmap image, Size spriteSize)
        {
            List<Bitmap> subImages = new List<Bitmap>();

            for (int y = 0; y < image.Height; y += spriteSize.Height)
            {
                for (int x = 0; x < image.Width; x += spriteSize.Width)
                {
                    Bitmap subImage = image.GetSubImageClipped(x, y, spriteSize.Width, spriteSize.Height);
                    subImages.Add(subImage);
                }
            }

            return subImages;
        }

        public static void SetAllSpritesInImage(Bitmap image, Size spriteSize, IList<Bitmap> images)
        {
            using(Graphics g = Graphics.FromImage(image))
            {
                int pos = 0;
                for (int y = 0; y < image.Height; y += spriteSize.Height)
                {
                    for (int x = 0; x < image.Width; x += spriteSize.Width)
                    {
                        if (pos < images.Count)
                        {
                            Rectangle bounds = new Rectangle(x, y, spriteSize.Width, spriteSize.Height);
                            g.DrawImage(images[pos], bounds);
                        }
                        pos++;
                    }
                }
            }
        }

        public static string GetAnimationName(StandardSpriteAnimations animation, StandardSpriteOrientations orientation)
        {
            return (Enum.GetName(typeof(StandardSpriteAnimations), animation) +
                   "_" +
                   Enum.GetName(typeof(StandardSpriteOrientations), orientation)).ToLower();
        }

        public static List<Rectangle> CalcFrameBounds(Bitmap img, int width, int height)
        {
            List<Rectangle> FrameBounds = new List<Rectangle>();

            for (int y = 0; y < img.Height; y += height)
            {
                for (int x = 0; x < img.Width; x += width)
                {
                    //img.GetSubImageClipped(x, y, SpriteWidth, SpriteHeight);
                    FrameBounds.Add(new Rectangle(x, y, width, height));
                }
            }

            return FrameBounds;
        }
    }
}
