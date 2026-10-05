using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;


namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        //teha constructor, mis ühendab DB-d 
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, SpacesShip domain)
        {
            //kindlasti peab ankeedil olema üks fail
            if (dto.Files != null && dto.Files.Count > 0)
            {

                //kui ei ole wwwroot-s multipleFileUpload directory t
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //tee directory wwwrooti alla
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //meil on vaja teha muutuja nimega uploadsFolder
                    //sinna muutuja taha on vaja Path kombineerida
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    //igale failile unikaalne Guid selle nime ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //iga kord, kui faili laed ülesse, siis tehakse see väikesteks
                    //tükkideks
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //domaini teha FileToApi
                        //FileToApi
                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipID = domain.Id
                        };

                        _context.FileToApis.AddAsync(path);
                    }
                }
            }
        }
    }
}