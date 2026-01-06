using Aspire.Hosting.Maui;
using Projects;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<MauiProjectResource> mauiApp = builder.AddMauiProject("tms", "../TMS.App/TMS.App.csproj");

mauiApp.AddWindowsDevice();

DistributedApplication distributedApplication = builder.Build();

await distributedApplication.RunAsync();