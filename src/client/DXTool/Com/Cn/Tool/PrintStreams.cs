using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Com.Cn.Tool
{
	public class PrintStreams
	{
		private PrintDocument printdoc = new PrintDocument();

		private PrintPreviewDialog printPreview = new PrintPreviewDialog();

		private string printerName;

		private PaperSize _CustomPaperSize;

		private int m_currentPageIndex;

		private Margins _margins;

		private IList<Stream> m_streams;

		private Bitmap _bitMap;

		public PaperSize CustomPaperSize
		{
			get
			{
				return _CustomPaperSize;
			}
			set
			{
				_CustomPaperSize = value;
			}
		}

		public Margins Margins
		{
			get
			{
				return _margins;
			}
			set
			{
				_margins = value;
			}
		}

		public Bitmap BitMap
		{
			get
			{
				return _bitMap;
			}
			set
			{
				_bitMap = value;
			}
		}

		private PrintStreams()
		{
		}

		public PrintStreams(string PrinterName, PaperSize papersize, Margins margins, Bitmap bitmap)
		{
			printerName = PrinterName;
			_CustomPaperSize = papersize;
			_margins = margins;
			_bitMap = bitmap;
			InitialPrint();
		}

		public PrintStreams(string PrinterName, PaperSize papersize, Margins margins, IList<Stream> streams)
		{
			printerName = PrinterName;
			_CustomPaperSize = papersize;
			_margins = margins;
			m_streams = streams;
			InitialPrint();
		}

		private void InitialPrint()
		{
			PrinterSettings printerSettings = new PrinterSettings();
			if (!string.IsNullOrEmpty(printerName))
			{
				printerSettings.PrinterName = printerName;
			}
			printerSettings.DefaultPageSettings.PaperSize = _CustomPaperSize;
			printerSettings.DefaultPageSettings.Margins = _margins;
			printdoc.PrinterSettings = printerSettings;
			printdoc.PrintPage += printdoc_PrintPage;
			printPreview.Document = printdoc;
			printPreview.Height = 500;
			printPreview.Width = 800;
		}

		private void printdoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			Metafile image = new Metafile(m_streams[m_currentPageIndex]);
			e.Graphics.DrawImage(image, e.PageBounds);
			m_currentPageIndex++;
			e.HasMorePages = m_currentPageIndex < m_streams.Count;
			if (e.HasMorePages)
			{
				return;
			}
			foreach (Stream stream in m_streams)
			{
				stream.Position = 0L;
			}
			m_currentPageIndex = 0;
		}

		public void PrintPreview()
		{
			printPreview.ShowDialog();
		}

		public void Print()
		{
			printdoc.Print();
		}
	}
}
