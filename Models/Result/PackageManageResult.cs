using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class PackageManageResult
    {
        public List<PackageCartDto> Packages { get; set; } = new List<PackageCartDto>();

        public BaseResult Result { get; set; } = new BaseResult();
    }
}
