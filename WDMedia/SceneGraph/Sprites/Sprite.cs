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
using WD_toolbox.Maths.Geometry;
using WDMedia.Rendering;
using WD_toolbox;

namespace WDMedia.SceneGraph.Sprites
{
   

    public class SpriteAnimation
    {
        public int[] Frames {get; protected set;}
        public double FrameDurationMS {get; protected set;}
        public double AnimationDurationMS { get; protected set; }
        public Point2D Movement {get; protected set;}

        public SpriteAnimation(int[] frames, double frameDurationMS ) : this (frames, frameDurationMS, Point2D.Origen) 
        {
        }

        public SpriteAnimation(int[] frames, double frameDurationMS, Point2D movement)
        {
            this.Frames = frames;
            this.FrameDurationMS = frameDurationMS;
            this.AnimationDurationMS = this.FrameDurationMS * Frames.Length;
            this.Movement = movement;
        }


    }


    public class Sprite : ISprite, IReloadFile
    {
        public int SpriteWidth {get; protected set;}
        public int SpriteHeight { get; protected set; }
        public Size SpriteSize { get; protected set; }
        public double DefaultFps { get { return 1000.0 / DefaultFrameIntervalMS; } }
        public int DefaultFrameIntervalMS { get; protected set; }
        public string FileName { get; protected set; }
        public List<Rectangle> FrameBounds  { get; protected set; }

        public Dictionary<string, SpriteAnimation> Animations { get; protected set; }

        public Bitmap Image { get; protected set; }
        public object NativeObject { get; set; }

        public Sprite(int width, int height, double fps, string fileName)
        {
            this.SpriteWidth = width;
            this.SpriteHeight = height;
            SpriteSize = new Size(this.SpriteWidth, this.SpriteHeight);
            this.FileName = fileName;
            //this.DefaultFps = (fps==0)?1:fps;
            this.DefaultFrameIntervalMS = (int)(1000.0 / fps);
            FrameBounds = new List<Rectangle>();
            Animations = new Dictionary<string, SpriteAnimation>();
            ReloadFile(FileName);
        }

        public void addAnimation(StandardSpriteAnimations animation,
                                 StandardSpriteOrientations orientation,
                                 params int[] frames)
        {
            addAnimation(SpriteHelper.GetAnimationName(animation, orientation), frames);
        }

        public void addAnimation(StandardSpriteAnimations animation,
                                 StandardSpriteOrientations orientation,
                                 Point2D movement,
                                 params int[] frames)
        {
            addAnimation(SpriteHelper.GetAnimationName(animation, orientation), movement, frames);
        }


        public void addAnimation(string name,
                         params int[] frames)
        {
            SpriteAnimation anim = new SpriteAnimation(frames, this.DefaultFrameIntervalMS);

            Animations.Add(name, anim);
        }


        public void addAnimation(string name,
                                 Point2D movement,
                                 params int[] frames)
        {
            SpriteAnimation anim = new SpriteAnimation(frames, this.DefaultFrameIntervalMS, movement);

            Animations.Add(name, anim);
        }

        public bool ReloadFile(string path)
        {
            this.FileName = path;
            Bitmap img = Bitmap.FromFile(path) as Bitmap;
            if(img != null)
            {
                img = img.GetUnFuckedVersion();
                Image = img;

                FrameBounds = SpriteHelper.CalcFrameBounds(img, SpriteWidth, SpriteHeight);
                return true;
            }
            WDAppLog.logFileOpenError(ErrorLevel.Error, path);
            return false;
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
}
