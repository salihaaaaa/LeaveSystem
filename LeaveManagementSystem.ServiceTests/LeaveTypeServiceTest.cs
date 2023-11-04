using AutoFixture;
using FluentAssertions;
using Moq;
using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using Xunit.Abstractions;

namespace LeaveManagementSystem.ServiceTests
{
    public class LeaveTypeServiceTest
    {
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly Mock<ILeaveTypeRepository> _leaveTypeRepositoryMock;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        private readonly ILeaveTypeAdderService _leaveTypeAdderService;
        private readonly ILeaveTypeGetterService _leaveTypeGetterService;
        private readonly ILeaveTypeDeleterService _leaveTypeDeleterService;
        private readonly ILeaveTypeUpdaterService _leaveTypeUpdaterService;

        public LeaveTypeServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;

            _leaveTypeRepositoryMock = new Mock<ILeaveTypeRepository>();
            _leaveTypeRepository = _leaveTypeRepositoryMock.Object;

            _leaveTypeAdderService = new LeaveTypeAdderService(_leaveTypeRepository);
            _leaveTypeGetterService = new LeaveTypeGetterService(_leaveTypeRepository);
            _leaveTypeDeleterService = new LeaveTypeDeleterService(_leaveTypeRepository);
            _leaveTypeUpdaterService = new LeaveTypeUpdaterService(_leaveTypeRepository);
        }

        #region AddLeaveType
        [Fact]
        public async Task AddLeaveType_NullLeaveType_ToBeArgumentNullException()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = null;

            //Act
            var action = async () =>
            {
                await _leaveTypeAdderService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddLeaveType_LeaveTypeNameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture
                .Build<LeaveTypeAddRequest>()
                .With(temp => temp.LeaveTypeName, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _leaveTypeAdderService.AddLeaveType(leaveTypeAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddLeaveType_FullLeaveTypeDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveTypeAddRequest? leaveTypeAddRequest = _fixture
                .Create<LeaveTypeAddRequest>();

            LeaveType leaveType = leaveTypeAddRequest.ToLeaveType();
            LeaveTypeResponse leavetype_response_expected = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .AddLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            //Act
            LeaveTypeResponse leavetype_response_from_add = await _leaveTypeAdderService.AddLeaveType(leaveTypeAddRequest);
            leavetype_response_expected.LeaveTypeID = leavetype_response_from_add.LeaveTypeID;

            //Assert
            leavetype_response_from_add.LeaveTypeID.Should().NotBe(Guid.Empty);
            leavetype_response_from_add.Should().BeEquivalentTo(leavetype_response_expected);
        }
        #endregion

        #region GetAllLeaveType
        [Fact]
        public async Task GetAllLeaveType_ToBeEmptyList()
        {
            //Arrange
            var leaveTypes = new List<LeaveType>();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .GetAllLeaveType())
                .ReturnsAsync(leaveTypes);

            //Act
            List<LeaveTypeResponse> leaveType_response_from_get = await _leaveTypeGetterService.GetAllLeaveType();

            //Assert
            leaveType_response_from_get.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllLeaveType_WithFewLeaveType_ToBeSuccessful()
        {
            //Arrange
            List<LeaveType> leaveTypes = new List<LeaveType>()
            {
                _fixture.Create<LeaveType>(),
                _fixture.Create<LeaveType>(),
                _fixture.Create<LeaveType>(),
            };

            List<LeaveTypeResponse> leaveType_response_list_expected = leaveTypes
                                                                        .Select(temp => temp
                                                                        .ToLeaveTypeResponse()).ToList();

            //print leaveType_response_list_expected
            _testOutputHelper.WriteLine("Expected: ");
            foreach (LeaveTypeResponse leaveType_response_expected in leaveType_response_list_expected)
            {
                _testOutputHelper.WriteLine(leaveType_response_expected.ToString());
            }

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .GetAllLeaveType())
                .ReturnsAsync(leaveTypes);

            //Act
            List<LeaveTypeResponse> leaveTypes_response_list_from_get = await _leaveTypeGetterService.GetAllLeaveType();

            //print leaveTypes_response_list_from_get
            _testOutputHelper.WriteLine("Actual: ");
            foreach (LeaveTypeResponse leaveTypes_response_from_get in leaveTypes_response_list_from_get)
            {
                _testOutputHelper.WriteLine(leaveTypes_response_from_get.ToString());
            }

            //Assert
            leaveTypes_response_list_from_get.Should().BeEquivalentTo(leaveType_response_list_expected);
        }
        #endregion

        #region GetLeaveTypeByLeaveTypeID
        [Fact]
        public async Task GetLeaveTypeByLeaveTypeID_NullLeaveTypeID_ToBeNull()
        {
            //Arrange
            Guid? leaveTypeID = null;

            //Act
            LeaveTypeResponse? leaveType_response_from_get = await _leaveTypeGetterService.GetLeaveTypeByLeaveTypeID(leaveTypeID);

            //Assert
            leaveType_response_from_get.Should().BeNull();
        }

        [Fact]
        public async Task GetLeaveByLeaveTypeID_ToBeSuccessful()
        {
            //Arrange
            LeaveType leaveType = _fixture.Create<LeaveType>();

            LeaveTypeResponse leaveType_response_expected = leaveType.ToLeaveTypeResponse();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .GetLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveType);

            //Act
            LeaveTypeResponse? leaveType_response_from_get = await _leaveTypeGetterService.GetLeaveTypeByLeaveTypeID(leaveType.LeaveTypeID);

            //Assert
            leaveType_response_from_get.Should().BeEquivalentTo(leaveType_response_expected);
        }
        #endregion

