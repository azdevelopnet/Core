using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Extension.OfficeOpenXml.Excel
{
    public class ExcelRow
    {
        /// <summary>
        /// The excel file this row is a part of
        /// </summary>
        public ExcelFile ExcelFile;

        public Row ThisRow;
        public List<ExcelCell> Cells = new List<ExcelCell>();

        /// <summary>
        /// Creates an empty row
        /// </summary>
        public ExcelRow(ExcelFile file)
        {
            ExcelFile = file;
            ThisRow = new Row();
        }

        /// <summary>
        /// Opens a new row element from an excel file
        /// </summary>
        public ExcelRow(ExcelFile file, Row row)
        {
            ExcelFile = file;
            ThisRow = row;

            foreach(Cell cell in ThisRow.ChildElements)
            {
                Cells.Add(new ExcelCell(ExcelFile, cell));
            }
        }

        /// <summary>
        /// Adds a new cell to the row
        /// </summary>
        /// <param name="value"></param>
        public ExcelCell AddCell(string value)
        {
            var cell = new ExcelCell(ExcelFile, value);
            ThisRow.Append(cell.ThisCell);
            Cells.Add(cell);
            return cell;
        }

        /// <summary>
        /// Adds a new cell to the row
        /// </summary>
        /// <param name="value"></param>
        public ExcelCell AddCell(int value)
        {
            var cell = new ExcelCell(ExcelFile, value);
            ThisRow.Append(cell.ThisCell);
            Cells.Add(cell);
            return cell;
        }

        /// <summary>
        /// The value of a cell within the row by the index
        /// </summary>
        /// <returns></returns>
        public ExcelCell GetCellByColumnName(string name)
        {
            return Cells.FirstOrDefault(c => c.ColumnName == name);
        }

    }
}
