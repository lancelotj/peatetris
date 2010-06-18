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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace peatetris {
    /// <summary>
    /// The main window
    /// </summary>
    public partial class FormMain : Form {
        /// <summary>
        /// Windows message number for key down event.
        /// </summary>
        const int WM_KEYDOWN = 0x100;
        /// <summary>
        /// Creates the main window instants.
        /// </summary>
        public FormMain() {
            InitializeComponent();
        }
        /// <summary>
        /// Handles the start button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e) {
            gameArea.CurrentBlock = gameArea.NewBlock();
            nextBlk = gameArea.NewBlock();
            gameArea.CurrentBlock.Show();
            timer.Start();
            //this.Focus(); 
        }
        /// <summary>
        /// Handles the up/down/left/right key down event.
        /// </summary>
        /// <param name="msg">windows message</param>
        /// <param name="keyData">windows message data</param>
        /// <returns>true if the message is handled</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            if (msg.Msg == WM_KEYDOWN) {
                switch (keyData) {
                    case Keys.Left:
                        gameArea.MoveLeft();
                        return true;

                    case Keys.Right:
                        gameArea.MoveRight();
                        return true;

                    case Keys.Down:
                        gameArea.MoveDown();
                        return true;
                    case Keys.Up:
                        gameArea.RotateBlock();
                        return true;
                }

            }
            return false;
        }

        /// <summary>
        /// Handles the exit menu click event.
        /// </summary>
        /// <param name="sender">this form</param>
        /// <param name="e">empty event arg</param>
        private void miExit_Click(object sender, EventArgs e) {
            Close();
        }

        /// <summary>
        /// Forces the block move down at the timer tick.
        /// </summary>
        /// <param name="sender">this form</param>
        /// <param name="e">empty event arg</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            gameArea.MoveDown();
        }

        private void gameArea_StopMoveEvent(object sender, EventArgs e) {
            timer.Stop();
        }

        private void gameArea_StartNewEvent(object sender, EventArgs e) {
            nextBlock.Show();
            gameArea.CurrentBlock = nextBlock;
            nextBlock = gameArea.NewBlock();
            timer.Start();
        }

        private void gameArea_AddScoreEvent(object sender, AddScoreEventArgs e) {
            score += 5*e.Count*e.Count + 5;
            elimRows += e.Count;
            lbElimRows.Text = "Rows: " + elimRows.ToString();
            lbScore.Text = "Score: " + score.ToString();
        }

        /// <summary>
        /// The game score
        /// </summary>
        private int score = 0;
        
        /// <summary>
        /// Eliminated rows count
        /// </summary>
        private int elimRows = 0;

        /// <summary>
        /// The next block to be dropped.
        /// </summary>
        private Block nextBlock = null;
    }
}
