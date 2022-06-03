using Microsoft.SqlServer.Management.UI.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Sqlserver.maid.Infrastructures.Control
{
    public class GridResultControl : IDisposable
    {
        private readonly GridControl _gridControl;

        public int ColumnCount { get; private set; }

        public long RowCount { get; private set; }

        public bool IsDisposed { get; private set; }

        public GridResultControl(GridControl gridControl)
        {
            _gridControl = gridControl ?? throw new ArgumentNullException(nameof(gridControl));
            ColumnCount = gridControl.ColumnsNumber;
            RowCount = gridControl.GridStorage?.NumRows() ?? throw new ArgumentNullException(nameof(gridControl.GridStorage));
        }

        public string GetCellValue(long nRowIndex, int nColIndex)
        {
            return Function.Run(() =>
            {
                return _gridControl.GridStorage.GetCellDataAsString(nRowIndex, nColIndex);
            });
        }

        public string GetColumnHeader(int nColIndex)
        {
            return Function.Run(() =>
            {
                if (nColIndex > 0 && nColIndex < ColumnCount)
                {
                    _gridControl.GetHeaderInfo(nColIndex, out string result, out Bitmap _);
                    return result;
                }

                return string.Empty;
            });
        }

        public string[] GetColumnHeaders()
        {
            return Function.Run(() =>
            {
                var array = new string[ColumnCount - 1];
                for (int i = 1; i < ColumnCount; i++)
                {
                    array[i - 1] = GetColumnHeader(i);
                }

                return array;
            });
        }

        public GridResultSelectedCell GetSelectedCell()
        {
            return Function.Run(() =>
            {
                var selectedCells = GetSelectedCells();
                if (selectedCells.LongCount() > 0)
                    return selectedCells.ElementAt(0);

                return default;
            });
        }

        public int GetSelectedColumn()
        {
            return Function.Run(() => { return GetSelectedCell()?.ColumnIndex ?? -1; });
        }

        public long GetSelectedRow()
        {
            return Function.Run(() => { return GetSelectedCell()?.RowIndex ?? -1; });
        }

        public IEnumerable<GridResultSelectedCell> GetSelectedCells()
        {
            return Function.Run(() =>
            {
                return _gridControl.SelectedCells
                .Cast<BlockOfCells>()
                .Select((Func<BlockOfCells, GridResultSelectedCell>)((item) => item));
            });
        }

        public void Dispose()
        {
            if (IsDisposed)
                return;

            GC.ReRegisterForFinalize(ColumnCount);
            GC.ReRegisterForFinalize(RowCount);
            GC.ReRegisterForFinalize(_gridControl);
            GC.ReRegisterForFinalize(this);

            IsDisposed = true;
        }
    }
}