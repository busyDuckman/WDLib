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
using WD_toolbox.AplicationFramework;
using WDMedia.Rendering;

namespace WDMedia.SceneGraph.Sprites
{
    public class LayeredSpriteLayer : IPRImage, IDisposable
    {
        public Bitmap Image { get; protected set; }
        public object NativeObject { get; set; }
        public string Name { get; protected set; }

        public LayeredSpriteLayer(string name, Bitmap image)
	    {
            this.Image = image;
            this.Name = name;
            NativeObject = null;
	    }

        public void Dispose()
        {
            if (Image != null)
            {
                Image.Dispose();
                Image = null;
            }

            IDisposable nat = NativeObject as IDisposable;
            if (nat != null)
            {
                nat.Dispose();
                NativeObject = null;
            }
        }
    }

    public class LayeredSprite : ISprite, IReloadFile
    {
        //public Bitmap Image { get; protected set; }
        public List<LayeredSpriteLayer> Layers  { get; protected set; }

        public int SpriteWidth { get; protected set; }
        public int SpriteHeight { get; protected set; }
        public double DefaultFps { get { return 1000.0 / DefaultFrameIntervalMS; } }
        public int DefaultFrameIntervalMS { get; protected set; }
        public string FileName { get; protected set; }
        public Bitmap Image { get { return Layers[0].Image; } }
        public object NativeObject { get; set; }
        public string Name { get; set; }
        public Size SpriteSize {get; protected set;}

        public Dictionary<string, SpriteAnimation> Animations { get; protected set; }
        public List<Rectangle> FrameBounds { get; protected set; }

        public LayeredSprite(int width, int height, double fps)
        {
            this.SpriteWidth = width;
            this.SpriteHeight = height;
            SpriteSize = new Size(this.SpriteWidth, this.SpriteHeight);
            this.DefaultFrameIntervalMS = (int)(1000.0 / fps);
            Layers = new List<LayeredSpriteLayer>();
            Animations = new Dictionary<string, SpriteAnimation>();
            FrameBounds = new List<Rectangle>();
        }

        public bool ReloadFile(string path)
        {
            FrameBounds = SpriteHelper.CalcFrameBounds(Layers[0].Image, SpriteWidth, SpriteHeight);
            return true;
        }

        public void Dispose()
        {
            foreach (LayeredSpriteLayer layer in Layers)
            {
                layer.Dispose();
            }
        }

        
    }
}
