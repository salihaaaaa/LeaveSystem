using AutoFixture;
using FluentAssertions;
using Moq;
using LeaveManagementSystem.Core.Domain.Entities;
using LeaveManagementSystem.Core.Domain.IdentityEntities;
using LeaveManagementSystem.Core.Domain.RepositoryContracts;
using LeaveManagementSystem.Core.DTO;
using LeaveManagementSystem.Core.Enums;
using LeaveManagementSystem.Core.ServiceContracts;
using LeaveManagementSystem.Core.Services;
using System.Linq.Expressions;
using Xunit.Abstractions;

namespace LeaveManagementSystem.ServiceTests
{
    public class LeaveServiceTest
    {
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly Mock<ILeaveRepository> _leaveRepositoryMock;
        private readonly ILeaveRepository _leaveRepository;

        private readonly ILeaveAdderService _leaveAdderService;
        private readonly ILeaveGetterService _leaveGetterService;
        private readonly ILeaveDeleterService _leaveDeleterService;
        private readonly ILeaveUpdaterService _leaveUpdaterService;
        private readonly ILeaveSorterService _leaveSorterService;
        
        public LeaveServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _testOutputHelper = testOutputHelper;

            _leaveRepositoryMock = new Mock<ILeaveRepository>();
            _leaveRepository = _leaveRepositoryMock.Object;

            _leaveAdderService = new LeaveAdderService(_leaveRepository);
            _leaveGetterService = new LeaveGetterService(_leaveRepository);
            _leaveDeleterService = new LeaveDeleterService(_leaveRepository);
            _leaveUpdaterService = new LeaveUpdaterService(_leaveRepository);
            _leaveSorterService = new LeaveSorterService(_leaveRepository);
        }

