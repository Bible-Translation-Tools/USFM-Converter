namespace USFMConverter.Core.Data;

public class WriterProjectItem: IProjectItem
{
    public string Label => Path;
    public string Path { get; set; }
}