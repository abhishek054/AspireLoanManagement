﻿using AspireLoanManagement.Business.Models;
using AutoFixture;
using FluentValidation;
using Moq;
using AspireLoanManagement.Controllers;
using FluentAssertions;
using AspireLoanManagement.Business.Loan;
using AspireLoanManagement.Business.Repayment;
using AspireLoanManagement.Utility.CommonEntities;

namespace AspireLoanManagementTest.Controllers
{
    public class LoanControllerTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILoanService> _loanServiceMock;
        private readonly Mock<IRepaymentService> _repaymentServiceMock;
        private readonly Mock<IValidator<LoanModelVM>> _loanValidatorMock;
        private readonly LoanController _loanController;

        public LoanControllerTest()
        {
            _fixture = new Fixture();
            _loanServiceMock = new Mock<ILoanService>();
            _repaymentServiceMock = new Mock<IRepaymentService>();
            _loanValidatorMock = new Mock<IValidator<LoanModelVM>>();
            _loanController = new LoanController(_loanServiceMock.Object);
        }

        [Fact]
        public async Task CreateLoan_WithValidLoanModel_ShouldReturnLoanModel()
        {
            // Arrange
            var validLoan = _fixture.Build<LoanModelVM>().Create();

            _loanServiceMock.Setup(x => x.AddLoanAsync(It.IsAny<LoanModelVM>())).ReturnsAsync(validLoan);

            // Act
            var result = await _loanController.CreateLoan(_fixture.Build<LoanModelVM>().With(x => x.Amount, 20).Create());

            // Assert
            result.Should().BeEquivalentTo(validLoan);
            _loanValidatorMock.Verify();
            _loanServiceMock.Verify();
        }

        [Fact]
        public async Task CreateLoan_WithServiceFailure_ShouldThrowException()
        {
            // Arrange
            var validLoan = _fixture.Build<LoanModelVM>().Create();
            var serviceException = new Exception("Mock service failed");

            _loanServiceMock.Setup(x => x.AddLoanAsync(It.IsAny<LoanModelVM>())).ThrowsAsync(serviceException);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _loanController.CreateLoan(validLoan));
            _loanValidatorMock.Verify();
            _loanServiceMock.Verify();
        }

        [Fact]
        public async Task CreateLoan_WithZeroAmount_ShouldThrowException()
        {
            // Arrange
            var validLoan = _fixture.Build<LoanModelVM>().With(x => x.Amount, 0).Create();

            _loanServiceMock.Setup(x => x.AddLoanAsync(It.IsAny<LoanModelVM>())).ReturnsAsync(validLoan);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _loanController.CreateLoan(validLoan));
            exception.Message.Should().Be("Error Occured");
            _loanValidatorMock.Verify();
            _loanServiceMock.Verify();
        }

        [Fact]
        public async Task ApproveLoan_ValidId_Success()
        {
            // Arrange
            var id = _fixture.Create<int>();
            _loanServiceMock.Setup(x => x.ApproveLoan(It.IsAny<int>())).ReturnsAsync(LoanStatus.Approved);

            // Act
            var result = await _loanController.ApproveLoan(id);

            // Assert
            result.Should().Be(LoanStatus.Approved);
            _loanServiceMock.Verify();

        }
    }
}
