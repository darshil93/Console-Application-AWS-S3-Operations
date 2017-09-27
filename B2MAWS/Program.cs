using Amazon.S3;
using Amazon.SecurityToken;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using System.Collections.Specialized;
using Amazon.SecurityToken.Model;
using Amazon.S3.Util;


namespace B2MAWS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter 1 : to check if bucket exists");
            Console.WriteLine("Enter 2 : to create a bucket");
            Console.WriteLine("Enter 3 : to upload csv file");
            Console.WriteLine("Enter 4 : to run sql script file");
            int value = 1;
            while (value > 0)
            {
                Console.WriteLine("Enter Choice :");
                value = int.Parse(Console.ReadLine());
                switch (value)
                {
                    case 1:
                        { Console.WriteLine("Enter Bucket Name"); string s = Console.ReadLine(); CreateBucket u = new CreateBucket(s);
                            //if (CreateBucket.CheckBucketExist()) { Console.WriteLine("Bucket Does not Exists"); } else { Console.WriteLine("Bucket Exists"); }  // mentioned in the checkbucketclass
                            CreateBucket.CheckBucketExist(); break; }
                    case 2:
                        {  CreateBucket c = new CreateBucket(UploadObject.bucketName); CreateBucket.CheckAndCreateBucket(); break; }
                    case 3:
                        { UploadObject u = new UploadObject(); UploadObject.Connection(); break; }
                    case 4:
                        {
                            CSVParser.GetScript();
                            break;
                        }

                }
            }
            //call the constructor
            //UploadObject u = new UploadObject();

            //check whether bucket exists
            //CreateBucket c = new CreateBucket(UploadObject.bucketName);//CreateBucket c = new CreateBucket("todayprosp");
            //CreateBucket.CheckBucketExist();

            //upload csv file
            //UploadObject.Connection();

        }
    }

    #region //STSLOGIC
    //class TempCredExplicitSessionStart
    //{
    //    static string bucketName = ConfigurationManager.AppSettings["buketname"];
    //    static IAmazonS3 client;

    //    public static void Main(string[] args)
    //    {
    //        NameValueCollection appConfig = ConfigurationManager.AppSettings;
    //        string accessKeyID = appConfig["AWSAccessKey"];
    //        string secretAccessKeyID = appConfig["AWSSecretKey"];

    //        try
    //        {
    //            Console.WriteLine("Listing objects stored in a bucket");
    //            SessionAWSCredentials tempCredentials =GetTemporaryCredentials(accessKeyID, secretAccessKeyID);

    //            // Create client by providing temporary security credentials.
    //            using (client = new AmazonS3Client(tempCredentials, Amazon.RegionEndpoint.USEast1))
    //            {
    //                ListObjectsRequest listObjectRequest = new ListObjectsRequest();
    //                listObjectRequest.BucketName = bucketName;

    //                // Send request to Amazon S3.
    //                ListObjectsResponse response = client.ListObjects(listObjectRequest);
    //                List<S3Object> objects = response.S3Objects;
    //                foreach(var v in objects) { Console.Write("{0}", v.Key); }
    //                Console.WriteLine("Object count = {0}", objects.Count);

    //                Console.WriteLine("Press any key to continue...");
    //                Console.ReadKey();
    //            }
    //        }
    //        catch (AmazonS3Exception s3Exception)
    //        {
    //            Console.WriteLine(s3Exception.Message,
    //                              s3Exception.InnerException);
    //        }
    //        catch (AmazonSecurityTokenServiceException stsException)
    //        {
    //            Console.WriteLine(stsException.Message,
    //                             stsException.InnerException);
    //        }
    //        Console.ReadLine();
    //    }

    //    private static SessionAWSCredentials GetTemporaryCredentials(string accessKeyId, string secretAccessKeyId)
    //    {
    //        AmazonSecurityTokenServiceClient stsClient =
    //            new AmazonSecurityTokenServiceClient(accessKeyId,secretAccessKeyId);

    //        GetSessionTokenRequest getSessionTokenRequest =  new GetSessionTokenRequest();
    //        getSessionTokenRequest.DurationSeconds = 7200; // seconds

    //        GetSessionTokenResponse sessionTokenResponse =
    //                      stsClient.GetSessionToken(getSessionTokenRequest);
    //        Credentials credentials = sessionTokenResponse.Credentials;

    //        SessionAWSCredentials sessionCredentials =
    //            new SessionAWSCredentials(credentials.AccessKeyId,
    //                                      credentials.SecretAccessKey,
    //                                      credentials.SessionToken);

    //        return sessionCredentials;
    //    }
    //}
    #endregion
}
