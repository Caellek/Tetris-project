using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class ScoresTable : Form
    {
        public ScoresTable()
        {
            InitializeComponent();

            //Pobranie Wyników i wyświetlenie ich w dataGridView, maksymalnie tyle ile zadeklarowano w klasie Board
            dataGridView1.DataSource = Board.ScoresTable.OrderByDescending(player => player.Wynik).Take(Board.MaxScores).ToList();
        }

    }
}