        #region AddLeave
        [Fact]
        public async Task AddLeave_NullLeave_ToBeArgumentNullException()
        {
            //Arrange
            LeaveAddRequest? leaveAddRequest = null;

            //Act
            var action = async () =>
            {
                await _leaveAdderService.AddLeave(leaveAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddLeave_ReasonIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveAddRequest? leaveAddRequest = _fixture
                .Build<LeaveAddRequest>().With(temp => temp.Reason, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _leaveAdderService.AddLeave(leaveAddRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddLeave_FullLeaveDetails_ToBeSuccessful()
        {
            //Arrange
            LeaveAddRequest? leave_add_request = _fixture.Create<LeaveAddRequest>();

            Leave leave = leave_add_request.ToLeave();
            LeaveResponse leave_response_expected = leave.ToLeaveResponse();

            _leaveRepositoryMock
                .Setup(temp => temp.AddLeave(It.IsAny<Leave>()))
                .ReturnsAsync(leave);

            //Act
            LeaveResponse leave_response_from_add = await _leaveAdderService.AddLeave(leave_add_request);
            leave_response_expected.LeaveID = leave_response_from_add.LeaveID;

            //Assert
            leave_response_from_add.LeaveID.Should().NotBe(Guid.Empty);
            leave_response_from_add.Should().BeEquivalentTo(leave_response_expected);
        }
        #endregion

        #region GetAllLeave
        [Fact]
        public async Task GetAllLeave_ToBeEmptyList()
        {
            //Arrange
            var leaves = new List<Leave>();

            _leaveRepositoryMock
                .Setup(temp => temp.GetAllLeave())
                .ReturnsAsync(leaves);

            //Act
            List<LeaveResponse> leaves_response_from_get = await _leaveGetterService.GetAllLeave();

            //Assert
            leaves_response_from_get.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllLeave_WithFewLeave_ToBeSuccessful()
        {
            //Arrange
            List<Leave> leaves = new List<Leave>()
           {
               _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Pending")
                .Create(),

               _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Rejected")
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Approved")
                .Create(),
           };

            List<LeaveResponse> leave_response_list_expected = leaves.Select(temp => temp.ToLeaveResponse()).ToList();

            //print leave_response_list_expected
            _testOutputHelper.WriteLine("Expected:");
            foreach (LeaveResponse leave_response_expected in leave_response_list_expected)
            {
                _testOutputHelper.WriteLine(leave_response_expected.ToString());
            }

            _leaveRepositoryMock
                .Setup(temp => temp.GetAllLeave())
                .ReturnsAsync(leaves);

            //Act
            List<LeaveResponse> leave_response_list_from_get = await _leaveGetterService.GetAllLeave();

            //print leave_response_list_from_get
            _testOutputHelper.WriteLine("Actual:");
            foreach (LeaveResponse leave_response_from_get in leave_response_list_from_get)
            {
                _testOutputHelper.WriteLine(leave_response_from_get.ToString());
            }

            //Assert
            leave_response_list_from_get.Should().BeEquivalentTo(leave_response_list_expected);
        }
        #endregion

        #region GetFilteredLeave
        [Fact]
        public async Task GetFilteredLeave_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            List<Leave> leaves = new List<Leave>()
            {
                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Pending")
                .Create(),

               _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Rejected")
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Approved")
                .Create()
            };

            List<LeaveResponse> leave_response_list_expected = leaves.Select(temp => temp.ToLeaveResponse()).ToList();

            //print leave_response_list_expected
            _testOutputHelper.WriteLine("Expected:");
            foreach (LeaveResponse leave_response_expected in leave_response_list_expected)
            {
                _testOutputHelper.WriteLine(leave_response_expected.ToString());
            }

            _leaveRepositoryMock
                .Setup(temp => temp.GetFilteredLeave(It.IsAny<Expression<Func<Leave, bool>>>()))
                .ReturnsAsync(leaves);

            //Act
            List<LeaveResponse> leave_response_list_from_search = await _leaveGetterService.GetFilteredLeave(nameof(Leave.Status), "");

            //print leave_response_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (LeaveResponse leave_response_from_search in leave_response_list_from_search)
            {
                _testOutputHelper.WriteLine(leave_response_from_search.ToString());
            }

            //Assert
            leave_response_list_from_search.Should().BeEquivalentTo(leave_response_list_expected);
        }

        [Fact]
        public async Task GetFilteredLeave_SearchByStatus_ToBeSuccessful()
        {
            //Arrange
            List<Leave> leaves = new List<Leave>()
            {
                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Pending")
                .Create(),

               _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Rejected")
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Approved")
                .Create()
            };

            List<LeaveResponse> leave_response_list_expected = leaves.Select(temp => temp.ToLeaveResponse()).ToList();

            //print leave_response_list_expected
            _testOutputHelper.WriteLine("Expected:");
            foreach (LeaveResponse leave_response_expected in leave_response_list_expected)
            {
                _testOutputHelper.WriteLine(leave_response_expected.ToString());
            }

            _leaveRepositoryMock
                .Setup(temp => temp.GetFilteredLeave(It.IsAny<Expression<Func<Leave, bool>>>()))
                .ReturnsAsync(leaves);

            //Act
            List<LeaveResponse> leave_response_list_from_search = await _leaveGetterService.GetFilteredLeave(nameof(Leave.Status), "app");

            //print leave_response_list_from_search
            _testOutputHelper.WriteLine("Actual:");
            foreach (LeaveResponse leave_response_from_search in leave_response_list_from_search)
            {
                _testOutputHelper.WriteLine(leave_response_from_search.ToString());
            }

            //Assert
            leave_response_list_from_search.Should().BeEquivalentTo(leave_response_list_expected);
        }
        #endregion

        #region GetSortedLeave
        [Fact]
        public async Task GetSortedLeave_ToBeSuccessful()
        {
            //Arrange
            List<Leave> leaves = new List<Leave>()
            {
                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Pending")
                .Create(),

               _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Rejected")
                .Create(),

                _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Approved")
                .Create()
            };

            List<LeaveResponse> leave_response_list_expected = leaves.Select(temp => temp.ToLeaveResponse()).ToList();

            //print leave_response_list_expected
            _testOutputHelper.WriteLine("Expected:");
            foreach (LeaveResponse leave_response_expected in leave_response_list_expected)
            {
                _testOutputHelper.WriteLine(leave_response_expected.ToString());
            }

            _leaveRepositoryMock
                .Setup(temp => temp.GetAllLeave())
                .ReturnsAsync(leaves);

            List<LeaveResponse> allLeaves = await _leaveGetterService.GetAllLeave();

            //Act
            List<LeaveResponse> leave_response_list_from_sort = await _leaveSorterService.GetSortedLeave(allLeaves, nameof(Leave.User), SortOrderOptions.DESC);

            //print leave_response_list_from_sort
            _testOutputHelper.WriteLine("Actual:");
            foreach (LeaveResponse leave_response_from_sort in leave_response_list_from_sort)
            {
                _testOutputHelper.WriteLine(leave_response_from_sort.ToString());
            }

            //Assert
            leave_response_list_from_sort.Should().BeInDescendingOrder(temp => temp.Name);
        }
        #endregion

        #region GetLeaveByLeaveID
        [Fact]
        public async Task GetLeaveByLeaveID_NullLeaveID_ToBeNull()
        {
            //Arrange
            Guid? leaveID = null;

            //Act
            LeaveResponse? leave_response_from_get = await _leaveGetterService.GetLeaveByLeaveID(leaveID);

            //Assert
            leave_response_from_get.Should().BeNull();
        }

        [Fact]
        public async Task GetLeaveByLeaveID_WithLeaveID_ToBeSuccessfull()
        {
            //Arrange
            Leave leave = _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .Create();

            LeaveResponse leave_response_expected = leave.ToLeaveResponse();

            _leaveRepositoryMock
                .Setup(temp => temp.GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leave);

            //Act
            LeaveResponse? leave_response_from_get = await _leaveGetterService.GetLeaveByLeaveID(leave.LeaveID);

            //Assert
            leave_response_from_get.Should().BeEquivalentTo(leave_response_expected);
        }
        #endregion

        #region UpdateLeave
        [Fact]
        public async Task UpdateLeave_NullLeave_ToBeArgumentNullException()
        {
            //Arrange
            LeaveUpdateRequest? leave_update_request = null;

            //Act
            var action = async () =>
            {
                await _leaveUpdaterService.UpdateLeave(leave_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateLeave_ReasonIsNull_ToBeArgumentException()
        {
            //Arrange
            LeaveUpdateRequest? leave_update_request = _fixture
                .Build<LeaveUpdateRequest>()
                .With(temp => temp.Reason, null as string)
                .Create();

            //Act
            var action = async () =>
            {
                await _leaveUpdaterService.UpdateLeave(leave_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdateLeave_FullLeaveDetails_ToBeSuccessful()
        {
            //Arrange
            Leave leave = _fixture
                .Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Rejected")
                .Create();

            LeaveResponse leave_response_expected = leave.ToLeaveResponse();

            LeaveUpdateRequest leave_update_request = leave_response_expected.ToLeaveUpdateRequest();

            _leaveRepositoryMock
                .Setup(temp => temp.UpdateLeave(It.IsAny<Leave>()))
                .ReturnsAsync(leave);

            _leaveRepositoryMock
                .Setup(temp => temp.GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leave);

            //Act
            LeaveResponse leave_response_from_update = await _leaveUpdaterService.UpdateLeave(leave_update_request);

            //Assert
            leave_response_from_update.Should().BeEquivalentTo(leave_response_expected);
        }
        #endregion

        #region DeleteLeave
        [Fact]
        public async Task DeleteLeave_ValidLeaveID_ToBeSuccessful()
        {
            //Arrange 
            Leave leave = _fixture.Build<Leave>()
                .With(temp => temp.User, null as ApplicationUser)
                .With(temp => temp.LeaveType, null as LeaveType)
                .With(temp => temp.Status, "Rejected")
                .Create();

            _leaveRepositoryMock
                .Setup(temp => temp.DeleteLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            _leaveRepositoryMock
                .Setup(temp => temp.GetLeaveByLeaveID(It.IsAny<Guid>()))
                .ReturnsAsync(leave);

            //Act
            bool isDeleted = await _leaveDeleterService.DeleteLeave(leave.LeaveID);

            //Assert
            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteLeave_InvalidLeaveID()
        {
            //Act
            bool isDeleted = await _leaveDeleterService.DeleteLeave(Guid.NewGuid());

            //Assert
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
