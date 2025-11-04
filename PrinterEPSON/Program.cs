using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Numerics;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

class SilentPrinter
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: SilentPrinter <image_path>");
            return;
        }

        string file = args[0];

        if (!System.IO.File.Exists(file))
        {
            Console.WriteLine("File not found: " + file);
            return;
        }

        PrintDocument pd = new PrintDocument();
        //pd.PrinterSettings.PrinterName = "EPSON L8180 Series";   // exact Windows printer name

        foreach (string name in PrinterSettings.InstalledPrinters) 
        { 
            if (name.Contains("EPSON"))
            {
                pd.PrinterSettings.PrinterName = name;
            }
        }

        // choose one:
        //PaperSize size10x148 = new PaperSize("10x14.8cm", 394, 583); // 10×14.8 cm
        PaperSize size10x15 = new PaperSize("10x15cm", 400, 600); // 10x15 cm
        //PaperSize size10x152 = new PaperSize("10x15.2cm", 394, 600); // 10×15.2 cm
        // PaperSize size13x18 = new PaperSize("13x18cm", 512, 709); // 13×18 cm

        //var ps = pd.PrinterSettings.PaperSizes.Cast<PaperSize>().FirstOrDefault(p => p.PaperName.Contains("4x6"));
        //if (ps != null)
        //{
        //    pd.DefaultPageSettings.PaperSize = ps;
        //}

        //PaperSize selected = pd.DefaultPageSettings.PaperSize;

    //    PaperSize selected = pd.PrinterSettings.PaperSizes
    //.Cast<PaperSize>()
    //.FirstOrDefault(ps => ps.PaperName.Contains("100") && ps.PaperName.Contains("148"))
    //?? pd.DefaultPageSettings.PaperSize;

        //foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
        //{
        //    if (ps.PaperName.Contains("10") && ps.PaperName.Contains("15"))
        //    {
        //        selected = ps;
        //        break;
        //    }
        //}

        pd.DefaultPageSettings.PaperSize = size10x15;
        pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        pd.OriginAtMargins = false;

        pd.DefaultPageSettings.Landscape = false;


        pd.PrintPage += (s, e) =>
        {
            using (Image img = Image.FromFile(file))
            {
                //RectangleF printable = e.PageSettings.PrintableArea;
                //float printableWidth = printable.Width;
                //float printableHeight = printable.Height;
                //
                //float ratioX = printableWidth / img.Width;
                //float ratioY = printableHeight / img.Height;
                //float ratio = Math.Min(ratioX, ratioY);
                //
                //float newWidth = img.Width * ratio;
                //float newHeight = img.Height * ratio;
                //
                //float posX = printable.Left + (printableWidth - newWidth) / 2;
                //float posY = printable.Top + (printableHeight - newHeight) / 2;
                //
                //float overscale = 1.02f; // 2% bleed
                //newHeight = (int)(newHeight * overscale);
                //posY = page.Top - (int)((newHeight - page.Height) / 2);
                //
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                //
                //float offsetX = 4f; // move image right (positive)
                //float offsetY = 6f; // move image down (positive)
                //
                //e.Graphics.DrawImage(img, posX + offsetX, posY + offsetY, newWidth, newHeight + offsetY * 2);

                Rectangle page = e.PageBounds;
                //float overscale = 1.02f;

                float ratioX = (float)page.Width / img.Width; 
                float ratioY = (float)page.Height / img.Height; 
                float ratio = Math.Min(ratioX, ratioY); 

                int newWidth = (int)(img.Width * ratio); 
                int newHeight = (int)(img.Height * ratio); 

                //int posX = page.Left + (page.Width - newWidth) / 2; 
                int posX = (page.Width - newWidth) / 2;
                //int posY = page.Top + (page.Height - newHeight) / 2;
                int posY = (page.Height - newHeight) / 2;

                //float offsetX = 12f;
                //float offsetY = 5f;

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; 
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                //int shiftX = 3;  // ≈0.5 mm at 300 DPI
                //int shiftY = 5; // move image slightly up
                //e.Graphics.TranslateTransform(shiftX, shiftY);
                //
                //float scale = 0.97f; // 97% of original size
                //e.Graphics.ScaleTransform(scale, scale);
                ////e.PageSettings.PaperSize = new PaperSize("10x15cm", 400, 600);

                e.Graphics!.DrawImage(img, posX, posY, newWidth, newHeight);
            }
        };

        try
        {
            pd.Print();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
        }
    }
}
