using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2MAWS
{
    #region UploadObject
    public class UploadObject
    {
        //static string bucketName = GetAppSettings("bucketname");
        //static string keyName = GetAppSettings("keyname");
        public static string bucketName { get; set; }
        static string keyName;
        static string filePath;
        static IAmazonS3 client;
        public UploadObject()
        {
            Console.WriteLine("Enter the Bucket Name");
            bucketName = Console.ReadLine();
            DateTime date = DateTime.ParseExact(DateTime.Today.ToString(), "M/d/yyyy h:m:s tt", System.Globalization.CultureInfo.InvariantCulture);
            string formattedDate = date.ToString("yyyy-MM-dd") + "/csv1.csv";
            keyName = formattedDate;
            filePath = GetAppSettings("filepath");
        }

        public static string GetAppSettings(string s)
        {
            var appsettings = ConfigurationManager.AppSettings;
            s = appsettings[s];
            return s;
        }
        public static void Connection()
        {
             
            //var credentials = new StoredProfileAWSCredentials("darshil");
            
                NameValueCollection appConfig = ConfigurationManager.AppSettings;
            using (client = new AmazonS3Client(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"], Amazon.RegionEndpoint.USEast1))
            {
                Console.WriteLine("Uploading an object");
                WritingAnObject();
            }
            Console.WriteLine("File Uploaded");
            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
        }

        private static void WritingAnObject()
        {
            try
            {
                //PutObjectRequest putRequest1 = new PutObjectRequest
                //{
                //    BucketName = bucketName,
                //    Key = keyName,
                //    //ContentBody = "sample text"
                //};

                //PutObjectResponse response1 = client.PutObject(putRequest1);

                // 2. Put object-set ContentType and add metadata.
                PutObjectRequest putRequest2 = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                    FilePath = filePath,
                    ContentType = "text/csv"
                };
                //putRequest2.Metadata.Add("x-amz-meta-title", "someTitle");

                PutObjectResponse response2 = client.PutObject(putRequest2);

            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");

                }
                else
                {
                    Console.WriteLine(
                        "Error occurred. Message:'{0}' when writing an object"
                        , amazonS3Exception.Message);
                }
            }
        }
    }
    #endregion
}
