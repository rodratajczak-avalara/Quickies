using Newtonsoft.Json;
using QrBarCodeTestUtility.Models;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;

namespace QrBarCodeTestUtility
{
    public class GeneratePDF
    {
        private RestClient _taxFormCatalogClient = new RestClient("https://taxformcatalog-qa.returns.avalara.net");
        private RestClient _returnsEngineClient = new RestClient("https://returnsengine-qa.returns.avalara.net");
     

        public async Task<FormExtract> GetFormDefinition(string taxFormCode, int filingYear, int filingMonth)
        {
            return await _taxFormCatalogClient
                .Get<FormExtract>("api/FormDefExport?taxFormCode={0}&filingYear={1}&filingMonth={2}&productionOnly=true",
                    taxFormCode,
                    filingYear,
                    filingMonth);
        }

        public async Task<byte[]> GeneratePdf(object cfd)
        {
            var res = await _returnsEngineClient
                .WithBody(cfd)
                .WithContentType(ContentType.Pdf)
                .Post<byte[]>("api/pdfmerger");


            return res;
        }


        public async Task<ComputedFormData> GenerateComputedFormData(object inputData, string formExtract)
        {
            CdrRequest request = new CdrRequest
            {
                FormExtract = JsonConvert.DeserializeObject(formExtract),
                InputData = inputData,
            };

            return await _returnsEngineClient
                .WithBody(request)
                .Post<ComputedFormData>("api/computedformdata2?combinedOutput=false");
        }

        public EwayBillTfcModel PopulateTFCInputObject()
        {
            var json = File.ReadAllText(@"C:\avalara\git\quickies\QrBarCodeTestUtility\QrBarCodeTestUtility\TestData\data.json");
            var input = JsonConvert.DeserializeObject<EwayBillTfcModel>(json);
            return input;
        }
    }

    public class CdrRequest
    {
        public object FormExtract { get; set; }
        public object InputData { get; set; }
    }
}
