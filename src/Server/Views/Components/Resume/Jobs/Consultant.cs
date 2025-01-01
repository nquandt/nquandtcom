
namespace Components.Resume.Jobs;

public class Consultant : JobModel
{
    public override string Title => "Software Architect [Consultant]";
    public override string[] Bullets => [
        "Migrate Mulesoft Flows to .NET Web API's incrementally by endpoint.",
        "Build .NET API wrappers around legacy soap services to utilize REST + JSON instead of XML.",
        "Create a shared package for IOptions pattern validation in ASP.NET applications.",
        "Utilized .NET 6/8, ASP.NET, XUnit for testing, NSubstitute for mocking, FluentValidation for model validation and configuration validation.",
        "Drive discussion and direction around modern .NET architecture and practices.",
        "Participate in standard Agile practice, such as planning, review, retrospectives, PI planning.",
        "Utilize github for repos, including workflows for deployment.",
    ];
}