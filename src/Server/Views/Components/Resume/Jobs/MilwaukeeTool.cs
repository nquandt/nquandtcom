
namespace Components.Resume.Jobs;

public class MilwaukeeTool : JobModel
{
    public override string DateRange => "August 2021 - Present";
    public override string Title => "Software Architect";

    public override string Company => "Milwaukee Tool";

    public override string Url => "https://www.milwaukeetool.com";
    public override string[] Bullets => [
        "Maintain a multi-site [Sitecore 10 XM](https://www.sitecore.com/products/experience-manager?utm_websource=products) topology in kubernetes ([AKS](https://docs.microsoft.com/en-us/azure/aks/intro-kubernetes)), with full server side rendering via. [Razor](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-6.0).",
        "Scope and initiate an incremental migration from [Razor](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-6.0) pages to [NextJS](https://nextjs.org/) as our primary application front end. This migration aims to adopt a headless philosophy by seperating our CMS from our rendering host.",
        "Incrementally migrate backend services to .NET 6/7/8 from .NET Framework 4.8. Utilize path based routing to orchestrate between boundaries.",
        "Host applications following multi-region practices. Basic horizontal scalability, statelessness, eventing. Utilize a combinination of [AKS](https://docs.microsoft.com/en-us/azure/aks/intro-kubernetes) and [Azure Container Apps](https://azure.microsoft.com/en-us/products/container-apps) with event grid events triggering key vault or configuration changes/reloads. [Traffic Manager](https://learn.microsoft.com/en-us/azure/traffic-manager/traffic-manager-overview) -> [Application Gateway](https://learn.microsoft.com/en-us/azure/application-gateway/overview) -> Container Environments.",
        "Restructuring of our content system to align closer to our coding mental models. Templates -> Models, Content -> Class Instances etc, allowing for faster development cycles.",
        "Build and maintain a shared repository to standardize .NET middlewares, logging, options patterns, health checks and more in our multiservice topology.",
        "Implement [Terraform](https://www.terraform.io/) as our IaC tool, using Azure Storage Accounts as a backend provider to keep tooling in our Azure cloud.",
        "Utilize [YARP](https://github.com/microsoft/reverse-proxy) as a reverse proxy to route traffic between multiple applications. Included cookie based authentication middleware with redis sessions (effectively a BFF layer).",
        "Plan and execute the back-end half of development for a [product catalog utility](https://www.milwaukeetool.com/innovations/solutions), and [product comparison tool](https://www.milwaukeetool.com/products/power-tools/drilling/compare), both powered by [Svelte](https://svelte.dev/) and [Solr](https://solr.apache.org/).",
        "Create a new solution to decouple a small team from the primary development team to allow rapid development utilizing a microservice architecture. Including full devops pipelines and procedures.",
        "Implement a new development workflow for a small offshoot web project, utilizing [Gulp](https://gulpjs.com/) and [Rollup](https://rollupjs.org/).",
        "Integrate our .NET applications with [Azure Key Vault](https://docs.microsoft.com/en-us/azure/key-vault/general/basic-concepts) for key, secret, and certificate maintainance and security.",
        "Strategize and execute a site wide migration from Brightcove to Vimeo as a video hosting platform, including library migration.",
        "Implement and standardize health checks in our microservice applications to prevent noticed cold starts and downtime on kubernetes.",
        "Give monthly lunch'n'learn talks to the greater web dev team on various topics including: general development practice, new technologies, coding principles, etc.",
    ];
}