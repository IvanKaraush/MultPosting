using Mapster;
using MultiPosting.Application.Dto;
using MultPosting.Domain.Entities;

namespace MultiPosting.Application.Mapping;

public class ProjectMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Project, GetUserResourcesByProjectIdResponse>();
        config.NewConfig<UserResource, UserResourceDto>();
    }
}