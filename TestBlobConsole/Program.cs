using AzureBlobOperation;
using System;

namespace TestBlobConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        private void UploadFile(string Folder, string empID, string FileName)
        {
            var blobOps = new BlobOperationsLibrary();
            BlobOperationsLibrary.FolderName = Folder; // Same we are using to upload files in Temp (Instead of Folder we are Passing Temp file name)
            BlobOperationsLibrary.SubFolderName = "";
            BlobOperationsLibrary.BlobFileName = empID + "_" + FileName;
            BlobOperationsLibrary.EncryptBlob = false;
            BlobOperationsLibrary.isStream = true;
            BlobOperationsLibrary.UploadFiletoBlobStorageMI();
        }

    }
}
