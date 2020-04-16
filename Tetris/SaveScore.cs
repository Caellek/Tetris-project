using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class SaveScore : Form
    {
        public SaveScore()
        {
            InitializeComponent();
        }
        public string Nickname;
        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            Nickname = PlayerTextBox.Text;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
