# ChildFund CMS and Commerce Site

This repository contains the ChildFund website built on Optimizely CMS and Commerce.

---

## Prerequisites

[SQL Server 2019 or greater](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) is required as database engine.

[Visual Studio 2022 or greater](https://learn.microsoft.com/en-us/visualstudio/releases/2022/release-notes) required to open the solution.

[Microsoft SQL Server Management Studio 2018 or greater](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) required to access the database.

[.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to build and run the application.

[.NET 8 - Hosting Bundle](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.0-windows-hosting-bundle-installer) hosting bundle is required for IIS hosting.

---

## Installation

1. Clone this repository to your local development environment
2. Open the `ChildFund.sln` solution file in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Update the database connection string in `appsettings.json` or create environment-specific configuration files

---

## How to Run

### Windows Development

Prerequisites:
- .NET SDK 8+
- SQL Server 2016 Express LocalDB (or later)

```bash
# Navigate to the web project directory
cd src/ChildFund.Web

# Run the application
dotnet run
```

The application will be available at `https://localhost:5000/`

The application can also be run directly from Visual Studio by setting `ChildFund.Web` as the startup project and pressing F5.

## After Installation

Please follow these steps after initial setup:

1. **Rebuild the solution** - Do not just build the solution, we need a rebuild. If there is an error the first time, try a second time and it should work.
2. **Change default server** - In the drop down next to the full green arrow, choose the solution name server instead of the default IIS Express.
3. **Run the app** - Run the application clicking on the full green arrow and then wait until the site loads properly.
4. **Go to the editor interface** - Using the admin credentials go to `/episerver/cms` to access the editor interface
5. **Configure roles** - If your admin user cannot make changes to the content of the site, go to the admin section, access rights and add the role "Web admin" to the root folder with full permissions and apply the change to all nodes below.
6. **UI glitches** - If for some reason you go to the edit interface and you cannot see the settings tab, or any block or image inside the folders, please stop the application and run it again and it should work.

---

## Configuration

Most of the configuration has been moved to options classes. The options classes can be configured through code or the appsettings.json configuration file. For option classes to be automatically configured from `appsettings.json`, please use the `EPiServer.ServiceLocation.OptionsAttribute`. There is a configuration section which maps to the leaf node in the JSON.

To utilize legacy configuration sections you can install the `EPiServer.Cms.AspNetCore.Migration` package. This is available to ease migration, however we encourage updating to use options or `appsettings.json` if possible.

---

## Startup Extensibility

### Program.cs
EPiServer will by default use the built-in Dependency Injection framework (DI) in .NET. To connect the DI framework with EPiServer you need to call extension method `IHostBuilder.ConfigureCmsDefault()` in Program.cs.

## OPTIMIZELY GRAPH

Go to the appsettings.json file and change the Optimizely Graph configurations. You can use the integration environment API keys taken from the PaaS portal.

```
"Optimizely": {
    "ContentGraph": {
      "GatewayAddress": "https://cg.optimizely.com",
      "AppKey": "...",
      "Secret": "...",
      "SingleKey": "...",
      "AllowSendingLog": "true"
    }
  },
```

---

## 

1. Create a local domain name for development
```
local.childfund.org
```
2. Generate Certificate (Administrator PowerShell)
```
New-SelfSignedCertificate -Subject childfund-local.com -DnsName local.childfund.org -CertStoreLocation Cert:\LocalMachine\My -NotAfter (Get-Date).AddYears(10)
```
3. Trust the Certificate
   - Open MMC, add the Certificates snap-in for Computer Account > Local Machine
   - **Copy** the cert "local.childfund.org" from "Personal"
   - **PASTE** the cert into "Trusted Root Certification Authorities" (The certificate should exist in both locations)

---

## Setup Location Domains in Host File

1. Using an Administrator window of an editor like Notepad++, open the following file:
```
C:\Windows\System32\drivers\etc\hosts
```
2. Add the following lines to the file, under the comments:
```
127.0.0.1 local.childfund.org
```
3. Save the file

---

## Setup IIS

1. Add a new website in IIS
   1. Site Name: ChildFund
   2. Application Pool: ChildFund (It will create a new App Pool automatically. Do not use an existing one)
   3. Physical Path: C:\Projects\ChildFund\Site
   4. Binding
      1. Type: Https
      2. IP Address: All Unassigned
      3. Port: 443
      4. Host Name: local.childfund.org
      5. Require Server Name Indication: checked
         - (Leave the rest of the checkboxes unchecked)
      6. SSL Certificate: local.childfund.org
2. Create a publish profile in Visual Studio to publis to the IIS folder
3. Publish the application to the IIS folder
4. Navigate to https://local.childfund.org/ . Ensure the connection/certificate is valid over https://
5. Make sure to update the database connection string in the appsettings.json or appsettings.Development.json file in the published folder
6. Make sure the web.config file has a reference to the correct appsettings transform (development)

---

## Publish Profile

The publish profile will be in the .gitignore file, so this will have to be maintained on a local environment basis. This will be what the build/deploy script triggers to publish your local environment.

Make sure to change the local URL from below. Default location is: C:\Projects\ChildFund\Site

(Line 13 is where the local URL is)

Filename: IISProfile.pubxml

```
<?xml version="1.0" encoding="utf-8"?>
<!--
https://go.microsoft.com/fwlink/?LinkID=208121.
-->
<Project>
  <PropertyGroup>
    <WebPublishMethod>MSDeploy</WebPublishMethod>
    <LaunchSiteAfterPublish>true</LaunchSiteAfterPublish>
    <LastUsedBuildConfiguration>Development</LastUsedBuildConfiguration>
    <LastUsedPlatform>Any CPU</LastUsedPlatform>
    <SiteUrlToLaunchAfterPublish>local.childfund.org</SiteUrlToLaunchAfterPublish>
    <ExcludeApp_Data>false</ExcludeApp_Data>
    <ProjectGuid>a61c7309-8bf6-4be5-958d-056783a9e560</ProjectGuid>
    <SelfContained>false</SelfContained>
    <MSDeployServiceURL>localhost</MSDeployServiceURL>
    <DeployIisAppPath>local.childfund.org</DeployIisAppPath>
    <RemoteSitePhysicalPath />
    <SkipExtraFilesOnServer>true</SkipExtraFilesOnServer>
    <MSDeployPublishMethod>InProc</MSDeployPublishMethod>
    <EnableMSDeployBackup>false</EnableMSDeployBackup>
    <EnableMsDeployAppOffline>true</EnableMsDeployAppOffline>
    <UserName />
    <_SavePWD>false</_SavePWD>
    <_TargetId>IISWebDeploy</_TargetId>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
</Project>
```

---

## Update appsettings

appsettings.Local.json is versioned with sample settings. Use appsettings.Development.json as a base to create your own.

---
---
---

# Documentation

For documentation on Optimizely CMS and Commerce, see the [Optimizely World documentation](https://world.optimizely.com/documentation/developer-guides/).
