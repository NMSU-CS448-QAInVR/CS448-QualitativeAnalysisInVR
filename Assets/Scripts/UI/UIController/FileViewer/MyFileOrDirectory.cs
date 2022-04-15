public class MyFileOrDirectory {
    public string name;
    public string path;

    public string parentPath;

    private bool isDirectory;

    public MyFileOrDirectory(string _name, string _path, bool _isDirectory, string _parentPath="") {
        name = _name;
        path = _path;
        isDirectory = _isDirectory;
        parentPath = _parentPath;
    } // end MyFileOrDirectory

    public bool GetIsDirectory() {
        return isDirectory;
    } // end GetIsDirectory
} // end MyFileOrDirectory