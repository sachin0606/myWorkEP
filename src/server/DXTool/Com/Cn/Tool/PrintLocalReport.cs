using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Com.Cn.Tool
{
	public class PrintLocalReport : IDisposable
	{
		private IList<Stream> m_streams;

		private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
		{
			Stream stream = new FileStream("..\\..\\" + name + "." + fileNameExtension, FileMode.Create);
			m_streams.Add(stream);
			return stream;
		}

		private void Export(LocalReport report, DeviceInfo deviceInfo)
		{
			string text = "cm";
			string deviceInfo2 = "<DeviceInfo>  <OutputFormat>" + deviceInfo.OutputFormat.ToString() + "</OutputFormat>  <PageWidth>" + deviceInfo.PageWidth + text + "</PageWidth>  <PageHeight>" + deviceInfo.PageHeight + text + "</PageHeight>  <MarginTop>" + deviceInfo.MarginTop + text + "</MarginTop>  <MarginLeft>" + deviceInfo.MarginLeft + text + "</MarginLeft>  <MarginRight>" + deviceInfo.MarginRight + text + "</MarginRight>  <MarginBottom>" + deviceInfo.MarginBottom + text + "</MarginBottom></DeviceInfo>";
			m_streams = new List<Stream>();
			Warning[] warnings;
			report.Render("Image", deviceInfo2, CreateStream, out warnings);
			foreach (Stream stream in m_streams)
			{
				stream.Position = 0L;
			}
		}

		public void PrintReport(LocalReport report, DeviceInfo di)
		{
			if (!string.IsNullOrEmpty(report.ReportPath) && !File.Exists(report.ReportPath))
			{
				MessageBox.Show("The file is not exists");
			}
			Export(report, di);
			PrintStreams printStreams = new PrintStreams("", new PaperSize("custom size", (int)Tool.MillimetersToInch(di.PageWidth * 10m * 100m), (int)Tool.MillimetersToInch(di.PageHeight * 10m * 100m)), new Margins(0, 0, 0, 0), m_streams);
			printStreams.Print();
		}

		public void PrintReport(LocalReport report, string printerName, DeviceInfo di)
		{
			if (!string.IsNullOrEmpty(report.ReportPath) && !File.Exists(report.ReportPath))
			{
				MessageBox.Show("The file is not exists");
			}
			Export(report, di);
			PrintStreams printStreams = new PrintStreams(printerName, new PaperSize("custom size", (int)Tool.MillimetersToInch(di.PageWidth * 10m * 100m), (int)Tool.MillimetersToInch(di.PageHeight * 10m * 100m)), new Margins(0, 0, 0, 0), m_streams);
			printStreams.Print();
		}

		public void PrintPreviewReport(LocalReport report, DeviceInfo di)
		{
			if (!string.IsNullOrEmpty(report.ReportPath) && !File.Exists(report.ReportPath))
			{
				MessageBox.Show("The file is not exists");
			}
			Export(report, di);
			PrintStreams printStreams = new PrintStreams("", new PaperSize("custom size", (int)Tool.MillimetersToInch(di.PageWidth * 10m * 100m), (int)Tool.MillimetersToInch(di.PageHeight * 10m * 100m)), new Margins(0, 0, 0, 0), m_streams);
			printStreams.PrintPreview();
		}

		public void PrintPreviewReport(LocalReport report, string printerName, DeviceInfo di)
		{
			if (!string.IsNullOrEmpty(report.ReportPath) && !File.Exists(report.ReportPath))
			{
				MessageBox.Show("The file is not exists");
			}
			Export(report, di);
			PrintStreams printStreams = new PrintStreams(printerName, new PaperSize("custom size", (int)Tool.MillimetersToInch(di.PageWidth * 10m * 100m), (int)Tool.MillimetersToInch(di.PageHeight * 10m * 100m)), new Margins(0, 0, 0, 0), m_streams);
			printStreams.PrintPreview();
		}

		public void Dispose()
		{
			if (m_streams == null)
			{
				return;
			}
			foreach (Stream stream in m_streams)
			{
				stream.Close();
			}
			m_streams = null;
		}
	}
}
