using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Extension.OfficeOpenXml.Excel
{
    /// <summary>
    /// A new wrapper class for an excel document opened with open xml
    /// </summary>
    public class ExcelFile
    {
        /// <summary>
        /// The spreadsheet document
        /// </summary>
        public SpreadsheetDocument Document;

        /// <summary>
        /// The workbook part
        /// </summary>
        public WorkbookPart WorkbookPart;

        /// <summary>
        /// The workbook which is the direct child of the workbookpart
        /// </summary>
        public Workbook Workbook;

        /// <summary>
        /// A sheets element holds all sheets within this file
        /// </summary>
        public Sheets Sheets;

        /// <summary>
        /// The styles part of this file
        /// </summary>
        private WorkbookStylesPart _workbookStylesPart;

        /// <summary>
        /// The stylesheet for this file which holds all styles for this document
        /// </summary>
        public Stylesheet Stylesheet;

        /// <summary>
        /// The list of sheets as wrapper objects
        /// </summary>
        public List<ExcelSheet> SheetList = new List<ExcelSheet>();

        /// <summary>
        /// Opens an exsiting excel file by its filenam
        /// </summary>
        /// <param name="fileName"></param>
        public void Open(string fileName, bool editable)
        {
            Document = SpreadsheetDocument.Open(fileName, editable);
            LoadDocument();
        }

        /// <summary>
        /// Opens an existing excel file from a filestream
        /// </summary>
        /// <param name="fileName"></param>
        public void Open(FileStream stream, bool editable)
        {
            Document = SpreadsheetDocument.Open(stream, editable);
            LoadDocument();
        }

        /// <summary>
        /// Opens an existing excel file from a filestream
        /// </summary>
        /// <param name="fileName"></param>
        public void Open(MemoryStream stream, bool editable)
        {
            Document = SpreadsheetDocument.Open(stream, editable);
            LoadDocument();
        }

        /// <summary>
        /// Creates a new excel file and returns an instance for it
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void Create(string fileName)
        {
            Document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook);
            GenerateEmptyChildElements();
            AddSheet("Sheet1");
        }

        /// <summary>
        /// Creates a new excel file and returns an instance for it
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void Create(MemoryStream stream)
        {
            Document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            GenerateEmptyChildElements();
            AddSheet("Sheet1");
        }

        /// <summary>
        /// Creates a new file with the style copied from this one
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ExcelFile CopyWithStyle(string fileName)
        {
            var file = new ExcelFile();
            file.Create(fileName);
            file.WorkbookPart.WorkbookStylesPart.Stylesheet = (Stylesheet)Stylesheet.CloneNode(true);
            file.Stylesheet = file.WorkbookPart.WorkbookStylesPart.Stylesheet;
            return file;
        }

        /// <summary>
        /// Adds a new sheet
        /// </summary>
        /// <param name="name"></param>
        public void AddSheet(string name)
        {
            var maxId = SheetList.Select(s => s.ThisSheet.SheetId).Max() ?? 0;
            var sheet = new ExcelSheet(this, name, maxId + 1);
            SheetList.Add(sheet);
        }

        /// <summary>
        /// Loads an excel document
        /// </summary>
        private void LoadDocument()
        {
            //Add workbook part
            WorkbookPart = Document.WorkbookPart;
            Workbook = WorkbookPart.Workbook;

            //Adds an empty sheets section
            Sheets = Workbook.Sheets;
            // For each sheet, create a new sheet class
            foreach (Sheet sheet in Sheets)
            {
                SheetList.Add(new ExcelSheet(this, sheet));
            }

            //Adds an empty stylesheet
            _workbookStylesPart = WorkbookPart.WorkbookStylesPart;
            Stylesheet = _workbookStylesPart.Stylesheet;
        }

        /// <summary>
        /// Generates the workbook parts with an empty workbook
        /// </summary>
        private void GenerateEmptyChildElements()
        {
            //Add workbook part
            WorkbookPart = Document.AddWorkbookPart();
            Workbook = new Workbook();
            WorkbookPart.Workbook = Workbook;

            //Adds an empty sheets section
            Sheets = new Sheets();
            Workbook.AppendChild(Sheets);

            //Adds an empty stylesheet
            _workbookStylesPart = WorkbookPart.AddNewPart<WorkbookStylesPart>();
            Stylesheet = CreateDefaultStyleSheet();
            _workbookStylesPart.Stylesheet = Stylesheet;
        }

        /// <summary>
        /// Creates a default style sheet
        /// </summary>
        /// <returns></returns>
        private Stylesheet CreateDefaultStyleSheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }
                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }) // Index 0 - default
                );

            Borders borders = new Borders(
                    new Border() // Index 0 - default
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat() // Index 0 - default
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        /// <summary>
        /// Get the value of the shared string by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SharedStringItem GetSharedStringItemById(int id)
        {
            return WorkbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        /// <summary>
        /// Saves the workbook and the document
        /// </summary>
        public void Save()
        {
            Workbook.Save();
            Document.Save();
        }
    }
}
