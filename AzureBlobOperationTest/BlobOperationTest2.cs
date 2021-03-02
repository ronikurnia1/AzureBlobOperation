using AzureBlobOperation;
using Xunit;

namespace AzureBlobOperationTest
{
    public class BlobOperationTest2
    {
        [Theory]
        [InlineData("006", "folder-006", "subfolder-006", "filename-006")]
        [InlineData("007", "folder-007", "subfolder-007", "filename-007")]
        [InlineData("008", "folder-008", "subfolder-008", "filename-008")]
        [InlineData("009", "folder-009", "subfolder-009", "filename-009")]
        [InlineData("010", "folder-010", "subfolder-010", "filename-010")]
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
