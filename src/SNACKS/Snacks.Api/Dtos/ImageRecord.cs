namespace Snacks.Api.Dtos;

public record ImageRecord
{
    public required string FileType { get; set; }
    public required byte[] ImageData { get; set; }
}
