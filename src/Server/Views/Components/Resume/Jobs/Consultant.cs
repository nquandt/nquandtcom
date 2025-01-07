
namespace Components.Resume.Jobs;

public class Consultant : JobModel
{
    public override string DateRange => "Various 6-12 month contracts";
    public override string Title => "Software Consultant";
    public override string[] Bullets => [
"Led an incremental migration of Mulesoft flows to .NET Web APIs, implementing a phased approach by endpoint to ensure seamless integration and minimal disruption to business operations.",
"Designed and developed .NET API wrappers around legacy SOAP services, modernizing interfaces by transitioning to RESTful standards with JSON payloads, improving usability and compatibility.",
"Created and deployed a shared .NET package to enforce validation using the IOptions pattern, enhancing configuration consistency and reducing runtime errors in ASP.NET applications.",
"Leveraged XUnit for robust testing, incorporating NSubstitute for mocking and FluentValidation for comprehensive model and configuration validation.",
"Facilitated discussions and provided strategic guidance on modern .NET architecture and development practices, aligning teams with industry standards and best practices.",
"Introduced the use of Azure Key Vault in ASP.NET applications for secure management of application secrets and certificates.",
"Collaborated with development teams to deploy containerized applications using Azure Kubernetes Service, optimizing scalability and operational efficiency.",
"Implemented Github Actions in various repositories for NuGet publishing and application containerization."
    ];
}