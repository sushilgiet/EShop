using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Entities
{
    public class BlobInformation : BaseEntity<int>
    {
        public Uri? BlobUri { get; set; }
        public string BlobName
        {
            get
            {
                return BlobUri!.Segments[BlobUri.Segments.Length - 1];
            }
        }
        public string BlobNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(BlobName);
            }
        }
        public string OutputBlobPath
        {
            get
            {

                return BlobUri!.ToString().Replace(BlobUri!.Segments[BlobUri.Segments.Length - 1], "");

            }
        }
        
    }
}
