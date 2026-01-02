using Uniceps.app.DTOs.ReleaseDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.Extensions.ProductMappers
{
    public class ReleaseMapperExtension : IMapperExtension<Release, ReleaseDto, ReleaseCreationDto>
    {
        public Release FromCreationDto(ReleaseCreationDto data)
        {
            return new Release
            {
                ChangeLog = data.ChangeLog,
                ChangeLogAr = data.ChangeLogAr,
                DownloadSource = data.DownloadSource,
                DownloadUrl = data.DownloadUrl??"",
                ProductId = data.ProductId,
                TargetOS = data.TargetOS,
                Version = data.Version,
            };
        }

        public ReleaseDto ToDto(Release data)
        {
            return new ReleaseDto
            {
                ChangeLog = data.ChangeLog,
                DownloadSource = data.DownloadSource,
                DownloadUrl = data.DownloadUrl ?? "",
                ProductId = data.ProductId,
                TargetOS = data.TargetOS,
                Version = data.Version,
                ChangeLogAr = data.ChangeLogAr,
                CreatedAt = data.CreatedAt,
                TargetOSText = data.TargetOS.ToString(),
                Id = data.Id,
                DownloadSourceText = data.DownloadSource.ToString()
            };
        }
    }
}
