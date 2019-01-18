
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

public class _ErrorClass
{
    private string _fileName;

    public _ErrorClass(string filename)
    {
        this._fileName = filename;
    }

    public void SaveErrorToFile(string error)
    {
        FileStream fs;
        string pathLoginFile = Application.StartupPath + @"\" + this._fileName;
        if (!File.Exists(pathLoginFile))
        {
            fs = new FileStream(pathLoginFile, FileMode.CreateNew, FileAccess.Write);
        }
        else
        {
            fs = new FileStream(pathLoginFile, FileMode.Append, FileAccess.Write);
            fs.Write(Encoding.ASCII.GetBytes("\r\n"), 0, Encoding.ASCII.GetByteCount("\r\n"));
        }
        fs.Write(Encoding.ASCII.GetBytes(error), 0, Encoding.ASCII.GetByteCount(error));
        fs.Close();
    }
}

