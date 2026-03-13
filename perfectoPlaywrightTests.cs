using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using static Microsoft.Playwright.Assertions;

[TestFixture]
public class PerfectoPlaywrightTests
{
    private const string CLOUD_NAME = "demo";
    private const string SECURITY_TOKEN = "SECURITY_TOKEN";

    [Test]
    public async Task TitleTest()
    {
        var capabilities = new Dictionary<string, object>
        {
            ["securityToken"] = SECURITY_TOKEN,
            ["browserName"] = "Chrome",
            ["browserVersion"] = "latest",
            ["platformName"] = "Windows",
            ["platformVersion"] = "10",
            ["report.jobName"] = "Perfecto C# Playwright Test",
            ["report.jobNumber"] = "1",
            ["report.jobBranch"] = "1.1"
        };

        var encodedCaps = UrlEncoder.Default.Encode(JsonSerializer.Serialize(capabilities));
        var wssEndpoint = $"wss://{CLOUD_NAME}.perfectomobile.com/websocket?{encodedCaps}";

        Console.WriteLine("Starting Playwright session...");

        using var playwright = await Playwright.CreateAsync();
        Console.WriteLine($"Connecting to: {wssEndpoint}");
        var browser = await playwright.Chromium.ConnectAsync(wssEndpoint);
        IPage? page = null;
        var testPassed = false;
        var failureDescription = "My failure message";

        try
        {
            page = await browser.NewPageAsync();
            page.SetDefaultTimeout(60000);

            var paramsTestStart = new Dictionary<string, object>
            {
                ["name"] = "Sample Perfecto C# Playwright Test",
                ["tags"] = new[] { "Playwright", "tag2", "Dot Net" },
            };

            await page.EvaluateAsync("perfecto:report:testStart", JsonSerializer.Serialize(paramsTestStart));

            Console.WriteLine("Navigating to Google website...");
            await page.GotoAsync("https://www.google.com/");

            Console.WriteLine("Verifying page title...");
            await Expect(page).ToHaveTitleAsync("Google");
            testPassed = true;
            Console.WriteLine("SUCCESS: Page title validated using Playwright Expect.");
        }
        catch (Exception ex)
        {
            failureDescription = ex.Message;
            Console.WriteLine($"ERROR: An exception occurred: {ex.Message}");
            Assert.Fail(ex.Message);
        }
        finally
        {
            if (page is not null)
            {
                var paramsTestStop = new Dictionary<string, object>
                {
                    ["success"] = testPassed,
                    ["failureDescription"] = failureDescription
                };

                await page.EvaluateAsync("perfecto:report:testEnd", JsonSerializer.Serialize(paramsTestStop));
            }

            Console.WriteLine("Closing browser session...");
            await browser.CloseAsync();

        }
    }
}
