using System;
using System.Drawing;
using System.Drawing.Printing;

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
        pd.PrinterSettings.PrinterName = "EPSON L8180 Series";   // exact Windows printer name

        // choose one:
        PaperSize size10x148 = new PaperSize("10x14.8cm", 394, 583); // 10×14.8 cm
        // PaperSize size13x18 = new PaperSize("13x18cm", 512, 709); // 13×18 cm

        pd.DefaultPageSettings.PaperSize = size10x148;
        pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
        pd.OriginAtMargins = false;

        pd.PrintPage += (s, e) =>
        {
            using (Image img = Image.FromFile(file))
            {
                Rectangle page = e.PageBounds;

                float ratioX = (float)page.Width / img.Width;
                float ratioY = (float)page.Height / img.Height;
                float ratio = Math.Min(ratioX, ratioY);

                int newWidth = (int)(img.Width * ratio);
                int newHeight = (int)(img.Height * ratio);

                int posX = page.Left + (page.Width - newWidth) / 2;
                int posY = page.Top + (page.Height - newHeight) / 2;

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

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
