using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tetris
{
    //Klasa Gracza dla możliwości zapisu w tabeli wyników
    public class Player
    {
        public string Gracz { get; set; }
        public int Wynik { get; set; }

        public Player(string name, int score)
        {
            Gracz = name;
            Wynik = score;
        }

        public override string ToString()
        {
            return Gracz + ": " + Wynik;
        }
    }

    //Plansza gry
    class Board
    {
        public Board(Graphics g, Graphics gNext)
        {
            Graphics = g;
            GraphicsNext = gNext;
            GetNextFigure();
            InitGrid();

            ScoresTable = GetScoresTable();
        }

        public static int hstartPos = 5;

        public static Color BackColor = Color.Gray;

        public Graphics Graphics;
        public Graphics GraphicsNext;
        public Figura NextFigure;
        public Figura CurrentFigure;
        public List<Blok> Lying = new List<Blok>();
        private static List<Player> scoreTable;
        public static string ScoresTablePath = "Wyniki.txt";

        public static int Height = 20;
        public static int Width = 10;

        public bool[,] Grid = new bool[Height, Width];

        public static int ScoreForLine = 100;
        public static int ScoreForLevelUp = 1000;
        public static int LevelMax = 5;
        public static int MaxScores = 10;

        int line;
        public event EventHandler LineChanged;

        //Liczba wyczyszczonych linii
        public int Line
        {
            get
            {
                return line;
            }
            set
            {
                line = value;
                LineChanged?.Invoke(this, null);
            }
        }

        //Poziom
        int level;
        public event EventHandler LevelChanged;
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                LevelChanged?.Invoke(this, null);
            }
        }

        //Wielkość planszy do gry
        public static Size Size
        {
            get
            {
                return new Size(Width * Blok.Lenght, Height * Blok.Lenght);
            }
        }
        //Wielkość planszy dla następnych figur
        public static Size NextPanelSize
        {
            get
            {
                return new Size(6 * Blok.Lenght, 5 * Blok.Lenght);
            }
        }


        //Wynik
        int score;
        public event EventHandler ScoreChanged;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                ScoreChanged?.Invoke(this, null);
            }
        }

        //Pobieranie listy wyników
        public static List<Player> GetScoresTable()
        {
            List<Player> table = new List<Player>();

            if(File.Exists(ScoresTablePath))
            {
                StreamReader sr = new StreamReader(ScoresTablePath);
                string line = sr.ReadLine();
                while(!String.IsNullOrWhiteSpace(line))
                {
                    table.Add(new Player(line.Split(":".ToCharArray())[0], int.Parse(line.Split(":".ToCharArray())[1])));
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            return table;
        }

        public static List<Player> ScoresTable
        {
            get
            {
                return (scoreTable != null) ? scoreTable : GetScoresTable();
            }
            set
            {
                scoreTable = value;
            }
        }

        //Wybieranie kolejnej figury (losowo)
        private Figura GetRandomFigure()
        {
            Random r = new Random();
            switch(r.Next(1,8))
            {
                case 1:
                    return new Line(hstartPos, 0, Color.Blue);
                case 2:
                    return new Square(hstartPos, 0, Color.Red);
                case 3:
                    return new LeftThunder(hstartPos, 0, Color.Yellow);
                case 4:
                    return new RightThunder(hstartPos, 0, Color.Magenta);
                case 5:
                    return new LeftT(hstartPos, 0, Color.LimeGreen);
                case 6:
                    return new RightT(hstartPos, 0, Color.Orange);
                case 7:
                    return new Triangle(hstartPos, 0, Color.AliceBlue);
                default:
                    return new Triangle(hstartPos, 0, Color.AliceBlue);
            }
        }

        public void InitGrid()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Grid[i, j] = false;     //Pusta plansza
                }
            }
            
            foreach(var block in Lying)
            {
                Grid[block.verPos, block.horPos] = true;
            }
        }

        //Rysowanie planszy gry
        public void Draw(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(Size.Width, Size.Height);
            Graphics g = Graphics.FromImage(bitmap);

            InitGrid();
            g.Clear(BackColor);
            CurrentFigure.Draw(g);
            foreach(var block in Lying)
            {
                block.Draw(g);
            }
            Graphics.DrawImage(bitmap, new Point(0, 0));
        }

        //Rysowanie planszy z kolejnymi figurami
        public void DrawNext()
        {
            Bitmap bitmap = new Bitmap(NextPanelSize.Width, NextPanelSize.Height);
            Graphics g = Graphics.FromImage(bitmap);

            g.Clear(BackColor);
            NextFigure.Draw(g, 2, 1);
            GraphicsNext.DrawImage(bitmap, new Point(0, 0));
        }

        //Sprawdzanie czy linia została zapełniona oraz usunięcie jej
        //i przesunięcie pozostałych bloków w dół o zadaną ilość pól
        private bool FillRows(int i)
        {
            for(int j = 0; j < Width; j++)
            {
                if(Grid[i,j] != true)
                {
                    return false;
                }
            }
            return true;
        }

        private void DeleteFillRow()
        {
            for(int i = Height - 1; i>=0; i--)
            {
                if(FillRows(i))
                {
                    Lying.RemoveAll(block => block.verPos == i);

                    Score += ScoreForLine * Level;
                    Line = Score / 100;

                    if (Score / (ScoreForLevelUp * Level) > 0 && Level < LevelMax)
                        Level++;

                    Lying.Where(block => block.verPos < i).ToList().ForEach(block => block.verPos++);
                    InitGrid();
                    i++;
                }
            }
        }

        //Pobieranie kolejnej figury
        public void GetNextFigure()
        {
            if (NextFigure != null)
            {
                Lying.AddRange(CurrentFigure.Blocks);
                InitGrid();
                CurrentFigure = NextFigure;
            }
            else
                CurrentFigure = GetRandomFigure();

            CurrentFigure.PositionChanged += new EventHandler(Draw);
            NextFigure = GetRandomFigure();

            DeleteFillRow();

            Draw(null, null);
            DrawNext();
        }

        //Zapis wyniku
        public void SaveScore(string playerName)
        {
            ScoresTable.Add(new Player(playerName, Score));
            StreamWriter sr = new StreamWriter(ScoresTablePath);
            ScoresTable.ForEach(player => sr.WriteLine(player));
            sr.Close();
        }

        //Sprawdzenie czy wysokość leżących bloków osiągnęłą górną ściankę = koniec gry
        public bool GameOver()
        {
            if(Lying != null && Lying.Count > 0)
            {
                return Lying.Min(block => block.verPos) < NextFigure.height;
            }
            return false;
        }


    }
}