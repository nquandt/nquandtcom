namespace Components.Resume.Sections;

public interface Section : IViewModel
{
    string Title { get; }
}

public abstract class Section<T> : ViewModel<T>, Section where T : class
{
    public abstract string Title { get; }
}