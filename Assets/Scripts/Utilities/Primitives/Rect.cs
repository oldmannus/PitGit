using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pit.Utilities
{
    struct FRect
    {
        public int x;
        public int y;
        public int Width;
        public int Height;


        public FRect(FRect source)
        {
            x = source.x;
            y = source.y;
            Width = source.Width;
            Height = source.Height;
        }

        public FRect(int px, int py, int pwidth, int pheight)
        {
            x = px;
            y = py;
            Width = pwidth;
            Height = pheight;
        }

        public IRect ToIRect()
        {
            IRect r = new IRect();
            r.x = x;
            r.y  = y;
            r.Width = Width;
            r.Height = Height;

            return r;
        }
    }


    struct IRect
    {
        public int x;
        public int y;
        public int Width;
        public int Height;


        public IRect( IRect source )
        {
            x = source.x;
            y = source.y;
            Width = source.Width;
            Height = source.Height;
        }

        public IRect( int px, int py, int pwidth, int pheight)
        {
            x = px;
            y = py;
            Width = pwidth;
            Height = pheight;
        }

        public IRect ToRectangle()
        {
            IRect r = new IRect();
            r.x = x;
            r.y = y;
            r.Width = Width;
            r.Height = Height;

            return r;
        }
    }
}
