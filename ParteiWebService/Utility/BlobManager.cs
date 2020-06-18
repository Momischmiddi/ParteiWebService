using Aufgabe_2.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Aufgabe_2.StorageManagers
{
    public class BlobManager
    {
        public static BlobContainerClient imageBlobContainer;
        public static BlobContainerClient travelImageBlobContainer;

        public static async Task<Result> AddImageAsync(string fileName, IFormFile file)
        {
            var result = new Result();

            try
            {
                BlobClient blobClient = imageBlobContainer.GetBlobClient(fileName);

                var stream = file.OpenReadStream();

                if(await blobClient.ExistsAsync())
                {
                    //result.Successfull = false;
                    //result.Payload = "File named " + fileName + " already exists.";

                    result.Successfull = true;
                    result.Payload = blobClient.Uri.AbsoluteUri;
                } 
                else
                {
                    var resp = await blobClient.UploadAsync(stream, true);
                    stream.Close();

                    result.Successfull = true;
                    result.Payload = blobClient.Uri.AbsoluteUri;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                result.Successfull = false;
                result.Payload = e.Message;
            }

            return result;
        }

        public static async Task<Result> AddTravelImagesAsync(List<TravelImage> travelImages)
        {
            var result = new Result
            {
                Successfull = true
            };

            List<string> imageUlrs = new List<string>();

            try
            {
                foreach(TravelImage travelImage in travelImages) 
                {
                    if(!result.Successfull)
                    {
                        break;
                    }

                    BlobClient blobClient = travelImageBlobContainer.GetBlobClient(travelImage.FileName);

                    var stream = travelImage.File.OpenReadStream();

                    if (await blobClient.ExistsAsync())
                    {
                        result.Successfull = false;
                        result.Payload = "File named " + travelImage.FileName + " already exists.";
                    }
                    else
                    {
                        var resp = await blobClient.UploadAsync(stream, true);
                        stream.Close();

                        result.Successfull = true;
                        imageUlrs.Add(blobClient.Uri.AbsoluteUri);
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result.Successfull = false;
                result.Payload = e.Message;
            }

            if(result.Successfull)
            {
                result.Payload = imageUlrs;
            }

            return result;
        }

        public static void AddDummyTravel()
        {
            List<TravelImage> images = new List<TravelImage>();

            using (var stream = File.OpenRead(@"C:\Users\morit\Desktop\Bilder\bob1.png"))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                images.Add(
                    new TravelImage
                    {
                        FileName = "Bob1.png",
                        File = file
                    }
                );

                using (var stream2 = File.OpenRead(@"C:\Users\morit\Desktop\Bilder\bob2.png"))
                {
                    var file2 = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "image/png"
                    };

                    images.Add(
                        new TravelImage
                        {
                            FileName = "Bob2.png",
                            File = file2
                        }
                    );

                    var res = AddTravelImagesAsync(images).Result;
                }
            }
        }

        public static void Setup()
        {
            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(Credentials.BlobServiceClientKey);
                imageBlobContainer = blobServiceClient.GetBlobContainerClient("images");
                travelImageBlobContainer = blobServiceClient.GetBlobContainerClient("travelimages");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
