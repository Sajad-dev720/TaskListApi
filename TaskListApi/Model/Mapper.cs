using AutoMapper;
using TaskListApi.Domain;

namespace TaskListApi.Model;

public class Mapper : Profile
{
	public Mapper()
	{
		CreateMap<TaskEntity, TaskModel>().ReverseMap();
	}
}
