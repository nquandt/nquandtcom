
namespace Components.Resume;

public abstract class JobModel : ViewModel<JobModel>
{
    public abstract string Title { get; }
    public virtual string Company => "";

    public virtual string DateRange => "";

    public virtual string Url => "";
    public abstract string[] Bullets { get; }

    public virtual bool Print => false;
    // public abstract string ContentView { get; }
}