using Microsoft.SqlServer.Management.UI.Grid;

namespace Sqlserver.maid.Infrastructures.SqlControl
{
    public class GridResultSelectedCell
    {
        public int ColumnIndex { get; private set; }
        public long RowIndex { get; private set; }

        public GridResultSelectedCell(long nRowIndex, int nColIndex)
        {
            RowIndex = nRowIndex;
            ColumnIndex = nColIndex;
        }

        public static implicit operator GridResultSelectedCell(BlockOfCells blockOfCells)
        {
            return new GridResultSelectedCell(blockOfCells.OriginalY, blockOfCells.OriginalX);
        }
    }
}