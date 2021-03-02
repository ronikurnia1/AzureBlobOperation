using AzureBlobOperation;
using Xunit;

namespace AzureBlobOperationTest
{
    public class BlobOperationTest3
    {
        [Theory]
        [InlineData("011", "folder-011", "subfolder-011", "filename-011")]
        [InlineData("012", "folder-012", "subfolder-012", "filename-012")]
        [InlineData("013", "folder-013", "subfolder-013", "filename-013")]
        [InlineData("014", "folder-014", "subfolder-014", "filename-014")]
        [InlineData("015", "folder-015", "subfolder-015", "filename-015")]
        public void ConcurrentTest(
            string empId, string folderName, string subFolderName, string fileName)
        {
            //// Arrange for original version
            //BlobOperationsLibrary.FolderName = folderName;
            //BlobOperationsLibrary.SubFolderName = subFolderName;
            //BlobOperationsLibrary.BlobFileName = fileName;

            // Arrange for Version 1
            var blobOps = new BlobOperationsLibraryV1();
            blobOps.FolderName = folderName;
            blobOps.SubFolderName = subFolderName;
            blobOps.BlobFileName = fileName;

            var expectedFolderName = $"folder-{empId}";
            var expectedsubFolderName = $"subfolder-{empId}";
            var expectedfileName = $"filename-{empId}";

            //// Assert for original version
            //Assert.Equal(expectedFolderName, BlobOperationsLibrary.FolderName);
            //Assert.Equal(expectedsubFolderName, BlobOperationsLibrary.SubFolderName);
            //Assert.Equal(expectedfileName, BlobOperationsLibrary.BlobFileName);

            // Assert for Version 1
            Assert.Equal(expectedFolderName, blobOps.FolderName);
            Assert.Equal(expectedsubFolderName, blobOps.SubFolderName);
            Assert.Equal(expectedfileName, blobOps.BlobFileName);
        }
    }
}
