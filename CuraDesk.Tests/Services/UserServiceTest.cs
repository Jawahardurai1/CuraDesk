using CuraDesk.Business.Dto;
using CuraDesk.Business.Interface.Repository;
using CuraDesk.Business.Interface.Service;
using CuraDesk.Business.Services;
using CuraDesk.Model.Entities;
using CuraDesk.Utility.Email;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
namespace CuraDesk.Tests.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _UserRepository;
        private readonly Mock<IEmailService> _EmailService;
        private readonly UserService userService;
        public UserServiceTest()
        {
            _UserRepository = new Mock<IUserRepository>();
            _EmailService = new Mock<IEmailService>();
            userService = new UserService(
                _UserRepository.Object, _EmailService.Object);
        }

    
    [Fact]
        public async Task AddUserAsync_ShouldCreateSuccess()
        {
            var UserDto = new CreateUserDto
            {
                UserName = "John",
                EmailId = "john@gmail.com"
            };
            var createdUser = new User
            {
                UserId = Guid.Parse("6b29fc40-ca47-1067-b31d-00dd010662da"),
                UserName = "John",
                EmailId = "john@gmail.com"
            };

            _UserRepository.Setup(x => x.GetUserByEmailIdAsync(UserDto.EmailId)).ReturnsAsync((User?)null);
            _UserRepository
    .Setup(x => x.CreateUserAsync(It.IsAny<User>()))
    .ReturnsAsync(createdUser);
            var result = await userService.AddUserAsync(UserDto);

            Assert.NotNull(result);
            Assert.Equal("John", result.FullName);
        }

    }
}
