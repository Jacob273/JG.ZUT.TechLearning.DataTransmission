using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JG.TechLearning.DataTransmission
{
    /// <summary>
    /// Klasa pomocna do zapisywania danych w excelu. Lamie zasade "Open/closed" z SOLID :).
    /// </summary>
    public static class ExcelWriter
    {
        private const int FinalSampleValueColumnIndex = 1;
        private const int SampleDeltaTimeColumnIndex = 2;

        private const int RowsOffset = 2;

        private const int Sample1ValueColumnIndex = 4;
        private const int Sample2ValueColumnIndex = 5;
        
        private const int Signal1ColumnDataIndex = 6;
        private const int Signal2ColumnDataIndex = 7;
        private const int Signal3ColumnDataIndex = 8;
        private const int AdditionalDataColumnIndex = 9;



        private const int DFTRealColumnIndex = 1;
        private const int DFTImaginaryColumnIndex = 2;
        private const int DFTRealAndImaginarySummedColumnIndex = 3;

        public static void WriteSamples(string path, string excelFileName, List<Sample> samples, 
            SignalData inputData1, SignalData inputData2 = null, SignalData inputData3 = null, AdditionalSignalData inputData4 = null,
            List<Sample> samples1 = null, List<Sample> samples2= null)
        {
            Application app = null;
            Workbooks workBooks = null;
            Workbook workbook = null;
            Sheets worksheets = null;
            Worksheet worksheet = null;

            CreateExcelComponents(out app, out workBooks, out workbook, out worksheets, out worksheet);
            WriteSampleValuesToCells(samples, worksheet, FinalSampleValueColumnIndex, SampleDeltaTimeColumnIndex);
            WriteSignalDataToCells(inputData1, worksheet, Signal1ColumnDataIndex);
            WriteSignalDataToCells(inputData2, worksheet, Signal2ColumnDataIndex);
            WriteSignalDataToCells(inputData3, worksheet, Signal3ColumnDataIndex);
            WriteAdditionalDataToCells(inputData4, worksheet, AdditionalDataColumnIndex);
            WriteSampleValuesToCells(samples1, worksheet, Sample1ValueColumnIndex, 0);
            WriteSampleValuesToCells(samples2, worksheet, Sample2ValueColumnIndex, 0);

            SaveAndRelease(path, excelFileName, app, workBooks, workbook, worksheets, worksheet);
        }



        public static void WriteSignalDataAndDFTResult(string path, string excelFileName, FourierResult dftResult, SignalData inputData1 = null, SignalData inputData2 = null)
        {
            Application app = null;
            Workbooks workBooks = null;
            Workbook workbook1 = null;
            Sheets worksheets = null;
            Worksheet worksheet = null;
            CreateExcelComponents(out app, out workBooks, out workbook1, out worksheets, out worksheet);

            WriteDFTResultToCells(dftResult, worksheet, DFTRealColumnIndex, DFTImaginaryColumnIndex, DFTRealAndImaginarySummedColumnIndex);

            WriteSignalDataToCells(inputData1, worksheet, Signal1ColumnDataIndex);
            WriteSignalDataToCells(inputData2, worksheet, Signal2ColumnDataIndex);

            SaveAndRelease(path, excelFileName, app, workBooks, workbook1, worksheets, worksheet);
        }

        private static void SaveAndRelease(string path, string excelFileName, Application app, Workbooks workBooks, Workbook workbook, Sheets worksheets, Worksheet worksheet)
        {
            workbook.SaveAs($@"{path}\{DateTime.Now.ToString("HH_mm_ss_yyyyMMdd")}{excelFileName}", XlFileFormat.xlOpenXMLWorkbook);

            workbook.Close();
            Marshal.FinalReleaseComObject(workbook);
            workbook = null;

            workBooks.Close();
            Marshal.FinalReleaseComObject(workBooks);
            workBooks = null;

            Marshal.FinalReleaseComObject(worksheet);
            worksheet = null;

            Marshal.FinalReleaseComObject(worksheets);
            worksheets = null;


            app.Quit();
            Marshal.FinalReleaseComObject(app);
            app = null;
        }

        private static void CreateExcelComponents(out Application app, out Workbooks workBooks, out Workbook workbook, out Sheets worksheets, out Worksheet worksheet)
        {
            app = new Application();
            workBooks = app.Workbooks;
            workbook = workBooks.Add(Type.Missing);
            worksheets = workbook.Sheets;
            worksheet = (Worksheet)worksheets[1];
        }



        private static void WriteSampleValuesToCells(List<Sample> samples, Worksheet worksheet, int valueColumnIndex, int deltaTimeColumnIndex)
        {
            if(samples == null)
            {
                return;
            }

            if (valueColumnIndex != 0)
            {
                for (int i = 0; i < samples.Count; i++)
                {
                    int currentRow = i + RowsOffset;
                    var sample = samples[i];
                    worksheet.Cells[currentRow, valueColumnIndex].NumberFormat = "@";
                    worksheet.Cells[currentRow, valueColumnIndex].Value = sample.Value;
                }
            }

            if (deltaTimeColumnIndex != 0)
            {
                for (int i = 0; i < samples.Count; i++)
                {
                    int currentRow = i + RowsOffset;
                    var sample = samples[i];
                    worksheet.Cells[currentRow, deltaTimeColumnIndex].NumberFormat = "@";
                    worksheet.Cells[currentRow, deltaTimeColumnIndex].Value = sample.DeltaTime;
                }
            }
        }

        private static void WriteDFTResultToCells(FourierResult fourierResult, Worksheet worksheet, int realIndex, int imagIndex, int realAndImagSummed)
        {
            if (realIndex != 0)
            {
                for (int i = 0; i < fourierResult.RealComponents.Count; i++)
                {
                    int currentRow = i + RowsOffset;
                    var sample = fourierResult.RealComponents[i];
                    worksheet.Cells[currentRow, realIndex].NumberFormat = "@";
                    worksheet.Cells[currentRow, realIndex].Value = sample.Value;
                }
            }

            if (imagIndex != 0)
            {
                for (int i = 0; i < fourierResult.ImaginaryComponents.Count; i++)
                {
                    int currentRow = i + RowsOffset;
                    var sample = fourierResult.ImaginaryComponents[i];
                    worksheet.Cells[currentRow, imagIndex].NumberFormat = "@";
                    worksheet.Cells[currentRow, imagIndex].Value = sample.Value;
                }
            }

            if (realAndImagSummed != 0)
            {
                for (int i = 0; i < fourierResult.RealAndImagSummed.Count; i++)
                {
                    int currentRow = i + RowsOffset;
                    var sample = fourierResult.RealAndImagSummed[i];
                    worksheet.Cells[currentRow, realAndImagSummed].NumberFormat = "@";
                    worksheet.Cells[currentRow, realAndImagSummed].Value = sample.Value;
                }
            }
        }

        private static void WriteSignalDataToCells(SignalData inputData, Worksheet worksheet, int columnIndex)
        {
            if (inputData != null)
            {
                worksheet.Cells[0 + RowsOffset, columnIndex].Value = "freq: " + inputData.Frequency.ToString();
                worksheet.Cells[1 + RowsOffset, columnIndex].Value = "puls: " + inputData.Pulsation.ToString();
                worksheet.Cells[2 + RowsOffset, columnIndex].Value = "samplingRate: " + inputData.SamplingRate.ToString();
                worksheet.Cells[3 + RowsOffset, columnIndex].Value = "simulationTime: " + inputData.SimulationTime.ToString();
                worksheet.Cells[4 + RowsOffset, columnIndex].Value = "phase: " + inputData.Phase.ToString();
                worksheet.Cells[5 + RowsOffset, columnIndex].Value = "ampl: " + inputData.Amplitude.ToString();
                worksheet.Cells[6 + RowsOffset, columnIndex].Value = "period: " + inputData.Period.ToString();
            }
        }
        private static void WriteAdditionalDataToCells(AdditionalSignalData inputData, Worksheet worksheet, int columnIndex)
        {
            if (inputData != null)
            {
                worksheet.Cells[1, columnIndex].NumberFormat = "@";
                worksheet.Cells[1, columnIndex].Value = "factor: " + inputData.ModulationDepthFactor.ToString();
            }
        }
    }
}