        #region UpdateLeaveType
        [Fact]
        public async Task UpdateLeaveType_NullLeaveType_ToBeArgumentNullException()
        {
            //Arrange
            LeaveTypeUpdateRequest? leaveTypeUpdateRequest = null;

            //Act
            var action = async () =>
            {
                await _leaveTypeUpdaterService.UpdateLeaveType(leaveTypeUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        [Fact]
        public async Task UpdateLeaveType_LeaveTypeNameIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveTypeUpdateRequest? leaveTypeUpdateRequest = _fixture
                .Build<LeaveTypeUpdateRequest>()
                .With(temp => temp.LeaveTypeName, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _leaveTypeUpdaterService.UpdateLeaveType(leaveTypeUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateLeaveType_FullLeaveTypeDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveType? leaveType = _fixture
                .Create<LeaveType>();

            LeaveTypeResponse leaveType_response_expected = leaveType.ToLeaveTypeResponse();

            LeaveTypeUpdateRequest leaveType_update_request = leaveType_response_expected.ToLeaveTypeUpdateRequest();

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .GetLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
                .ReturnsAsync(leaveType);

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .UpdateLeaveType(It.IsAny<LeaveType>()))
                .ReturnsAsync(leaveType);

            //Act
            LeaveTypeResponse leavetype_response_from_update = await _leaveTypeUpdaterService.UpdateLeaveType(leaveType_update_request);
            leaveType_response_expected.LeaveTypeID = leavetype_response_from_update.LeaveTypeID;

            //Assert
            leavetype_response_from_update.LeaveTypeID.Should().NotBe(Guid.Empty);
            leavetype_response_from_update.Should().BeEquivalentTo(leaveType_response_expected);
        }
        #endregion

        #region DeleteLeaveType
        [Fact]
        public async Task DeleteLeaveType_ValidLeaveTypeID_ToBeSuccessful()
        {
            //Arrange
            LeaveType leaveType = _fixture.Create<LeaveType>();

            _leaveTypeRepositoryMock
               .Setup(temp => temp
               .GetLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
               .ReturnsAsync(leaveType);

            _leaveTypeRepositoryMock
                .Setup(temp => temp
                .DeleteLeaveTypeByLeaveTypeID(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            //Act
            bool isDeleted = await _leaveTypeDeleterService.DeleteLeaveType(leaveType.LeaveTypeID);

            //Assert
            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteLeaveType_InvalidLeaveTypeID()
        {
            //Act
            bool isDeleted = await _leaveTypeDeleterService.DeleteLeaveType(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}