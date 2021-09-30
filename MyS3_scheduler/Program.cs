using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using MyS3_scheduler.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyS3_scheduler
{

    class Program
    {

        IAmazonS3 S3Client { get; set; }

        public Program(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }
       
        public static async Task Main(string[] args)
        {
            //string filePath = "D:\\Test.json";
            string filePath = Path.Combine("Content", "Test.json");
            

            var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            //////////////////////////////////* To get BucketName and Key Name*/////////////////////////////////////////


            //Console.WriteLine("Enter the Bucket Name : \n");
            request.BucketName = ConfigurationManager.AppSettings["BucketName"];

            //Console.WriteLine("Enter the Key Name : \n");
            request.Key = ConfigurationManager.AppSettings["Key"];


            GetObjectResponse response = await client.GetObjectAsync(request);
            StreamReader reader = new StreamReader(response.ResponseStream);
            string content = reader.ReadToEnd();
            Console.Out.WriteLine("Scheduler started running............\n");
            List<masterjson> dese = JsonConvert.DeserializeObject<List<masterjson>>(content);
            Console.Out.WriteLine("Master Data : \n" + content);

           //////////////////////////////////* logic to get the even numbers*/////////////////////////////////////////
           
            dese.RemoveAll(i => i.id % 2 > 0);
            string serializedval = JsonConvert.SerializeObject(dese);
            

            using (var mutex = new Mutex(false, "LARGEFILE"))
            {
                
                mutex.WaitOne();
                //File.AppendAllText(path, " " + message + Environment.NewLine);
               
                System.IO.File.WriteAllText(filePath, string.Empty);
                System.IO.File.WriteAllText(filePath, serializedval);
                mutex.ReleaseMutex();
            }

            //////////////////////////////////* To Write the Even numbers */////////////////////////////////////////


            TransferUtilityclass obj = new TransferUtilityclass();
            obj.Transfer(request.BucketName ,request .Key );
            Console.Out.WriteLine("Even numbers Present : \n" + serializedval);
        }
      


}


}
