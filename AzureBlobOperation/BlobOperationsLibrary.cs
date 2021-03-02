
using Azure.Storage.Blobs;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.RetryPolicies;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AzureBlobOperation
{
    public class BlobOperationsLibrary
    {
        public static Microsoft.Azure.KeyVault.KeyVaultClient kvc = (Microsoft.Azure.KeyVault.KeyVaultClient)null;

        public string BlobConnectionString { get; set; }

        public static string ContainerName { get; set; }

        public string ClientID { get; set; }

        public string ClientSecret { get; set; }

        public static string AccountName { get; set; }

        public static string AccountKey { get; set; }

        public static string FolderName { get; set; } = DateTime.Now.Year.ToString();

        public static string SubFolderName { get; set; } = string.Empty;

        public static Stream BlobFile { get; set; }

        public static bool isStream { get; set; } = false;

        public static bool EncryptBlob { get; set; } = false;

        public static string Filepath { get; set; }

        public static string BlobFileName { get; set; }

        public static BlobRequestOptions BlobOptions { get; set; }

        public static string BlobServiceURI { get; set; } = Convert.ToString(ConfigurationManager.AppSettings["BlobURI"]);

        public static string UploadFiletoBlobStorage()
        {
            try
            {
                BlobRequestOptions optionsWithRetryPolicy = new BlobRequestOptions() { ParallelOperationThreadCount = 1, DisableContentMD5Validation = true, StoreBlobContentMD5 = false };

                CloudBlobContainer containerReference = new CloudStorageAccount(new StorageCredentials(BlobOperationsLibrary.AccountName, BlobOperationsLibrary.AccountKey), true).CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName);
                containerReference.CreateIfNotExists((BlobRequestOptions)null, (OperationContext)null);
                CloudBlockBlob cloudBlockBlob = (CloudBlockBlob)null;
                if (string.IsNullOrEmpty(BlobOperationsLibrary.FolderName) && string.IsNullOrEmpty(BlobOperationsLibrary.SubFolderName))
                    cloudBlockBlob = containerReference.GetBlockBlobReference(BlobOperationsLibrary.BlobFileName);
                if (!string.IsNullOrEmpty(BlobOperationsLibrary.FolderName) && string.IsNullOrEmpty(BlobOperationsLibrary.SubFolderName))
                    cloudBlockBlob = containerReference.GetBlockBlobReference(BlobOperationsLibrary.FolderName + "/" + BlobOperationsLibrary.BlobFileName);
                if (!string.IsNullOrEmpty(BlobOperationsLibrary.FolderName) && !string.IsNullOrEmpty(BlobOperationsLibrary.SubFolderName))
                    cloudBlockBlob = containerReference.GetBlockBlobReference(BlobOperationsLibrary.FolderName + "/" + BlobOperationsLibrary.SubFolderName + "/" + BlobOperationsLibrary.BlobFileName);
                if (BlobOperationsLibrary.isStream)
                {
                    Stream blobFile = BlobOperationsLibrary.BlobFile;
                    if (BlobOperationsLibrary.EncryptBlob)
                        cloudBlockBlob.UploadFromStream(blobFile, blobFile.Length, (AccessCondition)null, BlobOperationsLibrary.BlobOptions, (OperationContext)null);
                    else
                        cloudBlockBlob.UploadFromStream(blobFile, blobFile.Length, (AccessCondition)null, (BlobRequestOptions)optionsWithRetryPolicy, (OperationContext)null);
                }
                else
                {
                    Stream source = (Stream)File.OpenRead(BlobOperationsLibrary.Filepath);
                    if (BlobOperationsLibrary.EncryptBlob)
                        cloudBlockBlob.UploadFromStream(source, source.Length, (AccessCondition)null, BlobOperationsLibrary.BlobOptions, (OperationContext)null);
                    else
                        cloudBlockBlob.UploadFromStream(source, source.Length, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                return "Failed;Reason" + ex.Message;
            }
        }

        public static string UploadFiletoBlobStorageMI()
        {
            try
            {
                CloudStorageAccount result = Task.Run<CloudStorageAccount>(new Func<Task<CloudStorageAccount>>(BlobOperationsLibrary.GetStorageCredential)).GetAwaiter().GetResult();
                AzureServiceTokenProvider serviceTokenProvider = new AzureServiceTokenProvider((string)null, "https://login.microsoftonline.com/");
                CloudBlobContainer containerReference = result.CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName);
                containerReference.CreateIfNotExists((BlobRequestOptions)null, (OperationContext)null);
                CloudBlockBlob cloudBlockBlob = (CloudBlockBlob)null;

                BlobRequestOptions optionsWithRetryPolicy = new BlobRequestOptions() { ParallelOperationThreadCount = 1, DisableContentMD5Validation = true, StoreBlobContentMD5 = false };


                if (string.IsNullOrEmpty(BlobOperationsLibrary.FolderName) && string.IsNullOrEmpty(BlobOperationsLibrary.SubFolderName))
                    cloudBlockBlob = containerReference.GetBlockBlobReference(BlobOperationsLibrary.BlobFileName);
                if (!string.IsNullOrEmpty(BlobOperationsLibrary.FolderName) && string.IsNullOrEmpty(BlobOperationsLibrary.SubFolderName))
                    cloudBlockBlob = containerReference.GetBlockBlobReference(BlobOperationsLibrary.FolderName + "/" + BlobOperationsLibrary.BlobFileName);
                if (!string.IsNullOrEmpty(BlobOperationsLibrary.FolderName) && !string.IsNullOrEmpty(BlobOperationsLibrary.SubFolderName))
                    cloudBlockBlob = containerReference.GetBlockBlobReference(BlobOperationsLibrary.FolderName + "/" + BlobOperationsLibrary.SubFolderName + "/" + BlobOperationsLibrary.BlobFileName);


                if (BlobOperationsLibrary.isStream)
                {
                    Stream blobFile = BlobOperationsLibrary.BlobFile;

                    blobFile.Seek(0, SeekOrigin.Begin);

                    //blobFile.Position = 0L;



                    if (BlobOperationsLibrary.EncryptBlob)
                        cloudBlockBlob.UploadFromStream(blobFile, blobFile.Length, (AccessCondition)null, BlobOperationsLibrary.BlobOptions, (OperationContext)null);
                    else
                        cloudBlockBlob.UploadFromStream(blobFile, blobFile.Length, (AccessCondition)null, (BlobRequestOptions)optionsWithRetryPolicy, (OperationContext)null);
                }
                else
                {
                    Stream source = (Stream)File.OpenRead(BlobOperationsLibrary.Filepath);
                    source.Seek(0, SeekOrigin.Begin);
                    if (BlobOperationsLibrary.EncryptBlob)
                        cloudBlockBlob.UploadFromStream(source, source.Length, (AccessCondition)null, BlobOperationsLibrary.BlobOptions, (OperationContext)null);
                    else
                        cloudBlockBlob.UploadFromStream(source, source.Length, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
                }

                return "Success";



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);

                return "Failed;Reason" + ex.Message;
            }
        }

        private static string createToken(string resourceUri, string keyName, string key)
        {
            string str = Convert.ToString((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 604800);
            string s = HttpUtility.UrlEncode(resourceUri) + "\n" + str;
            string base64String = Convert.ToBase64String(new HMACSHA256(Encoding.UTF8.GetBytes(key)).ComputeHash(Encoding.UTF8.GetBytes(s)));
            return string.Format((IFormatProvider)CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}", (object)HttpUtility.UrlEncode(resourceUri), (object)HttpUtility.UrlEncode(base64String), (object)str, (object)keyName);
        }

        //public MemoryStream DownLoadFilefromBlobStorage(string foldername,string blobfilename)
        //{
        //    try
        //    {

        //        BlobRequestOptions optionsWithUseTransactionalMD5 = new BlobRequestOptions() { UseTransactionalMD5 = true };

        //        CloudBlockBlob blockBlobReference = new CloudStorageAccount(new StorageCredentials(BlobOperationsLibrary.AccountName, BlobOperationsLibrary.AccountKey), true).CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName + "/" + BlobOperationsLibrary.FolderName).GetBlockBlobReference(BlobOperationsLibrary.BlobFileName);
        //        MemoryStream memoryStream = new MemoryStream();
        //        blockBlobReference.DownloadToStream((Stream)memoryStream, (AccessCondition)null, optionsWithUseTransactionalMD5, (OperationContext)null);
        //        return memoryStream;
        //    }
        //    catch (Exception ex)
        //    {
        //        return (MemoryStream)null;
        //    }
        //}



        public MemoryStream DownLoadFilefromBlobStorage(string foldername, string blobfilename)
        {
            try
            {

                BlobRequestOptions optionsWithUseTransactionalMD5 = new BlobRequestOptions() { UseTransactionalMD5 = true };

                CloudBlockBlob blockBlobReference = new CloudStorageAccount(new StorageCredentials(BlobOperationsLibrary.AccountName, BlobOperationsLibrary.AccountKey), true).CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName + "/" + foldername).GetBlockBlobReference(blobfilename);
                MemoryStream memoryStream = new MemoryStream();
                blockBlobReference.DownloadToStream((Stream)memoryStream, (AccessCondition)null, optionsWithUseTransactionalMD5, (OperationContext)null);
                return memoryStream;
            }
            catch (Exception ex)
            {
                return (MemoryStream)null;
            }
        }

        public Stream ParallelDownLoadFilefromBlobStorage(string foldername, string blobfilename)
        {
            try
            {
                CloudBlockBlob blockBlobReference = new CloudStorageAccount(new StorageCredentials(BlobOperationsLibrary.AccountName, BlobOperationsLibrary.AccountKey), true).CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName + "/" + foldername).GetBlockBlobReference(blobfilename);

                Stream mss = null;

                MemoryStream ms = new MemoryStream();

                mss = ParallelDownloadBlob(ms, blockBlobReference);
                mss.Seek(0, SeekOrigin.Begin);
                return mss;

            }
            catch (Exception ex)
            {
                return (MemoryStream)null;
            }
        }


        public static MemoryStream DownLoadFilefromBlobStorageMI()
        {
            try
            {
                BlobRequestOptions optionsWithUseTransactionalMD5 = new BlobRequestOptions() { UseTransactionalMD5 = true };

                CloudStorageAccount result = Task.Run<CloudStorageAccount>(new Func<Task<CloudStorageAccount>>(BlobOperationsLibrary.GetStorageCredential)).GetAwaiter().GetResult();
                AzureServiceTokenProvider serviceTokenProvider = new AzureServiceTokenProvider((string)null, "https://login.microsoftonline.com/");
                CloudBlockBlob blockBlobReference = result.CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName).GetBlockBlobReference(BlobOperationsLibrary.FolderName + "/" + BlobOperationsLibrary.BlobFileName);

                using (MemoryStream memoryStream = new MemoryStream())
                {

                    blockBlobReference.DownloadToStream((Stream)memoryStream, (AccessCondition)null, (BlobRequestOptions)null, (OperationContext)null);
                    memoryStream.Position = 0;

                    return memoryStream;
                }
            }
            catch (Exception ex)
            {
                return (MemoryStream)null;
            }
        }



        public Stream ParallelDownLoadFilefromBlobStorageMI(string foldername, string blobfilename)
        {
            try
            {
                BlobRequestOptions optionsWithUseTransactionalMD5 = new BlobRequestOptions() { UseTransactionalMD5 = true };




                CloudStorageAccount result = Task.Run<CloudStorageAccount>(new Func<Task<CloudStorageAccount>>(BlobOperationsLibrary.GetStorageCredential)).GetAwaiter().GetResult();
                AzureServiceTokenProvider serviceTokenProvider = new AzureServiceTokenProvider((string)null, "https://login.microsoftonline.com/");
                CloudBlockBlob blockBlobReference = result.CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName).GetBlockBlobReference(foldername + "/" + blobfilename);

                Stream mss = null;

                MemoryStream ms = new MemoryStream();

                mss = ParallelDownloadBlob(ms, blockBlobReference);

                mss.Seek(0, SeekOrigin.Begin);

                return mss;


            }
            catch (Exception ex)
            {
                return (MemoryStream)null;
            }

        }

        public string CheckBlobfromBlobStorageMI(string foldername, string blobfilename)
        {
            string isblobexists = "false";
            try
            {
                BlobRequestOptions optionsWithUseTransactionalMD5 = new BlobRequestOptions() { UseTransactionalMD5 = true };

                CloudStorageAccount result = Task.Run<CloudStorageAccount>(new Func<Task<CloudStorageAccount>>(BlobOperationsLibrary.GetStorageCredential)).GetAwaiter().GetResult();
                AzureServiceTokenProvider serviceTokenProvider = new AzureServiceTokenProvider((string)null, "https://login.microsoftonline.com/");
                CloudBlockBlob blockBlobReference = result.CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName).GetBlockBlobReference(foldername + "/" + blobfilename);

                if (blockBlobReference.Exists())
                {

                    isblobexists = "true";

                    if (blockBlobReference.Properties.Length > 4500)
                    {
                        isblobexists = "true";
                    }

                    else
                    {
                        isblobexists = "Blank";
                    }

                }

                else

                {
                    isblobexists = "false";
                }


                return isblobexists;


            }
            catch (Exception ex)
            {
                return "false";
            }

        }


        public string CheckBlobfromBlobStorage(string foldername, string blobfilename)
        {
            string isblobexists = "false";

            try
            {

                CloudBlockBlob blockBlobReference = new CloudStorageAccount(new StorageCredentials(BlobOperationsLibrary.AccountName, BlobOperationsLibrary.AccountKey), true).CreateCloudBlobClient().GetContainerReference(BlobOperationsLibrary.ContainerName + "/" + foldername).GetBlockBlobReference(blobfilename);

                if (blockBlobReference.Exists())
                {

                    isblobexists = "true";

                    if (blockBlobReference.Properties.Length > 4500)
                    {
                        isblobexists = "true";
                    }

                    else
                    {
                        isblobexists = "Blank";
                    }

                }

                else

                {
                    isblobexists = "false";
                }


                return isblobexists;


            }
            catch (Exception ex)
            {
                return "false";
            }

        }


        private Stream ParallelDownloadBlob(Stream outPutStream, CloudBlockBlob blob)
        {
            blob.FetchAttributes();
            int bufferLength = 1 * 1024 * 1024;//1 MB chunk
            long blobRemainingLength = blob.Properties.Length;
            Queue<KeyValuePair<long, long>> queues = new Queue<KeyValuePair<long, long>>();
            long offset = 0;
            while (blobRemainingLength > 0)
            {
                long chunkLength = (long)Math.Min(bufferLength, blobRemainingLength);
                queues.Enqueue(new KeyValuePair<long, long>(offset, chunkLength));
                offset += chunkLength;
                blobRemainingLength -= chunkLength;
            }
            Parallel.ForEach(queues,
                new ParallelOptions()
                {
                    //Gets or sets the maximum number of concurrent tasks
                    MaxDegreeOfParallelism = 10
                }, (queue) =>
                {
                    using (var ms = new MemoryStream())
                    {
                        blob.DownloadRangeToStream(ms, queue.Key, queue.Value);
                        lock (outPutStream)
                        {
                            outPutStream.Position = queue.Key;
                            var bytes = ms.ToArray();
                            outPutStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                });

            return outPutStream;
        }


        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            AuthenticationContext authContext = new AuthenticationContext(authority);
            ClientCredential clientCred = new ClientCredential("xyz", "abc");
            AuthenticationResult result = await authContext.AcquireTokenAsync(resource, clientCred);
            if (result == null)
                throw new InvalidOperationException("Failed to obtain the JWT token");
            return result.AccessToken;
        }

        private static async Task<NewTokenAndFrequency> TokenRenewerAsync(
        object state,
        CancellationToken cancellationToken)
        {
            string StorageResource = BlobOperationsLibrary.BlobServiceURI;
            AppAuthenticationResult authResult = await ((AzureServiceTokenProvider)state).GetAuthenticationResultAsync(StorageResource, (string)null, new CancellationToken());
            TimeSpan next = authResult.ExpiresOn - DateTimeOffset.UtcNow - TimeSpan.FromMinutes(5.0);
            if (next.Ticks < 0L)
                next = new TimeSpan();
            return new NewTokenAndFrequency(authResult.AccessToken, new TimeSpan?(next));
        }

        private static async Task<CloudStorageAccount> GetStorageCredential()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider((string)null, "https://login.microsoftonline.com/");
            NewTokenAndFrequency tokenAndFrequency1 = await BlobOperationsLibrary.TokenRenewerAsync((object)azureServiceTokenProvider, CancellationToken.None);
            NewTokenAndFrequency tokenAndFrequency2 = tokenAndFrequency1;
            tokenAndFrequency1 = new NewTokenAndFrequency();
            TokenCredential tokenCredential = new TokenCredential(tokenAndFrequency2.Token, new RenewTokenFuncAsync(BlobOperationsLibrary.TokenRenewerAsync), (object)azureServiceTokenProvider, tokenAndFrequency2.Frequency.Value);
            CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(tokenCredential), BlobOperationsLibrary.AccountName, "core.windows.net", true);
            return cloudStorageAccount;
        }
    }
}
