var target = Argument("target", "Default");

using System;
using System.Diagnostics;

// Variables
var configuration = "Release";
var fullFrameworkTarget = "net461";
var netCoreTarget20 = "netcoreapp2.0";
var netCoreTarget21 = "netcoreapp2.1";

var artifactOutput = "./artifacts";

string projectPath = "src/Simplecast.Client";
string projectProj = $"Simplecast.Client.csproj";

Task("Default")
    .IsDependentOn("Test");

Task("Compile")
    .Description("Builds all the projects in the solution")
    .Does(() =>
    {
        StartProcess("dotnet", new ProcessSettings {
            Arguments = "--info"
        });

        DotNetCoreBuildSettings settings = new DotNetCoreBuildSettings();
        settings.NoRestore = true;
        settings.Configuration = configuration;

        string slnPath = "./src/Simplecast.sln";

        Information($"Restoring projects");
        DotNetCoreRestore(slnPath);
        Information($"Building projects");
        DotNetCoreBuild(slnPath, settings);
    });

Task("Test")
    .Description("Run Tests")
    .IsDependentOn("Compile")
    .Does(() =>
    {
        string appveyor = EnvironmentVariable("APPVEYOR");
        bool isRunningOnUnix = IsRunningOnUnix();
        string testProjectPath = "./src/Tests/Simplecast.Client.Tests/Simplecast.Client.Tests.csproj";

        DotNetCoreTestSettings settings = new DotNetCoreTestSettings();
        settings.Configuration = configuration;

        if(!string.IsNullOrEmpty(appveyor) && appveyor == "True")
        {
            settings.ArgumentCustomization  = args => args.Append(" --test-adapter-path:. --logger:Appveyor");
        }

        Information($"Running {netCoreTarget20.ToUpper()} Tests");
        settings.Framework = netCoreTarget20;
        DotNetCoreTest(testProjectPath, settings);

        Information($"Running {netCoreTarget21.ToUpper()} Tests");
        settings.Framework = netCoreTarget21;
        DotNetCoreTest(testProjectPath, settings);

        Information($"Running {fullFrameworkTarget.ToUpper()} Tests");
        if(!isRunningOnUnix) // Windows
        {
            settings.Framework = fullFrameworkTarget;
            DotNetCoreTest(testProjectPath, settings);
        }
        else // Linux
        {
            NuGetInstallSettings nugetInstallSettings = new NuGetInstallSettings();
            nugetInstallSettings.Version = "2.4.0";
            nugetInstallSettings.OutputDirectory = "testrunner";            
            nugetInstallSettings.WorkingDirectory = ".";

            NuGetInstall("xunit.runner.console", nugetInstallSettings);

            StartProcess("mono", new ProcessSettings {
                Arguments = $"./testrunner/xunit.runner.console.2.4.0/tools/{fullFrameworkTarget}/xunit.console.exe ./src/Tests/Simplecast.Client.Tests/bin/Release/{fullFrameworkTarget}/Simplecast.Client.Tests.dll"
            });
        }
    });

Task("Nuget-Pack")
    .Description("Publish to nuget")
    .Does(() =>
    {
        var settings = new DotNetCorePackSettings();
        settings.Configuration = configuration;

        settings.OutputDirectory = artifactOutput;
        settings.WorkingDirectory = projectPath;
        DotNetCorePack(projectProj, settings);
    });

RunTarget(target);