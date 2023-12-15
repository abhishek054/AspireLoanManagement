using AspireLoanManagement.Business.Loan;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Business.Repayment;
using AspireLoanManagement.Repository;
using AspireLoanManagement.Repository.Loan;
using AspireLoanManagement.Repository.Repayment;
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
    public class RepaymentServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILoanRepository> _loanRepositoryMock;
        private readonly Mock<IRepaymentRepository> _repaymentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IAspireLogger> _loggerMock;
        private readonly Mock<IAspireCacheService> _cacheMock;
        private readonly RepaymentService _repaymentService;

        public RepaymentServiceTest()
        {
            _fixture = new Fixture();
            _loanRepositoryMock = new Mock<ILoanRepository>();
            _repaymentRepositoryMock = new Mock<IRepaymentRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<IAspireLogger>();
            _cacheMock = new Mock<IAspireCacheService>();
            _repaymentService = new RepaymentService(_loanRepositoryMock.Object, _loggerMock.Object, _cacheMock.Object, _repaymentRepositoryMock.Object);
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
            _repaymentRepositoryMock.Setup(x => x.SettleRepayment(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            var result = await _repaymentService.SettleRepayment(repaymentModel);

            // Assert
            result.Should().BeTrue();
            _loanRepositoryMock.Verify(x => x.GetLoanByIdAsync(repaymentModel.LoanID));
            _repaymentRepositoryMock.Verify(x => x.SettleRepayment(It.IsAny<int>()));
        }



    }
}
