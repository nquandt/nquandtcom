

using Components.Resume.Jobs;

namespace Components.Resume.Sections;

public class Experience : Section<Experience>
{
    public override string Title => "Experience";    

    public JobModel[] Jobs => [
        new MilwaukeeTool(),
        new Consultant()
    ];
}