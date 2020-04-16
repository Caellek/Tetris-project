using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Tetris : Form
    {
        private Graphics g, gNext;
        private Board board;
        SoundPlayer sound = new SoundPlayer(@"Tetris.wav");
        public Tetris()
        {
            InitializeComponent();

            g = boardPBox.CreateGraphics();
            gNext = NextFigureBox.CreateGraphics();

            FallingTimer.Interval = 600 - 100 * 1; //1 jest poziom
        }
        
        //Game Over
        private void InitGameOver()
        {
            FallingTimer.Enabled = false;
            sound.Stop();
            StartStopButton.Text = "Start";

            if(Board.ScoresTable.Count > Board.MaxScores)
            {
                foreach(var player in Board.ScoresTable)
                {
                    if(board.Score > player.Wynik || Board.ScoresTable.Count < Board.MaxScores)
                    {
                        SaveScore save = new SaveScore();
                        save.ShowDialog();
                        if (!String.IsNullOrWhiteSpace(save.Nickname))
                            board.SaveScore(save.Nickname);
                        break;
                    }
                }
            }
            else
            {
                SaveScore save = new SaveScore();
                save.ShowDialog();
                if (!String.IsNullOrWhiteSpace(save.Nickname))
                    board.SaveScore(save.Nickname);
            }

            board = null;
            g.Clear(Board.BackColor);
            gNext.Clear(Board.BackColor);
            GameOverLabel.Visible = true;
        }

        //Timer
        private void FallingTimer_Tick(object sender, EventArgs e)
        {
            if (board.CurrentFigure.CanMove(Direction.Down, board.Grid))
                board.CurrentFigure.Move(Direction.Down);
            else
            {
                if(board.GameOver())
                {
                    InitGameOver();
                    return;
                }
                FallingTimer.Interval = 600 - 100 * board.Level;
                board.GetNextFigure();
            }
        }

        //Czytanie klawiszy
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F)
            {
                StartStop();
            }

            if(e.KeyCode == Keys.A)
            {
                if (board.CurrentFigure.CanMove(Direction.Left, board.Grid))
                    board.CurrentFigure.Move(Direction.Left);
            }
            if(e.KeyCode == Keys.D)
            {
                if (board.CurrentFigure.CanMove(Direction.Right, board.Grid))
                    board.CurrentFigure.Move(Direction.Right);
            }
            if(e.KeyCode == Keys.S)
            {
                if (board.CurrentFigure.CanMove(Direction.Down, board.Grid))
                    board.CurrentFigure.Move(Direction.Down);
            }
            if(e.KeyCode == Keys.W)
            {
                if (board.CurrentFigure.CanRotate(board.Grid))
                    board.CurrentFigure.Rotate();
            }
        }

        //Zachowanie przy Stacie/Pauzie gry
        public void StartStop()
        {
            if (FallingTimer.Enabled && board != null)
            {
                FallingTimer.Enabled = false;
                sound.Stop();
                StartStopButton.Text = "Start";
                RestartButton.Enabled = true;
                
            }
            else if (!FallingTimer.Enabled && board != null)
            {
                FallingTimer.Enabled = true;
                sound.PlayLooping();
                StartStopButton.Text = "Pauza";
                RestartButton.Enabled = false;
                
            }
        }
        
        //Przycisk start/stop
        private void StartStop_game(object sender, EventArgs e)
        {
            if (board == null)
            {
                InitGame();
                StartStopButton.Text = "Pauza";
            }
            else
                StartStop();
        }

        //Rozpoczęcie gry
        public void InitGame()
        {
            board = new Board(g, gNext);
            board.ScoreChanged += (object obj, EventArgs args) => scoreLabel.Text = "Wynik: " + board.Score;
            board.LevelChanged += (object obj, EventArgs args) => levelLabel.Text = "Poziom: " + board.Level;
            board.LineChanged += (object obj, EventArgs args) => LineLabel.Text = "Linie: " + board.Score/100;
            board.Line = 0;
            board.Score = 0;
            board.Level = 1;
            GameOverLabel.Visible = false;
            FallingTimer.Enabled = true;
            sound.PlayLooping();
        }

        //Otworzenie Tabeli wyników
        private void scoreTableOpen(object sender, EventArgs e)
        {
            ScoresTable scores = new ScoresTable();
            scores.ShowDialog();
        }

        //Przycisk restartu
        private void Restart_Click(object sender, EventArgs e)
        {
            InitGame();
            RestartButton.Enabled = false;
        }

        //Zapis wyniku
        private void SaveScore()
        {
            if (Board.ScoresTable.Count == 0)
                return;

            SaveScore saveScore = new SaveScore();
            saveScore.ShowDialog();

            if(!String.IsNullOrWhiteSpace(saveScore.Nickname))
            {
                Board.ScoresTable.Add(new Player(saveScore.Nickname, board.Score));
            }
        }
    }
}
