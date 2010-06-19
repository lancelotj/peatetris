/*
   Author: Lance Jian

   Distributed under the terms of the GPL (GNU Public License)

   peatetris is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace peatetris {
    /// <summary>
    /// This is where all the blocks will be displayed and manipulated.
    /// The game area does not include the title screen, the score,
    /// or the Next Block section.
    /// </summary>
    public class GameArea : BlockArea {
        #region constants
        /// <summary>
        /// The default number of rows of a game area.
        /// </summary>
        public const int DEF_ROWS = 20;
        /// <summary>
        /// The default number of columns of a game area.
        /// </summary>
        public const int DEF_COLS = 10;
        #endregion
        #region constructor
        /// <summary>
        /// Create a two-dimensional array of pre-defined size.
        /// add an event handler to show and hide the squares
        /// in each block.
        /// </summary>
        public GameArea():base(DEF_ROWS, DEF_COLS) {
        }
        #endregion

        #region Public events
        /// <summary>
        /// Fires when the block reaches the bottom and cannot be moved.
        /// </summary>
        public event EventHandler StopMoveEvent;
        /// <summary>
        /// Fires when the lines are eliminated, the current block is cleared
        /// and ready to start a new block.
        /// </summary>
        public event EventHandler StartNewEvent;
        /// <summary>
        /// The add score delegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void AddScoreEventHandler(object sender, AddScoreEventArgs e);
        /// <summary>
        /// Fires when detected a line elimination and need to add score;
        /// </summary>
        public event AddScoreEventHandler AddScoreEvent;
        #endregion

        #region public methods
        /// <summary>
        /// Create a new random block, selecting from all possible blocks.
        /// </summary>
        /// <param name="area">the BlockArea to be displayed</param>
        /// <param name="x">the initial x</param>
        /// <param name="y">the initial y</param>
        public Block NewBlock(BlockArea area, int x, int y) {
            Block newBlk = null;
            BlockType type = (BlockType)rnd.Next(7);
            switch (type) {
                case BlockType.I:
                    newBlk = new IBlock(area, x, y);
                    break;
                case BlockType.J:
                    newBlk = new JBlock(area, x, y);
                    break;
                case BlockType.L:
                    newBlk = new LBlock(area, x, y);
                    break;
                case BlockType.S:
                    newBlk = new SBlock(area, x, y);
                    break;
                case BlockType.Z:
                    newBlk = new ZBlock(area, x, y);
                    break;
                case BlockType.O:
                    newBlk = new OBlock(area, x, y);
                    break;
                case BlockType.T:
                    newBlk = new TBlock(area, x, y);
                    break;
            }
            return newBlk;
        }
        /// <summary>
        /// Move block left.
        /// </summary>
        public void MoveLeft() {
            if (CurrentBlock != null)
                CurrentBlock.Left();
        }
        /// <summary>
        /// Move block right
        /// </summary>
        public void MoveRight() {
            if (CurrentBlock != null)
                CurrentBlock.Right();
        }
        /// <summary>
        /// Move block down. If it reaches the bottom, fire the StopMoveEvent. 
        /// It will then check if there are any lines that need to be eliminated.
        /// </summary>
        public void MoveDown() {
            if (CurrentBlock == null) {
                return;
            }
            if (CurrentBlock.CanMoveDown()) {
                CurrentBlock.Down();
            }
            else {
                if (StopMoveEvent != null) {
                    StopMoveEvent(this, null);
                }
                EliminateLines(CurrentBlock.BottomIndex());
                CurrentBlock = null;
                if (StartNewEvent != null) {
                    StartNewEvent(this, null);
                }
            }

        }

        /// <summary>
        /// Rotate block
        /// </summary>
        public void RotateBlock() {
            if (CurrentBlock != null)
                CurrentBlock.Rotate();
        }

        /// <summary>
        /// Eliminate full lines from the specific row.
        /// </summary>
        /// <param name="row">the row to start checking</param>
        public void EliminateLines(int row) {
            int upper = Math.Max(row - 3, 0);
            int elimCount = 0;
            for (int i = row; i >= upper; i--) {
                bool elim = true;
                for (int j = 0; j < Cols; j++) {
                    if (!GameArray[i, j].Visible) {
                        elim = false;
                        break;
                    }
                }
                if (!elim) {
                    continue;
                }
                elimCount++;
                for (int j = 0; j < Cols; j++)
                    GameArray[i, j].ClearEvents(); // unregister events, prevent memory leak.
                for (int k = i; k > 0; k--)
                    for (int j = 0; j < Cols; j++) {
                        GameArray[k, j] = GameArray[k - 1, j];
                        GameArray[k, j].Location = new Point(j, k);
                    }
                for (int j = 0; j < Cols; j++) {
                    GameArray[0, j] = new Square(j, 0);
                    GameArray[0, j].ShowEvent += new EventHandler(ShowSquare);
                    GameArray[0, j].HideEvent += new EventHandler(HideSquare);
                }
                i++; // one line is eliminated, so recheck this line.
                upper++; // the upper bound is moved down
                Refresh();
            }
            if (elimCount != 0 && AddScoreEvent != null) {
                AddScoreEvent(this, new AddScoreEventArgs(elimCount));
            }

        }

        #endregion

        #region private members
        /// <summary>
        /// a random number generator.
        /// </summary>
        private Random rnd = new Random((int)DateTime.Now.Ticks);

        #endregion
    }

    public class AddScoreEventArgs : EventArgs {
        public int Count {get; set;}
        public AddScoreEventArgs(int count) {
            Count = count;
        }
    }

   
}
