using System.Collections.Generic;

namespace Minesweeper_Alpha
{
    public class MinesweeperConfig
    {
        public List<BoomsPosition> _BoomsPosition = new List<BoomsPosition>();
        public List<NumberPosition> _NumberPosition = new List<NumberPosition>();
        public List<SaveAreaPosition> _SaveAreaPosition = new List<SaveAreaPosition>();
        public List<InitXY> InitXY = new List<InitXY>();
    }

    public class InitXY
    {
        public int x { get; set; }

        public int y { get; set; }
    }

    public class BoomsPosition
    {
        public int Booms_x { get; set; }

        public int Booms_y { get; set; }
    }

    public class NumberPosition
    {
        public int Number_x { get; set; }

        public int Number_y { get; set; }
    }

    public class SaveAreaPosition
    {
        public int SaveArea_x { get; set; }

        public int SaveArea_y { get; set; }
    }
}
