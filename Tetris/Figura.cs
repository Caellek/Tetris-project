using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    //Dla ułatwienia definiuję klasy enum dla obracania figury, oraz poruszania nią
    public enum Angle { Deg0, Deg90, Deg180, Deg270}
    public enum Direction { Left, Right, Down}

    public static class ExtendAngle
    {
        public static Angle Next(this Angle angle)
        {
            switch(angle)
            {
                case Angle.Deg0:
                    return Angle.Deg90;
                case Angle.Deg90:
                    return Angle.Deg180;
                case Angle.Deg180:
                    return Angle.Deg270;
                case Angle.Deg270:
                    return Angle.Deg0;
                default:
                    return Angle.Deg0;
            }
        }

        public static Angle Prev(this Angle angle)
        {
            switch (angle)
            {
                case Angle.Deg0:
                    return Angle.Deg270;
                case Angle.Deg270:
                    return Angle.Deg180;
                case Angle.Deg180:
                    return Angle.Deg90;
                case Angle.Deg90:
                    return Angle.Deg0;
                default:
                    return Angle.Deg0;
            }
        }
    }

    public class Blok
    {
        public int horPos;
        public int verPos;
        public Color color = Color.Gray;
        public Point LocationPoint
        {
            get
            {
                return new Point(horPos * Lenght, verPos * Lenght);
            }
        }
        public Rectangle Rect
        {
            get
            {
                return new Rectangle(LocationPoint, Size);
            }

        }

        public static int Lenght = 30;
        public static Size Size
        {
            get
            {
                return new Size(Lenght, Lenght);
            }
        } 

        public static Rectangle GetRect(int horPos, int verPos)
        {
            return new Rectangle(new Point(horPos * Lenght, verPos * Lenght), Size);
        }

        public Blok(int horPos, int verPos)
        {
            this.horPos = horPos;
            this.verPos = verPos;
        }

        public Blok(int horPos, int verPos, Color color)
        {
            this.horPos = horPos;
            this.verPos = verPos;
            this.color = color;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(color), Rect);
            Pen pen = new Pen(Color.Gray, 1);
            pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            g.DrawRectangle(pen, Rect);
        }
    }
    public abstract class Figura
    {
        public int horPos;
        public int verPos;
        public Color Color = Color.Black;

        public int width;
        public int height;

        public Blok[] Blocks = new Blok[4];
        protected int[] hBlockPos = new int[4];
        protected int[] vBlockPos = new int[4];

        public Angle angle = Angle.Deg0;

        public event EventHandler PositionChanged;

        public Figura(int horPos, int verPos, Color color)
        {
            this.horPos = horPos;
            this.verPos = verPos;
            Color = color;
            InitBlocks();
        }

        public Figura(int horPos, int verPos)
        {
            this.horPos = horPos;
            this.verPos = verPos;
            InitBlocks();
        }

        protected abstract void InitBlockPosition();
        public void InitBlocks()
        {
            InitBlockPosition();
            for(int i = 0; i < 4; i++)
            {
                Blocks[i] = new Blok(hBlockPos[i], vBlockPos[i], Color);
            }
            PositionChanged?.Invoke(this, null);
        }

        public void Draw(Graphics g)
        {
            for (int i = 0; i < 4; i++)
                Blocks[i].Draw(g);
        }

        public void Draw(Graphics g, int horPos, int verPos)
        {
            int hPosSave = this.horPos;
            int vPosSave = this.verPos;

            this.verPos = verPos;
            this.horPos = horPos;
            InitBlocks();
            Draw(g);

            this.horPos = hPosSave;
            this.verPos = vPosSave;
            InitBlocks();
        }

        //Obracanie
        public void Rotate()
        {
            angle = angle.Next();
            InitBlocks();
        }

        //Poruszanie klockiem
        public void Move(Direction direction)
        {
            switch(direction)
            {
                case Direction.Left:
                    horPos--;
                    break;
                case Direction.Right:
                    horPos++;
                    break;
                case Direction.Down:
                    verPos++;
                    break;
            }
            InitBlocks();
        }

        //Sprawdzenie czy danym klockiem można w danym momencie poruszyć
        public bool CanMove(Direction direction, bool[,] Grid)
        {
            switch (direction)
            {
                case Direction.Left:
                    foreach (var block in Blocks)
                    {
                        if (block.horPos - 1 < 0 || Grid[block.verPos, block.horPos - 1] == true)
                            return false;
                    }
                    break;

                case Direction.Right:
                    foreach(var block in Blocks)
                    {
                        if (block.horPos + 1 >= Board.Width || Grid[block.verPos, block.horPos + 1] == true)
                            return false;
                    }
                    break;
                case Direction.Down:
                    foreach(var block in Blocks)
                    {
                        if (block.verPos + 1 >= Board.Height || Grid[block.verPos + 1, block.horPos] == true)
                            return false;
                    }
                    break;
            }
            return true;
        }

        //Sprawdzenie czy można dany klocek obrócić 
        public bool CanRotate(bool[,] Grid)
        {
            angle = angle.Next();
            InitBlockPosition();
            for(int i = 0; i < 4; i++)
            {
                if ( hBlockPos[i] < 0 || hBlockPos[i] >= Board.Width || vBlockPos[i] <0 ||
                    vBlockPos[i] >= Board.Height || Grid[vBlockPos[i],hBlockPos[i]] == true)
                {
                    angle = angle.Prev();
                    InitBlockPosition();
                    return false;
                }
            }
            angle = angle.Prev();
            InitBlockPosition();
            return true;
        }
    }

    //Klasa produkująca linię oraz jej obracanie
    public class Line : Figura
    {
        public Line(int horPos, int verPos, Color color) : base(horPos, verPos, color)
        {
            width = 1;
            height = 4;
        }

        public Line(int horPos, int verPos) : base(horPos, verPos)
        {
            width = 1;
            height = 1;
        }

        protected override void InitBlockPosition()
        {
            switch(angle)
            {
                case Angle.Deg0:
                case Angle.Deg180:
                    for(int i=0; i < 4; i++)
                    {
                        hBlockPos[i] = horPos;
                        vBlockPos[i] = verPos + i;
                    }
                    width = 1;
                    height = 4;
                    break;
                case Angle.Deg90:
                case Angle.Deg270:
                    for (int i = 0; i < 4; i++)
                    {
                        hBlockPos[i] = horPos + i;
                        vBlockPos[i] = verPos;
                    }
                    width = 4;
                    height = 1;
                    break;
            }
        }
    }

    //Klasa produkująca kwadrat oraz jego obracanie
    public class Square : Figura
    {
        public Square(int horPos, int verPos, Color color) : base (horPos, verPos, color)
        {
            width = 2;
            height = 2;
        }

        public Square(int horPos, int verPos) : base (horPos, verPos)
        {
            width = 2;
            height = 2;
        }

        protected override void InitBlockPosition()
        {
            hBlockPos[0] = horPos;
            vBlockPos[0] = verPos;

            hBlockPos[1] = horPos + 1;
            vBlockPos[1] = verPos;

            hBlockPos[2] = horPos;
            vBlockPos[2] = verPos + 1;

            hBlockPos[3] = horPos + 1;
            vBlockPos[3] = verPos + 1;

        }
    }

    //Lewy zygzak
    public class LeftThunder : Figura
    {
        public LeftThunder(int horPos, int verPos, Color color) : base(horPos, verPos, color)
        {
            width = 2;
            height = 3;
        }

        public LeftThunder(int horPos, int verPos) : base (horPos, verPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch(angle)
            {
                case Angle.Deg0:
                case Angle.Deg180:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos + 1;

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos + 1;
                    vBlockPos[3] = verPos + 2;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg90:
                case Angle.Deg270:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos + 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos - 1;

                    hBlockPos[3] = horPos + 2;
                    vBlockPos[3] = verPos - 1;

                    width = 3;
                    height = 2;
                    break;
            }
        }
    }

    //Prawy zygzak
    public class RightThunder : Figura
    {
        public RightThunder(int horPos, int verPos, Color color) : base(horPos, verPos, color)
        {
            width = 2;
            height = 3;
        }

        public RightThunder(int horPos, int verPos) : base(horPos, verPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                case Angle.Deg180:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos + 1;

                    hBlockPos[2] = horPos - 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos - 1;
                    vBlockPos[3] = verPos + 2;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg90:
                case Angle.Deg270:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos + 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos + 2;
                    vBlockPos[3] = verPos + 1;

                    width = 3;
                    height = 2;
                    break;
            }
        }
    }

    //Lewa część T
    public class LeftT : Figura
    {
        public LeftT(int horPos, int verPos, Color color) : base(horPos, verPos, color)
        {
            width = 2;
            height = 3;
        }

        public LeftT(int horPos, int verPos) : base (horPos, verPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch(angle)
            {
                case Angle.Deg0:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos + 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos + 1;
                    vBlockPos[3] = verPos + 2;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg90:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos + 1;

                    hBlockPos[2] = horPos - 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos - 2;
                    vBlockPos[3] = verPos + 1;

                    width = 3;
                    height = 2;
                    break;

                case Angle.Deg180:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos - 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos - 1;
                    vBlockPos[2] = verPos - 1;

                    hBlockPos[3] = horPos - 1;
                    vBlockPos[3] = verPos - 2;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg270:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos - 1;

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos - 1;

                    hBlockPos[3] = horPos + 2;
                    vBlockPos[3] = verPos - 1;

                    width = 3;
                    height = 2;
                    break;
            }
        }
    }

    //Prawa część T
    public class RightT : Figura
    {
        public RightT(int horPos, int verPos, Color color) : base(horPos, verPos, color)
        {
            width = 2;
            height = 3;
        }

        public RightT(int horPos, int verPos) : base(horPos, verPos)
        {
            width = 2;
            height = 3;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos - 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos - 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos - 1;
                    vBlockPos[3] = verPos + 2;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg90:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos - 1;

                    hBlockPos[2] = horPos - 1;
                    vBlockPos[2] = verPos - 1;

                    hBlockPos[3] = horPos - 2;
                    vBlockPos[3] = verPos - 1;

                    width = 3;
                    height = 2;
                    break;

                case Angle.Deg180:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos + 1;
                    vBlockPos[1] = verPos; 

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos - 1;

                    hBlockPos[3] = horPos + 1;
                    vBlockPos[3] = verPos - 2;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg270:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos + 1;

                    hBlockPos[2] = horPos + 1;
                    vBlockPos[2] = verPos + 1;

                    hBlockPos[3] = horPos + 2;
                    vBlockPos[3] = verPos + 1;

                    width = 3;
                    height = 2;
                    break;
            }
        }
    }

    //Małe T
    public class Triangle : Figura
    {
        public Triangle(int horPos, int verPos, Color color) : base(horPos, verPos, color)
        {
            width = 3;
            height = 2;
        }

        public Triangle(int horPos, int verPos) : base(horPos, verPos)
        {
            width = 3;
            height = 2;
        }

        protected override void InitBlockPosition()
        {
            switch (angle)
            {
                case Angle.Deg0:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos + 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos + 2;
                    vBlockPos[2] = verPos;

                    hBlockPos[3] = horPos + 1;
                    vBlockPos[3] = verPos + 1;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg90:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos + 1;

                    hBlockPos[2] = horPos;
                    vBlockPos[2] = verPos + 2;

                    hBlockPos[3] = horPos - 1;
                    vBlockPos[3] = verPos + 1;

                    width = 2;
                    height = 3;
                    break;

                case Angle.Deg180:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos - 1;
                    vBlockPos[1] = verPos;

                    hBlockPos[2] = horPos - 2;
                    vBlockPos[2] = verPos;

                    hBlockPos[3] = horPos - 1;
                    vBlockPos[3] = verPos - 1;

                    width = 3;
                    height = 2;
                    break;

                case Angle.Deg270:
                    hBlockPos[0] = horPos;
                    vBlockPos[0] = verPos;

                    hBlockPos[1] = horPos;
                    vBlockPos[1] = verPos - 1;

                    hBlockPos[2] = horPos;
                    vBlockPos[2] = verPos - 2;

                    hBlockPos[3] = horPos + 1;
                    vBlockPos[3] = verPos - 1;

                    width = 2;
                    height = 3;
                    break;
            }
        }
    }
}
