﻿using Microsoft.Xna.Framework;
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
        public int score = 0;
        public int highScore = 0;

        Block[,] blocks = new Block[TClone.WIDTH, TClone.HEIGHT];
        Block.BlockPrefab activePrefab;
        Block.BlockPrefab nextPrefab;
        Block.BlockPrefab heldPrefab;
        Random rand;

        List<Block> activeBlocks = new List<Block>();
        Point activeOrigin;
        Point spawnPoint = new Point(TClone.WIDTH / 2 - 1, 0);

        float timePerDrop = .4f;
        float timer = 0;

        public GameBoard() {
            rand = new Random();
            nextPrefab = Block.prefabs[rand.Next(Block.prefabs.Count)];

            PlacePrefab(nextPrefab);
        }

        public void ClearBoard() {
            activeBlocks.Clear();
            for (int x = 0; x < TClone.WIDTH; x++) {
                for (int y = 0; y < TClone.HEIGHT; y++) {
                    blocks[x, y] = null;
                }
            }
        }

        public void PlacePrefab(Block.BlockPrefab prefab) {
            //Clear list of active blocks
            foreach(Block b in activeBlocks) {
                b.active = false;
            }
            activeBlocks.Clear();

            //Check if area is empty
            foreach (Block b in prefab.blocks) {
                int offX = b.GetPosition().X + spawnPoint.X;
                int offY = b.GetPosition().Y + spawnPoint.Y;

                if(blocks[offX,offY] != null) {
                    //TODO: Gameover
                    if(score > highScore) {
                        highScore = score;
                    }
                    score = 0;

                    ClearBoard();
                    break;
                }
            }

            foreach (Block b in prefab.blocks) {
                int offX = b.GetPosition().X + spawnPoint.X;
                int offY = b.GetPosition().Y + spawnPoint.Y;
                Block spawnedBlock = new Block(new Point(offX, offY), b.color);
                blocks[offX, offY] = spawnedBlock;

                //Add new block to active block list
                activeBlocks.Add(spawnedBlock);
                spawnedBlock.active = true;
            }
            activeOrigin = prefab.origin + spawnPoint;
            activePrefab = prefab;
        }

        public Block GetBlock(int x, int y) {
            if (x < 0 || x >= TClone.WIDTH ||
               y < 0 || y >= TClone.HEIGHT) {
                return null;
            }

            return blocks[x, y];
        }

        void Hold() {
            Block.BlockPrefab toPlace = heldPrefab;

            heldPrefab = activePrefab;
            foreach (Block b in activeBlocks) {
                Point blockPos = b.GetPosition();
                blocks[blockPos.X, blockPos.Y] = null;
            }

            if (toPlace.blocks == null)
                PlacePrefab(nextPrefab);
            else
                PlacePrefab(toPlace);
        }

        public void Update(float deltaTime) {
            //This can happen every frame
            //Check for user input left or right
            //Determine if move is legal (make sure every block can move)
            //If move is legal, perform it

            if(KeystateHelper.IsKeyReleased(Keys.LeftShift) || KeystateHelper.IsKeyReleased(Keys.RightShift)) {
                Hold();
            }

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
                    activeOrigin.X -= 1;
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
                    activeOrigin.X += 1;
                }
            }
            validMove = true;

            if(KeystateHelper.IsKeyReleased(Keys.LeftControl) || KeystateHelper.IsKeyReleased(Keys.RightControl)) {
                //Rotate clockwise

                //Determine centerpoint
                //Determine relative coordinates for each block
                //new relative coordinates = (x,y) -> (y,-x)

                //Check if rotation is possible
                bool validRotation = true;
                Point[] newPositions = new Point[activeBlocks.Count];
                for (int i = 0; i < newPositions.Length; i++) {
                    Point pos = activeBlocks[i].GetPosition() - activeOrigin;
                    Point newPos = new Point(-pos.Y, pos.X) + activeOrigin;
                    newPositions[i] = newPos;

                    Block testBlock = GetBlock(newPos.X, newPos.Y);
                    if ((newPos.X < 0 || newPos.X >= TClone.WIDTH || newPos.Y < 0 || newPos.Y >= TClone.HEIGHT) || 
                        testBlock != null && !testBlock.active) {
                        validRotation = false;
                    }
                }

                if (validRotation) {
                    //Remove the blocks from their current positon
                    foreach (Block b in activeBlocks) {
                        Point pos = b.GetPosition();
                        blocks[pos.X, pos.Y] = null;
                    }

                    for (int i = 0; i < newPositions.Length; i++) {
                        Block b = activeBlocks[i];
                        blocks[newPositions[i].X, newPositions[i].Y] = b;
                        b.SetPosition(newPositions[i]);
                    }
                }
            }

            //Accelerate the timer to drop the block faster
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
                activeOrigin.Y += 1;
            } else {
                //Check for and clear complete lines
                int multiplier = 0;
                for (int y = 0; y < TClone.HEIGHT; y++) {
                    bool completeLine = true;
                    for (int x = 0; x < TClone.WIDTH; x++) {
                        if (blocks[x, y] == null)
                            completeLine = false;
                    }

                    if (completeLine) {

                        if (multiplier == 0)
                            multiplier = 1;
                        else
                            multiplier *= 2;

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
                score += multiplier * 100;

                PlacePrefab(nextPrefab);
                nextPrefab = Block.prefabs[rand.Next(Block.prefabs.Count)];
            }
        }

        public void Draw(SpriteBatch sb) {
            foreach (Block b in blocks) {
                b?.Draw(sb);
            }

            foreach (Block b in nextPrefab.blocks) {
                b.Draw(sb, new Point(TClone.WIDTH + 2, 2));
            }

            if (heldPrefab.blocks != null) {
                foreach (Block b in heldPrefab.blocks) {
                    b.Draw(sb, new Point(TClone.WIDTH + 2, 9));
                }
            }

            sb.DrawString(TClone.font, "Score: " + score, Vector2.One, Color.Black);
            sb.DrawString(TClone.font, "High Score: " + highScore, new Vector2(1,TClone.font.LineSpacing), Color.Black);
        }
    }
}
