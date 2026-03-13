# C# Playwright Sample

This project is a small **.NET 10 test project** that verifies connectivity to Perfecto and runs a simple Playwright test with reporting.

It uses:

* Microsoft Playwright
* NUnit
* .NET

---

# Quick Start

If you already have the required tools installed, you can run the test with:

```bash
dotnet restore
dotnet build PerfectoPlaywrightTests.csproj
dotnet test PerfectoPlaywrightTests.csproj
```

Before running, update the following values in `perfectoPlaywrightTests.cs`:

* `CLOUD_NAME`
* `SECURITY_TOKEN`

---

# Prerequisites

If you are new to Playwright or .NET, install the following tools first.

## 1. Install .NET SDK

Install **.NET SDK 10.0 or newer**.

Download from:
[https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

Verify installation:

```bash
dotnet --version
```

Example output:

```
10.x.x
```

---

## 2. Install Node.js

Microsoft Playwright requires Node.js to download browser binaries.

Download the **LTS version** from:
[https://nodejs.org](https://nodejs.org)

Verify installation:

```bash
node --version
npm --version
```

Example output:

```
v20.x.x
10.x.x
```

---

## 3. Install Playwright Browsers

Playwright must download browser binaries before tests can run.

Run this command once:

```bash
npx playwright install
```

This installs:

* Chromium
* Firefox
* WebKit

---

# Project Structure

```
PerfectoPlaywrightTests
│
├── PerfectoPlaywrightTests.csproj
├── perfectoPlaywrightTests.cs
```

**Files**

* `PerfectoPlaywrightTests.csproj` — project configuration
* `perfectoPlaywrightTests.cs` — Playwright test implementation

Dependencies include:

* `Microsoft.Playwright` 1.53.0
* `NUnit` 4.2.2
* `Microsoft.NET.Test.Sdk`

---

# Running the Test

Navigate to the project folder:

```bash
cd PerfectoPlaywrightTests
```

Restore dependencies:

```bash
dotnet restore
```

Build the project:

```bash
dotnet build PerfectoPlaywrightTests.csproj
```

Run the test:

```bash
dotnet test PerfectoPlaywrightTests.csproj
```

---

# Expected Output

When the test runs successfully, you should see output similar to:

* Starting Playwright session...
* Navigating to Google website...
* SUCCESS: Page title validated using Playwright Expect.

---

# Notes

* Capabilities (including the `SECURITY_TOKEN`) are currently **hardcoded** in `perfectoPlaywrightTests.cs`.
* For production usage, move `SECURITY_TOKEN` and `CLOUD_NAME` to **environment variables**.
