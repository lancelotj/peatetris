using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace peatetris {
    public partial class FormMain : Form {
        const int WM_KEYDOWN = 0x100;
        public FormMain() {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e) {
            gameArea1.NewBlock();
            this.Focus();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {


            if (msg.Msg == WM_KEYDOWN) {
                switch (keyData) {
                    case Keys.Left:
                        gameArea1.MoveLeft();
                        return true;

                    case Keys.Right:
                        gameArea1.MoveRight();
                        return true;

                    case Keys.Down:
                        gameArea1.MoveDown();
                        return true;
                    case Keys.Up:
                        gameArea1.RotateBlock();
                        return true;
                }

            }
            return false;
        }

        private void miExit_Click(object sender, EventArgs e) {
            Close();
        }


    }
}
