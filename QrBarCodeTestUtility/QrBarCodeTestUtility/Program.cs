using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace QrBarCodeTestUtility
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Generating PDF...!");
            GeneratePDFAsync();
            Console.WriteLine("Generating PDF Completed...!");
        }

        public static async void GeneratePDFAsync()
        {

            // Example 1:  Adding SignatureFieldNames to computed form data (cfd)
            var ewbNo = 321001986510;
            var pdfGenService = new GeneratePDF();           
            var tfcInput = pdfGenService.PopulateTFCInputObject();            
            var definition = await pdfGenService.GetFormDefinition("INEWAYBILL", DateTime.UtcNow.Year, DateTime.UtcNow.Month);
            var signatureFields = definition?.fields?
                .Where(x => x.pdfExport != null && (x.bulkReview?.validationTag?.Equals("signature", StringComparison.OrdinalIgnoreCase) ?? false))
                .Select(x => x.pdfExport.fieldName)
                .ToList();
            var cfd = await pdfGenService.GenerateComputedFormData(tfcInput, JsonConvert.SerializeObject(definition));

            // Add signature.
            if (signatureFields?.Any() ?? false) 
            {
                cfd.signatureFieldNames = signatureFields;
            }

            var fileContent = await pdfGenService.GeneratePdf(JObject.Parse(JsonConvert.SerializeObject(cfd)));
            File.WriteAllBytes(string.Format("c:\\temp\\TestQRCode\\{0}.pdf", ewbNo), fileContent);

        }
    }
}
