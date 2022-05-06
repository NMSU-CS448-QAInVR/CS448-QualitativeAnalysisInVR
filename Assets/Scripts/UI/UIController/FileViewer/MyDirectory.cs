using System.Collections.Generic;
using System.IO;

public class MyDirectory : MyFileOrDirectory {
    public List<MyFile> files;
    public List<MyDirectory> directories;

    public MyDirectory(DirectoryInfo root) : base(root.Name, root.FullName, true, root.Parent.FullName) {
        FileInfo[] myFiles = root.GetFiles();
        files = new List<MyFile>(myFiles.Length);
        for (int i = 0; i < myFiles.Length; ++i) {
            FileInfo file = myFiles[i];
            files.Add(new MyFile(file.Name, file.FullName));
        } // end for i

        DirectoryInfo[] myDirectories = root.GetDirectories();
        directories = new List<MyDirectory>(myDirectories.Length);
        for (int i = 0; i < myDirectories.Length; ++i) {
            DirectoryInfo myDir = myDirectories[i];
            directories.Add(new MyDirectory(myDir));
        }
    } // end root

    public MyDirectory(string path) : this(FileManager.GetFilesAndDirsAt(path, false)){
    } // end root
    
}