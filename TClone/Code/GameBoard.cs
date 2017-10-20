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
        Block[,] blocks = new Block[TClone.WIDTH, TClone.HEIGHT];
        List<Block> activeBlocks = new List<Block>();

        float timePerDrop = .4f;
        float timer = 0;

        public GameBoard() {

        }

        public void ClearBoard() {
            activeBlocks.Clear();
            for (int x = 0; x < TClone.WIDTH; x++) {
                for (int y = 0; y < TClone.HEIGHT; y++) {
                    blocks[x, y] = null;
                }
            }
        }

        public void PlacePrefab(Block.BlockPrefab prefab, Point origin) {
            //Clear list of active blocks
            foreach(Block b in activeBlocks) {
                b.active = false;
            }
            activeBlocks.Clear();

            //Check if area is empty
            foreach (Block b in prefab.blocks) {
                int offX = b.GetPosition().X + origin.X;
                int offY = b.GetPosition().Y + origin.Y;

                if(blocks[offX,offY] != null) {
                    //TODO: Gameover
                    ClearBoard();
                    break;
                }
            }

            foreach (Block b in prefab.blocks) {
                int offX = b.GetPosition().X + origin.X;
                int offY = b.GetPosition().Y + origin.Y;
                Block spawnedBlock = new Block(new Point(offX, offY), b.color);
                blocks[offX, offY] = spawnedBlock;

                //Add new block to active block list
                activeBlocks.Add(spawnedBlock);
                spawnedBlock.active = true;
            }
        }

        public Block GetBlock(int x, int y) {
            if (x < 0 || x >= TClone.WIDTH ||
               y < 0 || y >= TClone.HEIGHT) {
                return null;
            }

            return blocks[x, y];
        }

        public void Update(float deltaTime) {
            //This can happen every frame
            //Check for user input left or right
            //Determine if move is legal (make sure every block can move)
            //If move is legal, perform it

            bool validMove = true;
            if (KeystateHelper.IsKeyReleased(Keys.Left)) {
                //Move Left
                foreach (Block b in activeBlocks) {
                    Point blockPos = b.GetPosition();
                    Block testBlock = GetBlock(blockPos.X - 1, blockPos.Y);
                    if ((blockPos.X-1 < 0) || (testBlock != null && !testBlock.active)) {
                        validMove = false;
                    }
                }

                if(validMove) {
                    foreach (Block b in activeBlocks) {
                        Point blockPos = b.GetPosition();
                        blocks[blockPos.X, blockPos.Y] = null;
                    }

                    foreach (Block b in activeBlocks) {
                        Point blockPos = b.GetPosition();
                        b.SetPosition(new Point(blockPos.X - 1, blockPos.Y));
                        blocks[blockPos.X - 1, blockPos.Y] = b;
                    }
                }
            } else if (KeystateHelper.IsKeyReleased(Keys.Right)) {
                //Move Right
                foreach (Block b in activeBlocks) {
                    Point blockPos = b.GetPosition();
                    Block testBlock = GetBlock(blockPos.X + 1, blockPos.Y);
                    if ((blockPos.X + 1 >= TClone.WIDTH) || (testBlock != null && !testBlock.active)) {
                        validMove = false;
                    }
                }

                if (validMove) {
                    foreach (Block b in activeBlocks) {
                        Point blockPos = b.GetPosition();
                        blocks[blockPos.X, blockPos.Y] = null;
                    }

                    foreach (Block b in activeBlocks) {
                        Point blockPos = b.GetPosition();
                        b.SetPosition(new Point(blockPos.X + 1, blockPos.Y));
                        blocks[blockPos.X + 1, blockPos.Y] = b;
                    }
                }
            }
            validMove = true;
            if (KeystateHelper.state.IsKeyDown(Keys.Down))
                timer += deltaTime * 5;

            timer += deltaTime;
            if(timer < timePerDrop) {
                return;
            }
            timer = timer - timePerDrop;

            //This happens on a timer
            //Determine if the active blocks can fall again
            //If move is legal, drop the blocks
            //Else spawn a new block

            foreach (Block b in activeBlocks) {
                Point blockPos = b.GetPosition();
                Block testBlock = GetBlock(blockPos.X, blockPos.Y + 1);
                if ((blockPos.Y + 1 >= TClone.HEIGHT) || (testBlock != null && !testBlock.active)) {
                    validMove = false;
                }
            }

            if (validMove) {
                foreach (Block b in activeBlocks) {
                    Point blockPos = b.GetPosition();
                    blocks[blockPos.X, blockPos.Y] = null;
                }

                foreach (Block b in activeBlocks) {
                    Point blockPos = b.GetPosition();
                    b.SetPosition(new Point(blockPos.X, blockPos.Y + 1));
                    blocks[blockPos.X, blockPos.Y + 1] = b;
                }
            } else {
                //Check for and clear complete lines
                for (int y = 0; y < TClone.HEIGHT; y++) {
                    bool completeLine = true;
                    for (int x = 0; x < TClone.WIDTH; x++) {
                        if (blocks[x, y] == null)
                            completeLine = false;
                    }

                    if (completeLine) {
                        for (int x = 0; x < TClone.WIDTH; x++) {
                            blocks[x, y] = null;
                        }

                        //Drop above blocks by one
                        for (int j = y-1; j >= 0; j--) {
                            for (int i = 0; i < TClone.WIDTH; i++) {
                                Block b = blocks[i, j];
                                blocks[i, j] = null;
                                if (b == null)
                                    continue;

                                b.SetPosition(new Point(i, j+ 1));
                                blocks[i, j+1] = b;
                            }
                        }
                    }
                }

                PlacePrefab(Block.prefabBlock, new Point(5, 1));
            }
        }

        public void Draw(SpriteBatch sb) {

            foreach (Block b in blocks) {
                b?.Draw(sb);
            }

            for (int i = 0; i < TClone.WIDTH; i++) {
                for (int j = 0; j < TClone.HEIGHT; j++) {
                    if (blocks[i, j] == null) {
                        continue;
                    }

                    Rectangle dest = new Rectangle(i * TClone.TILESIZE, j * TClone.TILESIZE, 4, 4);
                    sb.Draw(TClone.pixel, dest, Color.Yellow);
                }
            }
        }
    }
}
