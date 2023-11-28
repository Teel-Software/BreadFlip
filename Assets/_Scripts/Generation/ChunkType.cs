using System;

namespace BreadFlip.Generation
{
    public enum ChunkType
    {
        Table = ChunkTypes.Table,
        Sink = ChunkTypes.Sink,
        Stove = ChunkTypes.Stove,
        Wall = ChunkTypes.Wall,
    }

    [Flags]
    public enum ChunkTypes
    {
        Table = 1 << 0,
        Sink = 1 << 1,
        Stove = 1 << 2,
        Wall = 1 << 3,
    }
}