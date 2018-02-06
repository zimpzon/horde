namespace HordeEngine
{
    public class LogicalMap
    {
        public int[] walls;
        public int[] floor;
        public int[] props;
        public int Stride;
        public int Width;
        public int Height;
        public int Margin = 0;

        public void SetBounds(int w, int h, int stride)
        {
            Width = w;
            Height = h;
            Stride = stride;
        }

        public void EnsureAllocatedSizeFromBounds()
        {
            int size = Width * Height;
            if (walls == null || walls.Length < size)
            {
                walls = new int[size];
                floor = new int[size];
                props = new int[size];
            }
        }
    }
}
