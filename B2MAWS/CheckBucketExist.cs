using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B2MAWS
{

    #region CheckAndCreateBucket
    class CreateBucket
    {
        static string bucketName;
        public CreateBucket(string s) { bucketName = s; }

        public static bool CheckBucketExist()
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {

                if (!(AmazonS3Util.DoesS3BucketExist(client, bucketName)))
                {
                    Console.WriteLine("Bucket Doesn't Exist");
                    return true;
                }
                // Retrieve bucket location.
                //string bucketLocation = FindBucketLocation(client);
            }
            Console.WriteLine("Bucket Already Exists");
            return false;
            
        }
        public static void CheckAndCreateBucket()
        {
            using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
            {

                if (!(AmazonS3Util.DoesS3BucketExist(client, bucketName)))
                {
                    Console.WriteLine("Creating Bucket...");
                    CreateABucket(client);
                }
                else
                {
                    Console.WriteLine("Bucket Already Exists.");
                }
                // Retrieve bucket location.
                //string bucketLocation = FindBucketLocation(client);
            }

            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey();
        }

        static string FindBucketLocation(IAmazonS3 client)
        {
            string bucketLocation;
            GetBucketLocationRequest request = new GetBucketLocationRequest()
            {
                BucketName = bucketName
            };
            GetBucketLocationResponse response = client.GetBucketLocation(request);
            bucketLocation = response.Location.ToString();
            return bucketLocation;
        }

        static void CreateABucket(IAmazonS3 client)
        {
            try
            {
                PutBucketRequest putRequest1 = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                PutBucketResponse response1 = client.PutBucket(putRequest1);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine(
                        "For service sign up go to http://aws.amazon.com/s3");
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

