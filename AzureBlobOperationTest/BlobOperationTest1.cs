using AzureBlobOperation;
using Xunit;

namespace AzureBlobOperationTest
{
    public class BlobOperationTest1
    {
        [Theory]
        [InlineData("001", "folder-001", "subfolder-001", "filename-001")]
        [InlineData("002", "folder-002", "subfolder-002", "filename-002")]
        [InlineData("003", "folder-003", "subfolder-003", "filename-003")]
        [InlineData("004", "folder-004", "subfolder-004", "filename-004")]
        [InlineData("005", "folder-005", "subfolder-005", "filename-005")]
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
