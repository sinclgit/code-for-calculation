using AutoFixture;
using AutoMapper;
using Moq;
using Entities;
using Extensions;
using Repositories;
using Services;
using Contracts;
using Mapper;
using Models;
using Models.Domain;
using Data;

namespace calculator.Tests.Services{

public class AuthenticateTransientUserTests
{
    private IAuthenticateTransientUser _userService;
    private Mock<ICalculationInMemoryRepository> _mockUserRepository;
    private Fixture _fixture;

    public AuthenticateTransientUserTests()
    {
        // fixture for creating test data
        _fixture = new Fixture();

        // mock user repo dependency
        _mockUserRepository = new Mock<ICalculationInMemoryRepository>();
        //_userService = new Mock<IAuthenticateTransientUser>();

        // automapper dependency
        var mapper = new MapperConfiguration(x => x.AddProfile<AutoMapperProfile>()).CreateMapper();

        // service under test

    }

}
}