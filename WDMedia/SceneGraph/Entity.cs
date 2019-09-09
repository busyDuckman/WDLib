/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Maths.Geometry;
using WDMedia.SceneGraph.Sprites;

namespace WDMedia.SceneGraph
{
    /// <summary>
    /// Anything int the world
    /// </summary>
    public class Entity : WDMedia.SceneGraph.IUpdateable
    {
        public Rectangle2D Bounds;
        public ISprite Sprite;
        public string AnimationName;
        public double AnimationPos;
        public long AnimationStartTimeMS;
        public int CurrentAnimationFrame;
        public int CurrentSpriteFrame;
        public SpriteAnimation CurrentAnimation { get { return Sprite.Animations[AnimationName]; } }

        public Entity(ISprite sprite)
        {
            this.Sprite = sprite;
            this.AnimationName = Sprite.Animations.Keys.FirstOrDefault();
            this.AnimationPos = 0;
            this.AnimationStartTimeMS = 0;
            this.Bounds = new Rectangle2D(0, 0, sprite.SpriteWidth, sprite.SpriteHeight);
            this.Update(0);
        }

        public void SetAnimation(string animationName, long gameMS)
        {
            AnimationStartTimeMS = gameMS;
            this.AnimationName = animationName;
        }

        public void SetAnimation(StandardSpriteAnimations animation,
                                 StandardSpriteOrientations orientation,
                                 long gameMS)
        {
            AnimationStartTimeMS = gameMS;
            this.AnimationName = SpriteHelper.GetAnimationName(animation, orientation);
        }

        public void Update(long gameMS)
        {
            SpriteAnimation animation = CurrentAnimation;
            double AnimationLen = animation.AnimationDurationMS;
            long MS = gameMS - AnimationStartTimeMS;
            AnimationPos = (MS / AnimationLen) % 1.0;
            CurrentAnimationFrame = (int)(animation.Frames.Length * AnimationPos);
            CurrentSpriteFrame = animation.Frames[CurrentAnimationFrame];
        }
    }
}
