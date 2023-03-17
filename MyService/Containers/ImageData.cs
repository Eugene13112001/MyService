using System.Collections;
using MyService.Models;
namespace MyService.Containers
{
    public interface DataImage
    {

        
        public Image? GetElementId(Guid id);




    }
    public class ImageData : DataImage
    {

        public List<Image> images = new List<Image> {
                new Image { Id= new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "1.png" },
                new Image { Id= new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa7"), Name = "2.png" },


        };

       
        public Image? GetElementId(Guid id)
        {
            return this.images.FirstOrDefault(p => p.Id == id);

        }
       

    }
}
