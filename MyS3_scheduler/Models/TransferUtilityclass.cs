using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using System.IO;

namespace MyS3_scheduler.Models
{
    class TransferUtilityclass
    {
        //string filePath = "D:\\Test.json";

        string filePath = Path.Combine("Content", "Test.json");


        public void Transfer(string Bucketname,string key)
        {
            var S3client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();
           
            var transferUtility = new TransferUtility(S3client);
           
            TransferUtilityUploadRequest transferUtilityUploadRequest = new TransferUtilityUploadRequest
            {
                BucketName = Bucketname,

                Key = "Even_List",
                FilePath = filePath,
                ContentType = "text/plain"
            };

            transferUtility.Upload(transferUtilityUploadRequest); // use UploadAsync if possible

        }
    }
}
