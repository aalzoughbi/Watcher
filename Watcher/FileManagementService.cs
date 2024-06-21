using System;
using System.IO;

public class FileManagementService
{
    private string _processedFolder;

    public FileManagementService(string processedFolder)
    {
        if (string.IsNullOrWhiteSpace(processedFolder))
        {
            throw new ArgumentNullException(nameof(processedFolder), "Processed folder path cannot be null or empty.");
        }

        _processedFolder = processedFolder;
        Directory.CreateDirectory(_processedFolder);
    }

    public void MoveFile(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        string destPath = Path.Combine(_processedFolder, fileName);
        File.Move(filePath, destPath);
    }
}
