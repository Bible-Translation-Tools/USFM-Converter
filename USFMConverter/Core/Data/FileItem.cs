namespace USFMConverter.Core.Data;

public class FileItem: IProjectItem
{
    public string Label => Path;
    public string Path { get; set; }

    public FileItem(string path)
    {
        Path = path;
    }
}