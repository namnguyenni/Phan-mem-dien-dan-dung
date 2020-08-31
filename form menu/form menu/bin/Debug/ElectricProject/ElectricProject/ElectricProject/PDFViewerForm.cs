
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Interop.Word;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace ElectricProject
{
    public partial class PDFViewerForm : Form
    {
        public PDFViewerForm()
        {
            InitializeComponent();
            axAcroPDF1.setShowToolbar(true);
            axAcroPDF1.setShowScrollbars(true);
            
        }

        public void OpenFile(string src)
        {
            axAcroPDF1.src = src;
            if (panel2.Visible == true)
            {
                panel2.Visible = false;
            }
        }

            private void button_OpenOffice_Click(object sender, EventArgs e)
            {


                
                OpenFileDialog openfile = new OpenFileDialog()
                {
                    DefaultExt = ".pdf",
                    Filter = @"Data Files (*.xls, *.xlsx,*.ppt,*.pptx.*.doc,*.docx,*.pdf)|*.xls; *.xlsx; *.ppt; *.pptx; *.doc; *.docx; *.pdf|Pdf Files (*.pdf)|*.pdf|Excel Files (*.xls, *.xlsx)|
                    *.xls;*.xlsx|PowerPoint Files (*.ppt, *.pptx)|*.ppt;*.pptx|Word Files (*.doc, *.docx)|*.doc;*.docx"
                };

            if (openfile.ShowDialog() == DialogResult.OK)
                {

                if (panel2.Visible == true)
                {
                    panel2.Visible = false;
                }
                string file = openfile.FileName;
                    string outfile = Environment.CurrentDirectory + "/file.pdf";
                    string extension = System.IO.Path.GetExtension(file);
                    if (extension == ".doc" || extension == ".docx")
                    {
                        ConvertWordToPdf(file, outfile);
                    }

                    if (extension == ".ppt" || extension == ".pptx")
                    {
                        ConvertPowerPointToPdf(file, outfile);

                    }

                    if (extension == ".xls" || extension == ".xlsx")
                    {
                        ConvertExcelToPdf(file, outfile);
                    }

                    if (extension == ".pdf")
                    {
                        outfile = file;
                    }
                    axAcroPDF1.src = outfile;
                    
                }
            }

            private void Form2_FormClosed(object sender, FormClosedEventArgs e)
            {
                File.Delete(Environment.CurrentDirectory + "/file.pdf");
            }


            public bool ConvertWordToPdf(string inputFile, string outputfile)
            {

                Microsoft.Office.Interop.Word.Application wordApp =
                new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document wordDoc = null;
                object inputFileTemp = inputFile;

                try
                {
                    wordDoc = wordApp.Documents.Open(inputFile);
                    wordDoc.ExportAsFixedFormat(outputfile, WdExportFormat.wdExportFormatPDF);
                }
                catch (Exception)
                {
                    MessageBox.Show("Co loi xay ra");
                    return false;
                }
                finally
                {
                    if (wordDoc != null)
                    {
                        wordDoc.Close(WdSaveOptions.wdDoNotSaveChanges);
                    }
                    if (wordApp != null)
                    {
                        wordApp.Quit(WdSaveOptions.wdDoNotSaveChanges);
                        wordApp = null;
                    }
                }

                return true;
            }

            public static bool ConvertPowerPointToPdf(string inputFile, string outputfile)
            {
                string outputFileName = outputfile;
                Microsoft.Office.Interop.PowerPoint.Application powerPointApp =
                new Microsoft.Office.Interop.PowerPoint.Application();
                Presentation presentation = null;
                Presentations presentations = null;
                try
                {
                    presentations = powerPointApp.Presentations;
                    presentation = presentations.Open(inputFile, MsoTriState.msoFalse, MsoTriState.msoFalse,
                    MsoTriState.msoFalse);

                    presentation.ExportAsFixedFormat(outputFileName, PpFixedFormatType.ppFixedFormatTypePDF,
                    PpFixedFormatIntent.ppFixedFormatIntentScreen, MsoTriState.msoFalse,
                    PpPrintHandoutOrder.ppPrintHandoutVerticalFirst, PpPrintOutputType.ppPrintOutputSlides,
                    MsoTriState.msoFalse, null, PpPrintRangeType.ppPrintAll, string.Empty, false, true, true, true, false,
                    Type.Missing);
                }
                catch (Exception)
                {
                    MessageBox.Show("Co loi xay ra");
                    return false;
                }
                finally
                {
                    if (presentation != null)
                    {
                        presentation.Close();
                        Marshal.ReleaseComObject(presentation);
                        presentation = null;
                    }
                    if (powerPointApp != null)
                    {
                        powerPointApp.Quit();
                        Marshal.ReleaseComObject(powerPointApp);
                        powerPointApp = null;
                    }
                }
                return true;
            }

            public static bool ConvertExcelToPdf(string inputFile, string outputfile)
            {
                string outputFileName = outputfile;
                Microsoft.Office.Interop.Excel.Application excelApp =
                new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                Workbook workbook = null;
                Workbooks workbooks = null;
                try
                {
                    //ExportAsFixedFormatXlFixedFormatType
                    workbooks = excelApp.Workbooks;
                    workbook = workbooks.Open(inputFile);
                    workbook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, outputFileName,
                    XlFixedFormatQuality.xlQualityStandard, true, true, Type.Missing, Type.Missing, false, Type.Missing);
                }
                catch (Exception)
                {
                    MessageBox.Show("Co loi xay ra");
                    return false;
                }
                finally
                {
                    if (workbook != null)
                    {
                        workbook.Close(XlSaveAction.xlDoNotSaveChanges);
                        while (Marshal.FinalReleaseComObject(workbook) != 0) { };
                        workbook = null;
                    }
                    if (workbooks != null)
                    {
                        workbooks.Close();
                        while (Marshal.FinalReleaseComObject(workbooks) != 0) { };
                        workbooks = null;
                    }
                    if (excelApp != null)
                    {
                        excelApp.Quit();
                        excelApp.Application.Quit();
                        while (Marshal.FinalReleaseComObject(excelApp) != 0) { };
                        excelApp = null;
                    }
                }

                return true;
            }

        private void button_SaveOffice_Click(object sender, EventArgs e)
        {
            
        }

        private void button_PrintOffice_Click(object sender, EventArgs e)
        {
            axAcroPDF1.Print();
        }

        private void button_OpenOffice_MouseHover(object sender, EventArgs e)
        {
            textBox1.Visible = true;
        }

        private void button_OpenOffice_MouseLeave(object sender, EventArgs e)
        {
            textBox1.Visible = false;
        }
    }

}

