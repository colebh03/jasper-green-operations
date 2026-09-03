using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace JasperGreen.Models
{
    public class PdfMyHtmlService
    {        
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;
        
        public PdfMyHtmlService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<byte[]> GeneratePdfAsync(string html)
        {
            var sw = Stopwatch.StartNew();

            Console.WriteLine(
                $"PDF Request HTML Size: {html.Length:N0} characters");

            string apiKey =
                _configuration["PdfMyHtml:ApiKey"]!;

            string submitUrl =
                "https://api.pdfmyhtml.com/v1/html-to-pdf";

            // Serialize the rendered invoice HTML into the API request payload
            var payload = new
            {
                html = html
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            // Authenticate the request using the configured API key
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "X-API-Key",
                apiKey);

            Console.WriteLine("Submitting PDF job...");

            // Submit the HTML for asynchronous PDF generation
            var submitResponse =
                await _httpClient.PostAsync(
                    submitUrl,
                    content);

            submitResponse.EnsureSuccessStatusCode();

            Console.WriteLine(
                $"PDF job submitted after {sw.ElapsedMilliseconds}ms");

            string submitJson =
                await submitResponse.Content.ReadAsStringAsync();

            using JsonDocument submitDoc =
                JsonDocument.Parse(submitJson);

            // Capture the job ID used to poll the conversion status
            string jobId =
                submitDoc.RootElement
                    .GetProperty("job_id")
                    .GetString()!;

            Console.WriteLine(
                $"Job ID: {jobId}");

            int pollCount = 0;

            // Poll the API until the conversion completes or fails
            while (true)
            {
                await Task.Delay(250);

                pollCount++;

                string statusUrl =
                    $"https://api.pdfmyhtml.com/v1/jobs/{jobId}";

                var statusResponse =
                    await _httpClient.GetAsync(statusUrl);

                statusResponse.EnsureSuccessStatusCode();

                string statusJson =
                    await statusResponse.Content.ReadAsStringAsync();

                using JsonDocument statusDoc =
                    JsonDocument.Parse(statusJson);

                string status =
                    statusDoc.RootElement
                        .GetProperty("status")
                        .GetString()!;

                Console.WriteLine(
                    $"[{sw.ElapsedMilliseconds}ms] Poll #{pollCount} - Status: {status}");

                if (status == "COMPLETED")
                {
                    Console.WriteLine(
                        $"PDF completed after {sw.ElapsedMilliseconds}ms");

                    string pdfUrl =
                        statusDoc.RootElement
                            .GetProperty("download_url")
                            .GetString()!;

                    Console.WriteLine(
                        "Downloading PDF...");

                    var downloadSw = Stopwatch.StartNew();

                    // Download and return the completed PDF as a byte array
                    byte[] pdfBytes =
                        await _httpClient.GetByteArrayAsync(pdfUrl);

                    downloadSw.Stop();

                    Console.WriteLine(
                        $"PDF download completed in {downloadSw.ElapsedMilliseconds}ms");

                    Console.WriteLine(
                        $"Total GeneratePdfAsync time: {sw.ElapsedMilliseconds}ms");

                    return pdfBytes;
                }

                // Stop polling if the external conversion service reports a failure
                if (status == "FAILED")
                {
                    Console.WriteLine(
                        $"PDF generation failed after {sw.ElapsedMilliseconds}ms");

                    throw new Exception(
                        "PDF generation failed.");
                }
            }
        }
    }
}