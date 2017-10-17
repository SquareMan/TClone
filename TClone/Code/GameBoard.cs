using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TClone {
    class GameBoard {
        KeyboardState keyState;
        Block[,] blocks = new Block[TClone.WIDTH, TClone.HEIGHT];
        List<Block> activeBlocks = new List<Block>();

        float timePerDrop = .4f;
        float timer = 0;

        public GameBoard() {

        }

        public void PlacePrefab(Block.BlockPrefab prefab, Point origin) {
            //Clear list of active blocks
            activeBlocks.Clear();

            foreach (Block b in prefab.blocks) {
                int offX = b.GetPosition().X + origin.X;
                int offY = b.GetPosition().Y + origin.Y;
                blocks[offX, offY] = new Block(new Point(offX, offY), b.color);

                //Add new block to active block list
                activeBlocks.Add(blocks[offX, offY]);
            }
        }

        public void Update(float deltaTime) {
            timer += deltaTime;
            if(timer < timePerDrop) {
                return;
            }
            timer = timer - timePerDrop;

            keyState = Keyboard.GetState();
            if(keyState.IsKeyDown(Keys.Left)) {
                //Move Left
                foreach(Block b in activeBlocks) {
                    b.SetPosition(new Point(b.GetPosition().X - 1, b.GetPosition().Y));
                }
            } else if(keyState.IsKeyDown(Keys.Right)) {
                //Move Right
                foreach (Block b in activeBlocks) {
                    b.SetPosition(b.GetPosition() + new Point(1, 0));
                }
            }

            for (int x = TClone.WIDTH-1; x >= 0; x--) {
                for (int y = TClone.HEIGHT-2; y >= 0; y--) {
                    if (blocks[x, y] == null) {
                        continue;
                    }
                    if (blocks[x, y + 1] == null) {
                        //Drop block
                        blocks[x, y].SetPosition(new Point(x, y + 1));
                        blocks[x, y + 1] = blocks[x, y];
                        blocks[x, y] = null;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb) {
            foreach (Block b in blocks) {
                b?.Draw(sb);
            }
        }
    }
}
