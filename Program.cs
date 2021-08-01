using QRCoder;

using SelectPdf;

using System.Drawing;

namespace HtmlToPdf
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            string imgPath = @"C:\Users\aseel\Desktop\qr.png";
            string centerImage = @"C:\Users\aseel\Desktop\center logo.png";
            string outputPath = @"C:\Users\aseel\Desktop\sample.pdf";
            string qr_text = "The text which should be encoded.";
            string htmlInput = @"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Document</title>
</head>
<body>
    <header>
        <img src='https://leanoverlines.com//logo.png' alt='Logo Image' id='logo' width='250'>
        <img src='https://leanoverlines.com/ld-files/QR_$qrId.png' width='150' alt='QR-Code' style='right: 5px; position: absolute;'>
        
    </header>
    <br><br>
    <h1>WAYBILL</h1>
    <br>
    <table cellpadding='7' border='1' style='border-collapse: collapse;width:80%'>
        <tr style='background-color: #ccc;'>
            <th colspan='3'>DISPATCH SECTION</th> 
        </tr> 
        <tr>
            <td colspan='2'><b>&nbsp;FROM:</b> $from</td>
            <td >&nbsp;<b>TO:</b> $to</td>
        </tr>
        <tr>
            <td>&nbsp;<b>DATE:</b> $date</td>
            <td>&nbsp;<b>CONSIGNEE:</b> $id</td>
            <td>&nbsp;<b>Truck #</b> $palette: </td>
        </tr>
    </table>
    <br> 
    <br>
<table  cellpadding='7' border='1' style='border-collapse: collapse;width:100%'>
<thead>
    <tr style='background-color: #ccc;'>
        <th>Commodity</th>
        <th>Unit Type</th>
        <th>Unit Weight</th>
        <th>Number of Units</th>
    </tr>
    <tr>
        <td>$Commodity</td>
        <td>$Unit_Type</td>
        <td>$Unit_Weight</td>
        <td>$Number_of_Units</td>
    </tr>
</thead>
<tbody id='orderItems'></tbody>
</table>
<br>
    <br> 
<table  cellpadding='7' border='1' style='border-collapse: collapse;width:60%'>
    <tr style='background-color: #ccc;'>
        <th  colspan='2'>REMARKS</th> 
    </tr> 
    <tr>
        <td>&nbsp;<b>Driver’s Name:</b> $driver</td>
        <td>&nbsp;<b>Dispatched by:</b> $account</td>
    </tr>
    <tr>
        <td >&nbsp;<b>Driving License/Permit #:</b> $Nid</td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td>&nbsp;</td>
    </tr>
</table>
<br>
<div style='position: absolute;color: #F10001;clear:both;text-align:center;padding:5px;right: 0;width: 35%;border-radius: 15px;bottom: 5px;margin: 0 auto;'>
    Powered by :  <a style='color: #F10001;' href='https://leanoverlines.com'><b>Lean Over Lines</b></a> 
</div>
</body>
</html>";
            ConvertHtmlToPDF(htmlInput, imgPath, centerImage, qr_text, outputPath);

        }

        private static void ConvertHtmlToPDF(string htmlInput, string imgPath, string centerImage, string qr_text, string outputPath)
        {
            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
            if (System.IO.File.Exists(outputPath))
            {
                System.IO.File.Delete(outputPath);
            }

            using (QRCodeGenerator qrGenerator = new())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qr_text, QRCodeGenerator.ECCLevel.Q, true, true, QRCodeGenerator.EciMode.Utf8))
            using (QRCode qrCode = new(qrCodeData))
            using (Bitmap qrCodeImage = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.White, (Bitmap)Bitmap.FromFile(centerImage)))
            {
                qrCodeImage.Save(imgPath);
            }


            PdfPageSize pageSize = PdfPageSize.A4;
            PdfPageOrientation pdfOrientation = PdfPageOrientation.Portrait;
            int webPageWidth = 1024;
            int webPageHeight = 0;


            // instantiate a html to pdf converter object
            SelectPdf.HtmlToPdf converter = new();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(htmlInput);

            // save pdf document
            doc.Save(outputPath);

            // close pdf document
            doc.Close();
        }
    }
}
