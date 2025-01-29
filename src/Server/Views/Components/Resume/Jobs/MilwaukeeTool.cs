
namespace Components.Resume.Jobs;

public class MilwaukeeTool : JobModel
{
    // public override string DateRange => "August 2021 - Present";
    public override string Title => "Software Architect";

    public override string Company => "Milwaukee Tool";

    public override string Url => "https://www.milwaukeetool.com";
    public override string[] Bullets => [
        "Discern architectural direction and implementation details for www.milwaukeetool.com and corresponding digital products, with a focus on engaging users through cohesive and branded experiences.",
        "Deliver business-critical features by aligning technical solutions with organizational goals, ensuring scalability and maintainability.",
        "Drive mentalities away from monolithic approaches leaning into the idealogy that digital products can be isolated plugins, effectively vertical slice architecture at a product level.",
        "Scope and initiate an incremental migration to React-SSR as a front end rendering host from .NET Framework 4.8 MVC, enabling new hiring opportunies, improved development experiences, and an expanded tooling ecosystem for user focused products. This project spanned 18 months, including 40,000+ pages, and nearly 500 unique views. Migration allowed the reduction to 74 views and improved brand cohesion across digital assets through the use of a shared design component library.",
        "Pilot a shared React design component library for use in 6 different products initially, following an atomic design philosophy.",
        "Restructure monolithic application services into microservices specific to individual business verticals, allowing for faster feedback loops on development cycles. The monolith averaged 75 minutes to build/deploy, while each individual service ranges 1-8 minutes.",
        "Maintain a consistent development experience aimed at scaling products consistently and efficiently through the creation of an interal application framework. The framework streamlines aspnetcore application setup for Key Vault and App Configuration setup, including dynamic configuration updates via Event Grid eventing.",
        "Standardize application deployment and hosting through the use of Azure Devops Pipelines, Azure Container Registry, Azure Kubernetes Service and Azure Container Apps.",
        "Utilize YARP as a reverse proxy to route traffic between applications, incorporating cookie-based authentication middleware with Redis sessions to support a scaled Backend-for-Frontend (BFF) architecture. Doubles as an inverse application registration layer allowing A/B testing or full replacement of digital products without interuptions, consistent with a composable strategy.",
        "Monitor Azure resources using New Relic via an OTLP (OpenTelemetry) endpoint, enabling comprehensive observability and streamlined performance monitoring.",
        "Implement Terraform for Infrastructure as Code (IaC), utilizing Azure Storage Accounts as the backend provider",
        "Approach products from an Agile perspective, constantly iterating based on metrics on user engagement. Speed, data consistency, and brand alignment being key focus points.",
        "Deliver monthly lunch-and-learn sessions for the broader web development team, presenting on topics such as modern development practices, emerging technologies, and coding principles to foster professional growth and innovation."
    ];
}