using System;
using System.IO;
using Microsoft.Azure.WebJobs;
//using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ProductCatalog.Domain;

using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ThumbnailGeneraterFun
{
    public class FuncThumbnail
    {
        private IConfiguration _configuration;
        public FuncThumbnail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName("GenerateThumbnail")]
        public void Run([QueueTrigger("thumbnail-queue")] BlobInformation blobInfo,
                 [Blob("productimages/{BlobName}", FileAccess.Read)] Stream input,
                 [Blob("productimages/{BlobNameWithoutExtension}_thumbnail.jpg", FileAccess.Write)] Stream output, ILogger log)

        {

           
               
                Image image = Image.FromStream(input);
                var thumb = image.GetThumbnailImage(32, 32, () => false, IntPtr.Zero);
                thumb.Save(output, System.Drawing.Imaging.ImageFormat.Jpeg);


            var options = new DbContextOptionsBuilder<ProductCatalogContext>();
            options.UseSqlServer(_configuration.GetConnectionString("ProductCatalogContext"));
            var db = new ProductCatalogContext(options.Options);
            var id = blobInfo.Id;
            CatalogItem item = db.CatalogItems.Find(id);
            item.PictureFileName =$"{ blobInfo.OutputBlobPath}{blobInfo.BlobNameWithoutExtension}_thumbnail.jpg";
            db.SaveChanges();



        }


    }
}
