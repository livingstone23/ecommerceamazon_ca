using Ecommerce.Application.Models.ImageManagement;


namespace Ecommerce.Application.Contracts.Infrastructure;

    //Interface que permite utilizar por cualquier clase para el manejo de las imagenes
    public interface IManageImageService
    {

        //Metodo para realizar el upload de la imagen
        Task<ImageResponse> UploadImage(ImageData imageStream);

    }

