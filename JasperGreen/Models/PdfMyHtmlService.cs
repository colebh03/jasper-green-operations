/*
============================================================================
AUTHOR:       Cole Howell
COURSE:       ISTM 415
PROGRAM:      PdfMyHtmlService.cs

PURPOSE:      Handles communication with the pdfmyhtml API to generate
              PDF invoices from HTML content.

INPUT:        HTML string generated from Razor invoice view

PROCESS:      - Retrieves API key from configuration
              - Sends HTML to pdfmyhtml API
              - Receives generated PDF bytes

OUTPUT:       Returns generated PDF as byte array

HONOR CODE:   On my honor, as an Aggie, I have neither given nor received
              unauthorized aid on this academic work.
============================================================================
*/

using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace JasperGreen.Models
{
    public class PdfMyHtmlService
    {
        // Used to send HTTP requests to API
        private readonly HttpClient _httpClient;

        // Used to access appsettings.json values
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor using dependency injection.
        /// </summary>
        /// <param name="httpClient">Injected HttpClient</param>
        /// <param name="configuration">Injected configuration</param>
        public PdfMyHtmlService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        /* =====================================================================
   DOWNLOAD INVOICE PDF
   ===================================================================== */

        /// <summary>
        /// Sends HTML to pdfmyhtml API and returns generated PDF.
        /// </summary>
        /// <param name="html">Rendered invoice HTML</param>
        /// <returns>Generated PDF as byte array</returns>
        //    public async Task<byte[]> GeneratePdfAsync(string html)
        //    {
        //        // Retrieve API key from appsettings.json
        //        string apiKey =
        //            _configuration["PdfMyHtml:ApiKey"]!;

        //        // API endpoint
        //        string submitUrl =
        //            "https://api.pdfmyhtml.com/v1/html-to-pdf";

        //        // Create JSON payload
        //        var payload = new
        //        {
        //            html = html
        //        };

        //        // Convert payload to JSON
        //        var content = new StringContent(
        //            JsonSerializer.Serialize(payload),
        //            Encoding.UTF8,
        //            "application/json");

        //        // Clear previous headers
        //        _httpClient.DefaultRequestHeaders.Clear();

        //        // Add API key
        //        _httpClient.DefaultRequestHeaders.Add(
        //            "X-API-Key",
        //            apiKey);

        //        // Submit PDF generation job
        //        var submitResponse =
        //            await _httpClient.PostAsync(submitUrl, content);

        //        submitResponse.EnsureSuccessStatusCode();

        //        // Read JSON response
        //        string submitJson =
        //            await submitResponse.Content.ReadAsStringAsync();

        //        // Parse JSON
        //        using JsonDocument submitDoc =
        //            JsonDocument.Parse(submitJson);

        //        // Extract job_id
        //        string jobId =
        //            submitDoc.RootElement
        //                .GetProperty("job_id")
        //                .GetString()!;

        //        // Poll until completed
        //        while (true)
        //        {
        //            // Wait before checking status
        //            await Task.Delay(250);

        //            // Status endpoint
        //            string statusUrl =
        //$"https://api.pdfmyhtml.com/v1/jobs/{jobId}";

        //            var statusResponse =
        //                await _httpClient.GetAsync(statusUrl);

        //            statusResponse.EnsureSuccessStatusCode();

        //            string statusJson =
        //                await statusResponse.Content.ReadAsStringAsync();

        //            using JsonDocument statusDoc =
        //                JsonDocument.Parse(statusJson);

        //            string status =
        //                statusDoc.RootElement
        //                    .GetProperty("status")
        //                    .GetString()!;

        //            // PDF generation finished
        //            if (status == "COMPLETED")
        //            {
        //                // Retrieve PDF download URL
        //                string pdfUrl =
        //                    statusDoc.RootElement
        //                        .GetProperty("download_url")
        //                        .GetString()!;

        //                // Download PDF bytes
        //                return await _httpClient
        //                    .GetByteArrayAsync(pdfUrl);
        //            }


        //            // API failed
        //            if (status == "FAILED")
        //            {
        //                throw new Exception(
        //                    "PDF generation failed.");
        //            }
        //        }
        //    }

        public async Task<byte[]> GeneratePdfAsync(string html)
        {
            var sw = Stopwatch.StartNew();

            Console.WriteLine(
                $"PDF Request HTML Size: {html.Length:N0} characters");

            string apiKey =
                _configuration["PdfMyHtml:ApiKey"]!;

            string submitUrl =
                "https://api.pdfmyhtml.com/v1/html-to-pdf";

            var payload = new
            {
                html = html
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "X-API-Key",
                apiKey);

            Console.WriteLine("Submitting PDF job...");

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

            string jobId =
                submitDoc.RootElement
                    .GetProperty("job_id")
                    .GetString()!;

            Console.WriteLine(
                $"Job ID: {jobId}");

            int pollCount = 0;

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

                    byte[] pdfBytes =
                        await _httpClient.GetByteArrayAsync(pdfUrl);

                    downloadSw.Stop();

                    Console.WriteLine(
                        $"PDF download completed in {downloadSw.ElapsedMilliseconds}ms");

                    Console.WriteLine(
                        $"Total GeneratePdfAsync time: {sw.ElapsedMilliseconds}ms");

                    return pdfBytes;
                }

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