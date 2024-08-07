using AutoMapper;
using BCrypt.Net;
using Entities;
using Extensions;
using Models.Domain;
//using Entities;

namespace Contracts{


public interface IAuthenticateTransientUser
{
    IEnumerable<User> GetAll();
    User GetById(int id);
    void Create(CreateRequest model);
    void Update(int id, UpdateRequest model);
    void Delete(int id);
}
}