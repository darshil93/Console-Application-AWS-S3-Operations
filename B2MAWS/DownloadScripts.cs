using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2MAWS
{
    class DownloadScripts
    {
        static string bucketName = "milanprosp";
        //static string keyName = "samplescript.sql";
        static public List<string> keyName = ListObjects.scriptslist;
        static IAmazonS3 client;

        public static List<string> GetObject()
        {
            try
            {
                Console.WriteLine("Retrieving (GET) an object");
                List<string> data = ReadObjectData();
                return data;
            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message, s3Exception.InnerException);
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return null;
        }

        static List<string> ReadObjectData()
        {
            List<string> responseBody = new List<string>();
            //responseBody = "";
            if (keyName.Count < 1) return null;
            foreach (var i in keyName)
            {
                using (client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = i
                    };

                    using (GetObjectResponse response = client.GetObject(request))
                    using (Stream responseStream = response.ResponseStream)
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string title = response.Metadata["x-amz-meta-title"];
                        string timestamp = response.LastModified.ToString();
                        //Console.WriteLine("The object's title is {0}", title);

                        responseBody.Add(reader.ReadToEnd());
                    }
                }
            }
            return responseBody;

        }

        public static void DownloadAllScripts()
        {
            IAmazonS3 client;
            foreach (var i in keyName)
            {
                using (client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = bucketName,
                        Key = i
                    };

                    using (GetObjectResponse response = client.GetObject(request))
                    {
                        string dest = Path.Combine("C:/Program Files/Prosperoware.Milan/", i);
                        if (!File.Exists(dest))
                        {
                            response.WriteResponseStreamToFile(dest);
                        }
                        else
                        {
                            DateTime lastModified = System.IO.File.GetLastWriteTime(dest);
                            if (response.LastModified > lastModified)
                                response.WriteResponseStreamToFile(dest);
                        }
                    }
                }
            }
        }
    }

}