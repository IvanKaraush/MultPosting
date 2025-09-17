using Shared.Domain.Entities;

namespace MultPosting.Domain.Entities;

public class UserResource : BaseEntity
{
    public string Name { get; private set; }
    public string ImageUrl { get; private set; }
    public bool IsSelected { get; private set; }

    public UserResource(Guid id, string name, string imageUrl, bool isSelected) : base(id)
    {
        Name = name;
        IsSelected = isSelected;
        ImageUrl = imageUrl;
    }

    private UserResource() : base(Guid.NewGuid())
    {
    }
}