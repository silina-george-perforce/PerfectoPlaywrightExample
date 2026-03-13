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
    private const string SECURITY_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICI4YmI4YmZmZS1kMzBjLTQ2MjctYmMxMS0zNTYyMmY1ZDkyMGYifQ.eyJpYXQiOjE3NzMyNzc5NjQsImp0aSI6IjhlMDg4MzY3LTIzNDMtNDc5NC1hMmQyLThkODkyNzk4ZGJlYSIsImlzcyI6Imh0dHBzOi8vYXV0aC5wZXJmZWN0b21vYmlsZS5jb20vYXV0aC9yZWFsbXMvZGVtby1wZXJmZWN0b21vYmlsZS1jb20iLCJhdWQiOiJodHRwczovL2F1dGgucGVyZmVjdG9tb2JpbGUuY29tL2F1dGgvcmVhbG1zL2RlbW8tcGVyZmVjdG9tb2JpbGUtY29tIiwic3ViIjoiZmZkODk5NzQtMDI0NS00MTcxLWEwN2MtMzE5NjViNzg3NzJhIiwidHlwIjoiT2ZmbGluZSIsImF6cCI6Im9mZmxpbmUtdG9rZW4tZ2VuZXJhdG9yIiwibm9uY2UiOiJlMjdiN2U2My1kOTA5LTQzOTMtOGFlMS1lZDVlMGU2NTJkZWMiLCJzZXNzaW9uX3N0YXRlIjoiZGM3YWZkNjYtMmRkNC00YTc4LTg1YTctNTk3ZWY1MTU2MGFlIiwic2NvcGUiOiJvcGVuaWQgb2ZmbGluZV9hY2Nlc3MiLCJzaWQiOiJkYzdhZmQ2Ni0yZGQ0LTRhNzgtODVhNy01OTdlZjUxNTYwYWUifQ.tQuBNYxq3dnSqGh8EzNp1qHFnTffc_4U2828N4jqb9w";

    [Test]
    public async Task RbcPageTitleTest()
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
