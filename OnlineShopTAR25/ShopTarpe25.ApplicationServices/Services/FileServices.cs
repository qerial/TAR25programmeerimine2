using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using static System.Net.Mime.MediaTypeNames;


namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        // Andmebaasiga suhtlemiseks
        private readonly ShopTARpe25Context _context;
        // Failide asukoha leidmiseks
        private readonly IHostEnvironment _webHost;

        // Konstruktor
        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        // Failide üleslaadimine ja andmete salvestamine
        public void FilesToApi(KindergartenDto dto, KindergartenDomain domain)
        {
            // Kontrollime, kas failid on olemas
            if (dto.Files != null && dto.Files.Count > 0)
            {
                // Loome kausta, kui seda pole
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }
                // Käime kõik failid läbi
                foreach (var file in dto.Files)
                {

                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    // Loome failile unikaalse nime
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Salvestame faili kausta
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        // Loome faili andmete objekti
                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            KindergartenID = domain.Id
                        };
                        // Lisame faili andmed andmebaasi konteksti
                        _context.FileToApis.Add(path);
                    }
                }
            }
        }

        // Ühe faili kustutamine
        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            // Otsime faili andmebaasist
            var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.ID);

            if (imageId == null)
            {
                return null;
            }

            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

            //Kui fail on olemas, siis kustutab selle
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            // Eemaldame faili andmed andmebaasist
            _context.FileToApis.Remove(imageId);
            // Salvestame muudatused
            await _context.SaveChangesAsync();

            return imageId;
        }

        //Mitme failide kustutamine
        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto)
        {
            // Loome kustutatud failide listi
            List<FileToApi> removedImages = new List<FileToApi>();
            // Käime kõik failid läbi
            foreach (var image in dto)
            {
                var imageID = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == image.ID);
                // Kui faili ei leita, liigume järgmise juurde
                if (imageID == null)
                {
                    continue;
                }
                // Leiame faili asukoha
                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                               + imageID.ExistingFilePath;
                // Kustutame faili, kui see on olemas
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                // Eemaldame faili andmed andmebaasist
                _context.FileToApis.Remove(imageID);
                // Lisame faili nimekirja
                removedImages.Add(imageID);
            }
            // Salvestame kõik muudatused
            await _context.SaveChangesAsync();
            // Tagastame kustutatud failide andmed
            return removedImages;
        }
    }
}