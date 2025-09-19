using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace MultPosting.Domain.Entities;

public class UserResource : BaseEntity
{
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsSelected { get; private set; }
    public SocialMedia SocialMedia { get; private set; }
    public Guid ProjectId { get; private set; }

    public UserResource(Guid id, string name, string imageUrl, bool isSelected, SocialMedia socialMedia) : base(id)
    {
        Name = name;
        IsSelected = isSelected;
        ImageUrl = imageUrl;
        SocialMedia = socialMedia;
    }

    private UserResource() : base(Guid.NewGuid())
    {
    }
}