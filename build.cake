#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

// Cake Addins
#addin nuget:?package=Cake.FileHelpers&version=2.0.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var VERSION = "5.5.3";
var NUGET_SUFIX = ".0-steerpath";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var solutionPath = "./mapbox-android.sln";
var artifacts = new [] {
    new Artifact {
        AssemblyInfoPath = "./Naxam.Mapbox.Droid/Properties/AssemblyInfo.cs",
        NuspecPath = "./mapbox.nuspec",
        DownloadUrl = "http://steerpath.bintray.com/steerpath/com/steerpath/MapboxGLAndroidSDK/android-5.5.3-steerpath-ndk17-1-1-g21827609e/MapboxGLAndroidSDK-android-5.5.3-steerpath-ndk17-1-1-g21827609e.aar",
        JarPath = "./Naxam.Mapbox.Droid/Jars/mapbox-android-sdk.aar"
    }
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Downloads")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        var downloadUrl = string.Format(artifact.DownloadUrl, VERSION);
        var jarPath = string.Format(artifact.JarPath, VERSION);

        DownloadFile(downloadUrl, jarPath);
    }
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./packages");

    var nugetPackages = GetFiles("./*.nupkg");

    foreach (var package in nugetPackages)
    {
        DeleteFile(package);
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Downloads")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionPath, settings => settings.SetConfiguration(configuration));
});

Task("UpdateVersion")
    .Does(() => 
{
    foreach(var artifact in artifacts) {
        ReplaceRegexInFiles(artifact.AssemblyInfoPath, "\\[assembly\\: AssemblyVersion([^\\]]+)\\]", string.Format("[assembly: AssemblyVersion(\"{0}\")]", VERSION));
    }
});

Task("Pack")
    // .IsDependentOn("UpdateVersion")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        NuGetPack(artifact.NuspecPath, new NuGetPackSettings {
            Version = VERSION+NUGET_SUFIX,
            Dependencies = new []{
                new NuSpecDependency {
                    Id = "Xamarin.Android.Support.Annotations",
                    Version = "28.0.0.3"
                },
                new NuSpecDependency {
                    Id = "Xamarin.Android.Support.Fragment",
                    Version = "28.0.0.3"
                },
                new NuSpecDependency {
                    Id = "Naxam.Jakewharton.Timber",
                    Version = "4.5.1"
                },
                new NuSpecDependency {
                    Id = "Naxam.Mapbox.MapboxJavaGeojson",
                    Version = "2.2.10"
                },
                new NuSpecDependency {
                    Id = "Naxam.Mapbox.Services.Android.Telemetry",
                    Version = "2.2.10"
                },
                new NuSpecDependency {
                    Id = "Naxam.Mapbox.Services.Android.Telemetry",
                    Version = "2.2.10"
                },
                new NuSpecDependency {
                    Id = "Naxam.Mapzen.Lost.Droid",
                    Version = "3.0.4"
                },
                new NuSpecDependency {
                    Id = "Xamarin.GooglePlayServices.Location",
                    Version = "60.1142.1"
                },
                new NuSpecDependency {
                    Id = "Xamarin.Android.Support.v7.AppCompat",
                    Version = "28.0.0.3"
                },
                new NuSpecDependency {
                    Id = "Xamarin.Android.Support.Core.UI",
                    Version = "28.0.0.3"
                },
                new NuSpecDependency {
                    Id = "Xamarin.Android.Support.Media.Compat",
                    Version = "28.0.0.3"
                }
            },
            ReleaseNotes = new [] {
                $"Mapbox SDK for Android v{VERSION}"
            }
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

class Artifact {
    public string AssemblyInfoPath { get; set; }

    public string SolutionPath { get; set; }

    public string DownloadUrl  { get; set; }

    public string JarPath { get; set; }

    public string NuspecPath { get; set; }
}