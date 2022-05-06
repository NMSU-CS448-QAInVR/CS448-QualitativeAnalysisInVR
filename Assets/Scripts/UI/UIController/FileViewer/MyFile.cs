/*
    A class to replace the FileInfo class of .NET. 
    See MyFileOrDirectory for more info.
*/
public class MyFile : MyFileOrDirectory {
    public MyFile(string n, string p) : base(n, p, false) {
        
    }
}