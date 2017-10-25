using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TClone {
    class Animation : DrawableGameComponent {
        float lifeSpan = 3f;
        float timer = 0f;

        public Animation(Game game) : base(game) {
        }

        public void Play() {
            if(Game.Components.Contains(this)) {
                timer = 0;
                Debug.WriteLine("Animation Restarted");
                return;
            }

            Debug.WriteLine("Animation Played");
            Game.Components.Add(this);
        }

        public override void Update(GameTime gameTime) {
            if (timer >= lifeSpan) {
                Game.Components.Remove(this);
                timer = 0f;
                return;
            }

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime) {
            base.Draw(gameTime);

            TClone.instance.spriteBatch.Begin();
            TClone.instance.spriteBatch.DrawString(TClone.font, "Cleared!", (TClone.playArea.Center-(TClone.font.MeasureString("Cleared!")/2).ToPoint()).ToVector2(), new Color(Color.Black, .8f));
            TClone.instance.spriteBatch.End();
        }
    }
}
