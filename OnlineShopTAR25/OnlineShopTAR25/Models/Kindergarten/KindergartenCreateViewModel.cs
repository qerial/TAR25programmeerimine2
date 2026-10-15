namespace OnlineShopTAR25.Models.Kindergarten
{
    public class KindergartenCreateViewModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public List<IFormFile> Files { get; set; }
        public List<ImageViewModel> Image { get; set; }
            = new List<ImageViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
