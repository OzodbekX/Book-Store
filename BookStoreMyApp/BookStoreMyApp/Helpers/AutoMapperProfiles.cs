using AutoMapper;
using BookStoreMyApp.Models;
using BookStoreMyApp.tempModels;
using BookStoreMyApp.ViewModels;

namespace BookStoreMyApp.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BookViewModel, Book>();
            CreateMap<Book,BookViewModel>();
            CreateMap<AuthorViewModel, Author>();
            CreateMap<PictureViewModel, Picture>();
            CreateMap<UserViewModel, User>();
            CreateMap<PublisherViewModel, Publisher>();
            CreateMap<UserTaskView, UserTask>().ForMember(x => x.TaskFile, opt => opt.Ignore());
        }
    }
}
