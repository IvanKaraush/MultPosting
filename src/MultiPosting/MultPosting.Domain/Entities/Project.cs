using MultPosting.Domain.Primitives;
using Shared.Domain.Entities;
using Shared.Domain.Exceptions;

namespace MultPosting.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; }
    public IReadOnlyCollection<UserResource> UserResources => _userResources.AsReadOnly();
    public List<UserResource> _userResources;

    public Project(Guid id, string name) : base(id)
    {
        Name = name;
        _userResources = [];
    }

    private Project() : base(Guid.NewGuid())
    {
        _userResources = [];
    }

    public void AddUserResourceIfNotExist(Guid id, string name, string imageUrl, bool isSelected)
    {
        var userResource = _userResources.FirstOrDefault(c => c.Id == id);
        if (userResource == null)
        {
            _userResources.Add(new UserResource(id, name, imageUrl, isSelected));
        }
    }

    public void DeleteUserResource(Guid id)
    {
        var userResource = _userResources.FirstOrDefault(c => c.Id == id);
        if (userResource == null)
        {
            throw new EntityNotFoundException(string.Format(ExceptionMessages.EntityNotFound, id));
        }

        _userResources.Remove(userResource);
    }
}