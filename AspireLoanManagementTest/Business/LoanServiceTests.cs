using AspireLoanManagement.Business;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Utility.Cache;
using AspireLoanManagement.Utility.CommonEntities;
using AspireLoanManagement.Utility.Logger;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireLoanManagementTest.Business
{
    public class LoanServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAspireLogger> _loggerMock;
        private readonly Mock<IAspireCacheService> _cacheMock;
        private readonly LoanService _loanService;

        public LoanServiceTests()
        {
            _fixture = new Fixture();
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<IAspireLogger>();
            _cacheMock = new Mock<IAspireCacheService>();
            _loanService = new LoanService(_loanRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task AddLoanAsync_WithValidLoan_ShouldReturnLoanModel()
        {
            // Arrange
            var loanModel = _fixture.Create<LoanModelVM>();
            var loanDto = _fixture.Build<LoanModelDTO>().With(x => x.Repayments, new List<RepaymentModelDTO>() { }).Create();
            var expectedLoanVM = _fixture.Create<LoanModelVM>();

            _loanRepositoryMock.Setup(x => x.AddLoanAsync(It.IsAny<LoanModelVM>())).ReturnsAsync(loanDto);
            _mapperMock.Setup(x => x.Map<LoanModelVM>(loanDto)).Returns(expectedLoanVM);

            // Act
            var result = await _loanService.AddLoanAsync(loanModel);

            // Assert
            result.Should().BeEquivalentTo(expectedLoanVM);
            _loggerMock.Verify(logger => logger.Log(LogLevel.Info, It.IsAny<string>()), Times.Exactly(2));
            _loanRepositoryMock.Verify();
            _mapperMock.Verify();
        }

        [Fact]
        public async Task SettleRepayment_WithValidRepayment_ShouldSettleRepayment()
        {
            // Arrange
            var repaymentModel = _fixture.Create<RepaymentModelVM>();
            var loan = _fixture.Build<LoanModelDTO>()
                .With(x => x.Repayments, new List<RepaymentModelDTO>() { })
                .With(x => x.UserId, repaymentModel.UserId)
                .With(x => x.Id, repaymentModel.LoanID)
                .With(x => x.Status, LoanStatus.Approved)
                .With(x => x.Repayments, _fixture.Build<RepaymentModelDTO>()
                    .With(x => x.LoanID, repaymentModel.LoanID)
                    .Without(x => x.LoanModelDTO)
                    .With(x => x.PendingAmount, repaymentModel.RepaymentAmount)
                    .CreateMany(3).ToList())
                .Create();


            _loanRepositoryMock.Setup(x => x.GetLoanByIdAsync(repaymentModel.LoanID)).ReturnsAsync(loan);
            _loanRepositoryMock.Setup(x => x.SettleRepayment(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _loanService.SettleRepayment(repaymentModel);

            // Assert
            result.Should().BeTrue();
            _loanRepositoryMock.Verify(x => x.GetLoanByIdAsync(repaymentModel.LoanID));
            _loanRepositoryMock.Verify(x => x.SettleRepayment(It.IsAny<int>()));
        }

        [Fact]
        public async Task ApproveLoan_WithValidLoanId_ShouldReturnLoanStatus()
        {
            // Arrange
            var loanId = _fixture.Create<int>();
            var expectedLoanStatus = _fixture.Create<LoanStatus>();

            _loanRepositoryMock.Setup(x => x.ApproveLoan(loanId)).ReturnsAsync(expectedLoanStatus);

            // Act
            var result = await _loanService.ApproveLoan(loanId);

            // Assert
            result.Should().Be(expectedLoanStatus);
            _loggerMock.Verify(logger => logger.Log(LogLevel.Info, It.IsAny<string>()), Times.Once);
            _loanRepositoryMock.Verify();
        }

        [Fact]
        public async Task ApproveLoan_WithExceptionThrown_ShouldThrowException()
        {
            // Arrange
            var loanId = _fixture.Create<int>();
            var expectedExceptionMessage = "Loan approval failed";
            var exception = new Exception(expectedExceptionMessage);

            _loanRepositoryMock.Setup(x => x.ApproveLoan(loanId)).ThrowsAsync(exception);

            // Act & Assert
            var thrownException = await Assert.ThrowsAsync<Exception>(() => _loanService.ApproveLoan(loanId));
            thrownException.Message.Should().Be(expectedExceptionMessage);
            _loggerMock.Verify(logger => logger.Log(LogLevel.Info, It.IsAny<string>()), Times.Once);
            _loanRepositoryMock.Verify();
        }

        [Fact]
        public async Task GetLoanByIdAsync_WithLoanInCache_ShouldReturnLoanFromCache()
        {
            // Arrange
            var loanId = _fixture.Create<int>();
            var cachedLoan = _fixture.Create<LoanModelVM>();

            _cacheMock.Setup(x => x.Get<LoanModelVM>(loanId.ToString())).Returns(cachedLoan);

            // Act
            var result = await _loanService.GetLoanByIdAsync(loanId);

            // Assert
            result.Should().BeEquivalentTo(cachedLoan);
            _loggerMock.Verify(logger => logger.Log(LogLevel.Info, It.IsAny<string>()), Times.Once);
            _loanRepositoryMock.Verify(repo => repo.GetLoanByIdAsync(It.IsAny<int>()), Times.Never);
            _cacheMock.Verify(cache => cache.Set(It.IsAny<string>(), It.IsAny<LoanModelVM>(), TimeSpan.FromMinutes(10)), Times.Never);
        }

        [Fact]
        public async Task GetLoanByIdAsync_WithValidLoanIdNotInCache_ShouldReturnLoanModelFromDatabase()
        {
            // Arrange
            var loanId = _fixture.Create<int>();
            var repaymentModel = _fixture.Build<RepaymentModelVM>()
                .With(x => x.LoanID, loanId).Create();
            var loanFromDatabase = _fixture.Build<LoanModelDTO>()
                .With(x => x.Repayments, new List<RepaymentModelDTO>() { })
                .With(x => x.UserId, repaymentModel.UserId)
                .With(x => x.Id, repaymentModel.LoanID)
                .With(x => x.Status, LoanStatus.Approved)
                .With(x => x.Repayments, _fixture.Build<RepaymentModelDTO>()
                    .With(x => x.LoanID, repaymentModel.LoanID)
                    .Without(x => x.LoanModelDTO)
                    .With(x => x.PendingAmount, repaymentModel.RepaymentAmount)
                    .With(x => x.UserId, repaymentModel.UserId)
                    .CreateMany(3).ToList())
                .Create();
            var expectedLoanModel = _fixture.Create<LoanModelVM>();

            _cacheMock.Setup(x => x.Get<LoanModelVM>(loanId.ToString())).Returns((LoanModelVM)null);
            _loanRepositoryMock.Setup(x => x.GetLoanByIdAsync(loanId)).ReturnsAsync(loanFromDatabase);
            _mapperMock.Setup(x => x.Map<LoanModelVM>(loanFromDatabase)).Returns(expectedLoanModel);

            // Act
            var result = await _loanService.GetLoanByIdAsync(loanId);

            // Assert
            result.Should().BeEquivalentTo(expectedLoanModel);
            _loggerMock.Verify(logger => logger.Log(LogLevel.Info, It.IsAny<string>()), Times.Exactly(1));
            _loanRepositoryMock.Verify(repository => repository.GetLoanByIdAsync(loanId), Times.Once);
        }

    }
}
